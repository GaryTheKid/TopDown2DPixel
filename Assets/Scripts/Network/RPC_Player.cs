using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_Player : MonoBehaviour
{
    private PlayerWeaponController _playerWeaponController;
    private PlayerInventoryController _playerInventoryController;
    private PlayerBuffController _playerBuffController;
    private PlayerStatsController _playerStatsController;
    private HashSet<int> targets;

    private void Awake()
    {
        _playerWeaponController = GetComponent<PlayerWeaponController>();
        _playerInventoryController = GetComponent<PlayerInventoryController>();
        _playerBuffController = GetComponent<PlayerBuffController>();
        _playerStatsController = GetComponent<PlayerStatsController>();
    }

    private void Start()
    {
        targets = GetComponent<TargetList>().targets;
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
        }
    }

    [PunRPC]
    void RPC_DealProjectileDamage(int targetID, float dmgRatio)
    {
        DamageInfo info = _playerWeaponController.weapon.projectile.damageInfo;
        info.damageAmount *= dmgRatio;
        PhotonView.Find(targetID).transform.GetComponent<PlayerBuffController>().ReceiveDamage(info, transform.position);
    }

    [PunRPC]
    void RPC_FlipWeapon()
    {
        _playerWeaponController.weaponPrefab.localEulerAngles = new Vector3(180f, 0f, 0f);
    }

    [PunRPC]
    void RPC_UnflipWeapon()
    {
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
    void RPC_FireProjectile()
    {
        // reset charge and play attack animation
        _playerWeaponController.weaponAnimator.SetTrigger("Attack");

        // get projectile in weapon controller
        var projectile = _playerWeaponController.weapon.projectile;

        // instantiate and fire (add force)
        var projectilePf = Instantiate(projectile.GetProjectilePrefab(), _playerWeaponController.fireTransform);
        projectilePf.parent = null;
        projectilePf.GetComponent<Rigidbody2D>().AddForce(Utilities.Math.DegreeToVector2(_playerWeaponController.aimTransform.eulerAngles.z) * projectile.speed, ForceMode2D.Impulse);

        // set projectile world script
        var projectileWorld = projectilePf.GetComponent<ProjectileWorld>();
        projectileWorld.SetProjectile(projectile);
        projectileWorld.SetAttackerPV(GetComponent<PhotonView>());
        projectileWorld.Perish();
    }

    [PunRPC]
    void RPC_FireChargedProjectile()
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
            var projectilePf = Instantiate(projectile.GetProjectilePrefab(), _playerWeaponController.aimTransform);
            projectilePf.parent = null;
            projectilePf.GetComponent<Rigidbody2D>().AddForce(Utilities.Math.DegreeToVector2(_playerWeaponController.aimTransform.eulerAngles.z) * projectile.speed * chargeTier, ForceMode2D.Impulse);

            // set projectile world script
            var projectileWorld = projectilePf.GetComponent<ProjectileWorld>();
            projectileWorld.SetProjectile(projectile);
            projectileWorld.SetDamageRatio((float)chargeTier / (float)_playerWeaponController.weapon.maxChargeTier);
            projectileWorld.SetAttackerPV(GetComponent<PhotonView>());
            projectileWorld.Perish();

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
    void RPC_EquipSword()
    {
        Weapon sword = new Sword();
        _playerWeaponController.EquipWeapon(sword);
        _playerWeaponController.weaponType = sword.itemType;
    }

    [PunRPC]
    void RPC_EquipAxe()
    {
        Weapon axe = new Axe();
        _playerWeaponController.EquipWeapon(axe);
        _playerWeaponController.weaponType = axe.itemType;
    }

    [PunRPC]
    void RPC_EquipBow()
    {
        Weapon bow = new Bow();
        _playerWeaponController.EquipWeapon(bow);
        _playerWeaponController.weaponType = bow.itemType;
    }

    [PunRPC]
    void RPC_EquipGun()
    {
        Weapon gun = new Gun_AK();
        _playerWeaponController.EquipWeapon(gun);
        _playerWeaponController.weaponType = gun.itemType;
    }

    [PunRPC]
    void RPC_EquipWeapon(int index)
    {
        _playerWeaponController.EquipWeapon((Weapon)(_playerInventoryController.GetItem(index)));
        _playerWeaponController.weaponType = _playerWeaponController.weapon.itemType;
    }

    [PunRPC]
    void RPC_UnequipWeapon()
    {
        _playerWeaponController.UnequipWeapon();
    }
}