/* Last Edition: 06/11/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The script contains all RPC functions sent from the player PhotonView.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Utilities;

public class RPC_Player : MonoBehaviour
{
    private PlayerWeaponController _playerWeaponController;
    private PlayerInventoryController _playerInventoryController;
    private PlayerBuffController _playerBuffController;
    private PlayerStatsController _playerStatsController;
    private PlayerNetworkController _playerNetworkController;
    private HashSet<int> targets;

    private void Awake()
    {
        _playerWeaponController = GetComponent<PlayerWeaponController>();
        _playerInventoryController = GetComponent<PlayerInventoryController>();
        _playerBuffController = GetComponent<PlayerBuffController>();
        _playerStatsController = GetComponent<PlayerStatsController>();
        _playerNetworkController = GetComponent<PlayerNetworkController>();
    }

    private void Start()
    {
        targets = GetComponent<TargetList>().targets;
    }

    [PunRPC]
    void RPC_SpawnScoreboardTag(string playerID)
    {
        GameManager.gameManager.SpawnScoreboardTag(playerID);
    }

    [PunRPC]
    void RPC_RemoveScoreboardTag(string playerID)
    {
        GameManager.gameManager.DestroyScoreBoardTag(playerID);
    }

    [PunRPC] 
    void RPC_Die()
    {
        _playerWeaponController.UnequipWeapon();
        _playerStatsController.OnDeath.Invoke();
        _playerStatsController.playerStats.isDead = true;
    }

    [PunRPC]
    void RPC_Respawn()
    {
        _playerStatsController.OnRespawn.Invoke();
        _playerStatsController.playerStats.hp = _playerStatsController.playerStats.maxHp;
        _playerStatsController.playerStats.isDead = false;
    }

    [PunRPC]
    void RPC_OpenLootBox(short lootBoxWorldID)
    {
        LootBoxWorld lootBox = GameObject.Find("L" + lootBoxWorldID.ToString()).GetComponent<LootBoxWorld>();
        lootBox.OpenLootBox();
    }

    [PunRPC]
    void RPC_PickItem(short itemWorldID)
    {
        // destroy item world
        Destroy(GameObject.Find(itemWorldID.ToString()));

        // release the item world id
        GameManager.gameManager.ReleaseItemWorldId(itemWorldID);
    }

    [PunRPC]
    void RPC_DropItem(short itemID, short amount, short durability, Vector2 dropPos, float dropDirAngle)
    {
        // get item 
        Item item = ItemAssets.itemAssets.itemDic[itemID];
        Item itemCopy = (Item)Common.GetObjectCopyFromInstance(item);

        // drop item to the world
        var dropDirV2 = Utilities.Math.DegreeToVector2(dropDirAngle);
        ItemWorld itemWorld = ItemWorld.SpawnItemWorld(dropPos + dropDirV2 * 1.2f, itemCopy, GameManager.gameManager.RequestNewItemWorldId(), durability, amount);
        itemWorld.GetComponent<Rigidbody2D>().AddForce(dropDirV2 * 1.5f, ForceMode2D.Impulse);
        itemWorld.Expire(GameManager.gameManager.itemWorldLifeTime);
    }

    [PunRPC]
    void RPC_LockTarget(int targetID)
    {
        targets.Add(targetID);
    }
    
    [PunRPC]
    void RPC_UnlockTarget(int targetID)
    {
        targets.Remove(targetID);
    }

    [PunRPC]
    void RPC_DealDamage()
    {
        DamageInfo info = _playerWeaponController.weapon.damageInfo;

        foreach (int id in targets)
        {
            PhotonView.Find(id).transform.GetComponent<PlayerBuffController>().ReceiveDamage(info, transform.position);

            // add score
            _playerStatsController.UpdateScore((int)info.damageAmount);
            GameManager.gameManager.AddScoreUI(_playerNetworkController.playerID, (int)info.damageAmount);
        }
    }

    [PunRPC]
    void RPC_DealProjectileDamage(int targetID, float dmgRatio)
    {
        DamageInfo info = _playerWeaponController.weapon.projectile.damageInfo;
        info.damageAmount *= dmgRatio;
        PhotonView.Find(targetID).transform.GetComponent<PlayerBuffController>().ReceiveDamage(info, transform.position);

        // add score
        _playerStatsController.UpdateScore((int)info.damageAmount);
        GameManager.gameManager.AddScoreUI(_playerNetworkController.playerID, (int)info.damageAmount);
    }

    [PunRPC]
    void RPC_EquipWeapon(short itemID)
    {
        _playerWeaponController.EquipWeapon((Weapon)(ItemAssets.itemAssets.itemDic[itemID]));
    }

    [PunRPC]
    void RPC_UnequipWeapon()
    {
        _playerWeaponController.UnequipWeapon();
    }

    [PunRPC]
    void RPC_FlipWeapon()
    {
        if(_playerWeaponController.weaponPrefab != null)
            _playerWeaponController.weaponPrefab.localEulerAngles = new Vector3(180f, 0f, 0f);
    }

    [PunRPC]
    void RPC_UnflipWeapon()
    {
        if (_playerWeaponController.weaponPrefab != null)
            _playerWeaponController.weaponPrefab.localEulerAngles = new Vector3(0f, 0f, 0f);
    }

    [PunRPC]
    void RPC_ChargeWeapon()
    {
        int maxCharge = _playerWeaponController.weapon.maxChargeTier;

        // charge tier increment
        if (++_playerWeaponController.chargeTier > maxCharge)
            _playerWeaponController.chargeTier = maxCharge;

        // update sprite animation
        _playerWeaponController.weaponAnimator.SetFloat("ChargeTier", _playerWeaponController.chargeTier / (float)maxCharge);
    }

    [PunRPC]
    void RPC_FireWeapon()
    {
        _playerWeaponController.weaponAnimator.SetTrigger("Attack");
    }

    [PunRPC]
    void RPC_FireProjectile(Vector2 firePos, float fireDirDeg)
    {
        // reset charge and play attack animation
        _playerWeaponController.weaponAnimator.SetTrigger("Attack");

        // get projectile in weapon controller
        var projectile = _playerWeaponController.weapon.projectile;

        // instantiate and fire (add force)
        var dir = Utilities.Math.DegreeToVector2(fireDirDeg);
        var projectilePf = Instantiate(projectile.GetProjectilePrefab(), firePos, Quaternion.identity);
        projectilePf.eulerAngles = new Vector3(0f, 0f, fireDirDeg);

        projectilePf.parent = GameManager.gameManager.spawnedProjectileParent;
        projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed, ForceMode2D.Impulse);

        // set projectile world script
        var projectileWorld = projectilePf.GetComponent<ProjectileWorld>();
        projectileWorld.SetProjectile(projectile);
        projectileWorld.SetAttackerPV(GetComponent<PhotonView>());
        projectileWorld.PerishInTime();
    }

    [PunRPC]
    void RPC_FireChargedProjectile(Vector2 firePos, float fireDirDeg)
    {
        // reset charge and play attack animation
        _playerWeaponController.weaponAnimator.SetTrigger("Attack");
        _playerWeaponController.weaponAnimator.SetFloat("ChargeTier", 0);

        // get projectile in weapon controller
        var projectile = _playerWeaponController.weapon.projectile;

        // check charge tier
        var chargeTier = _playerWeaponController.chargeTier;
        if (chargeTier > 0)
        {
            // instantiate and fire (add force)
            var dir = Utilities.Math.DegreeToVector2(fireDirDeg);
            var projectilePf = Instantiate(projectile.GetProjectilePrefab(), firePos, Quaternion.identity);
            projectilePf.eulerAngles = new Vector3(0f, 0f, fireDirDeg);

            projectilePf.parent = GameManager.gameManager.spawnedProjectileParent;
            projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed * chargeTier, ForceMode2D.Impulse);

            // set projectile world script
            var projectileWorld = projectilePf.GetComponent<ProjectileWorld>();
            projectileWorld.SetProjectile(projectile);
            projectileWorld.SetDamageRatio((float)chargeTier / (float)_playerWeaponController.weapon.maxChargeTier);
            projectileWorld.SetAttackerPV(GetComponent<PhotonView>());
            projectileWorld.PerishInTime();

            // reset chargeTier
            _playerWeaponController.chargeTier = 0;
        }
    }

    [PunRPC]
    void RPC_UseHealthPotion(int healingAmount)
    {
        _playerBuffController.ReceiveHealing(healingAmount);
    }

    [PunRPC]
    void RPC_UseSpeedPotion(float boostAmount, float effectTime)
    {
        _playerBuffController.SpeedBoost(boostAmount, effectTime);
    }

    [PunRPC]
    void RPC_UseInvinciblePotion(float effectTime)
    {
        _playerBuffController.Invincible(effectTime);
    }
}