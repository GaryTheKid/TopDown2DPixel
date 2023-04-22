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
    private PhotonView _PV;
    private PlayerWeaponController _playerWeaponController;
    private PlayerBuffController _playerBuffController;
    private PlayerEffectController _playerEffectController;
    private PlayerStatsController _playerStatsController;
    private PlayerNetworkController _playerNetworkController;
    private PlayerVisionController _playerVisionController;
    private PlayerResourceController _playerResourceController;
    private HashSet<int> targets;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _playerWeaponController = GetComponent<PlayerWeaponController>();
        _playerBuffController = GetComponent<PlayerBuffController>();
        _playerEffectController = GetComponent<PlayerEffectController>();
        _playerStatsController = GetComponent<PlayerStatsController>();
        _playerNetworkController = GetComponent<PlayerNetworkController>();
        _playerVisionController = GetComponent<PlayerVisionController>();
        _playerResourceController = GetComponent<PlayerResourceController>();
    }

    private void Start()
    {
        targets = GetComponent<TargetList>().targets;
    }

    [PunRPC]
    void RPC_SpawnScoreboardTag(string playerID)
    {
        GameManager.singleton.SpawnScoreboardTag(playerID);
    }

    [PunRPC]
    void RPC_RemoveScoreboardTag(string playerID)
    {
        GameManager.singleton.DestroyScoreBoardTag(playerID);
    }

    [PunRPC]
    void RPC_LevelUp(short newLevel)
    {
        _playerEffectController.LevelUpEffect(newLevel);
        _playerStatsController.UpdateMaxExp(PlayerStatsController.GetMaxExpBasedOnLevel(newLevel) - _playerStatsController.playerStats.maxExp);
        _playerStatsController.UpdateMaxHP(PlayerStatsController.GetMaxHpBasedOnLevel(newLevel) - _playerStatsController.playerStats.maxHp);
        _playerStatsController.UpdateWorthExp(PlayerStatsController.GetWorthExpBasedOnLevel(newLevel) - _playerStatsController.playerStats.expWorth);
        _playerStatsController.UpdateWorthGold(PlayerStatsController.GetWorthGoldBasedOnLevel(newLevel) - _playerStatsController.playerStats.goldWorth);
        // TODO: perk system!!!!
    }

    [PunRPC]
    void RPC_GainGold(short amount)
    {
        _playerResourceController.GainGold(amount);
    }

    [PunRPC]
    void RPC_LoseGold(short amount)
    {
        _playerResourceController.LoseGold(amount);
    }

    [PunRPC]
    void RPC_SturdyBody(int maxHpBonus)
    {
        _playerStatsController.UpdateMaxExp(maxHpBonus);
    }

    [PunRPC]
    void RPC_Regeneration(int regenAmount)
    {
        _playerBuffController.Regeneration(regenAmount);
    }

    [PunRPC] 
    void RPC_Die()
    {
        _playerStatsController.OnDeath.Invoke();
        _playerWeaponController.UnequipWeapon();
        _playerWeaponController.UnequipHands();
        _playerStatsController.playerStats.isDead = true;
        _playerStatsController.playerStats.isRespawnable = false;
    }

    [PunRPC]
    void RPC_Respawn()
    {
        _playerStatsController.OnRespawn.Invoke();
        _playerWeaponController.EquipHands();
        _playerStatsController.playerStats.isDead = false;
        _playerStatsController.playerStats.isRespawnable = false;
    }

    [PunRPC]
    void RPC_DropItem(Vector2 pos, short itemID, short amount, short durability)
    {
        GameManager.singleton.DropItem(pos, itemID, amount, durability);
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
    void RPC_DealDamage(short whichWeapon)
    {
        DamageInfo info = ((Weapon)(ItemAssets.itemAssets.itemDic[whichWeapon])).damageInfo;

        foreach (int id in targets)
        {
            var target = PhotonView.Find(id).transform;
            var enemyPlayer = target.GetComponent<PlayerBuffController>();
            var enemyAI = target.GetComponent<AIBuffController>();

            if (enemyPlayer != null)
            {
                enemyPlayer.ReceiveDamage(_PV.ViewID, info, transform.position);
            }

            if (enemyAI != null)
            {
                enemyAI.ReceiveDamage(_PV.ViewID, info, transform.position);
            }
        }
    }

    [PunRPC]
    void RPC_DealProjectileDamage(int targetID, float dmgRatio, short whichProjectile)
    {
        DamageInfo info = ItemAssets.itemAssets.projectileDic[whichProjectile].damageInfo;
        info.damageAmount *= dmgRatio;

        var target = PhotonView.Find(targetID).transform;
        var enemyPlayer = target.GetComponent<PlayerBuffController>();
        var enemyAI = target.GetComponent<AIBuffController>();

        if (enemyPlayer != null)
        {
            enemyPlayer.ReceiveDamage(_PV.ViewID, info, transform.position);
        }

        if (enemyAI != null)
        {
            enemyAI.ReceiveDamage(_PV.ViewID, info, transform.position);
        }
    }

    [PunRPC]
    void RPC_DealDeployableDamage(int targetID, float dmgRatio, short whichDeployable)
    {
        DamageInfo info = ItemAssets.itemAssets.deployableObjDic[whichDeployable].damageInfo;
        info.damageAmount *= dmgRatio;

        var target = PhotonView.Find(targetID).transform;
        var enemyPlayer = target.GetComponent<PlayerBuffController>();
        var enemyAI = target.GetComponent<AIBuffController>();

        if (enemyPlayer != null)
        {
            enemyPlayer.ReceiveDamage(_PV.ViewID, info, transform.position);
        }

        if (enemyAI != null)
        {
            enemyAI.ReceiveDamage(_PV.ViewID, info, transform.position);
        }
    }

    [PunRPC]
    void RPC_DealSpellDamage(int targetID, float dmgRatio, short whichSpell)
    {
        DamageInfo info = ((Weapon)ItemAssets.itemAssets.itemDic[whichSpell]).damageInfo;
        info.damageAmount *= dmgRatio;

        var target = PhotonView.Find(targetID).transform;
        var enemyPlayer = target.GetComponent<PlayerBuffController>();
        var enemyAI = target.GetComponent<AIBuffController>();

        if (enemyPlayer != null)
        {
            enemyPlayer.ReceiveDamage(_PV.ViewID, info, transform.position);
        }

        if (enemyAI != null)
        {
            enemyAI.ReceiveDamage(_PV.ViewID, info, transform.position);
        }
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
        var clip = _playerWeaponController.fireFX.clip;
        _playerWeaponController.fireFX.PlayOneShot(clip);
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
        if (!_PV.IsMine && projectilePf.gameObject.layer == LayerMask.NameToLayer("Projectile")) projectilePf.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile"); 
        projectilePf.eulerAngles = new Vector3(0f, 0f, fireDirDeg);

        // set parent
        projectilePf.parent = GameManager.singleton.spawnedProjectileParent;

        // add force based on if windy weather
        switch (GameManager.singleton.weather)
        {
            case GameManager.Weather.Windy_East:
                projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed + Vector2.left * PlayerBuffController.WIND_FORCE_PROJECTILE, ForceMode2D.Impulse);
                break;
            case GameManager.Weather.Windy_West:
                projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed + Vector2.right * PlayerBuffController.WIND_FORCE_PROJECTILE, ForceMode2D.Impulse);
                break;
            case GameManager.Weather.Windy_North:
                projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed + Vector2.down * PlayerBuffController.WIND_FORCE_PROJECTILE, ForceMode2D.Impulse);
                break;
            case GameManager.Weather.Windy_South:
                projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed + Vector2.up * PlayerBuffController.WIND_FORCE_PROJECTILE, ForceMode2D.Impulse);
                break;
            default:
                projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed, ForceMode2D.Impulse);
                break;
        }

        // set projectile world script
        var projectileWorld = projectilePf.GetComponent<ProjectileWorld>();
        projectileWorld.SetProjectile(projectile);
        projectileWorld.SetAttackerPV(_PV);
        projectileWorld.PerishInTime();
    }

    [PunRPC]
    void RPC_FireChargedProjectile(Vector2 firePos, float fireDirDeg)
    {
        // reset charge and play attack animation
        _playerWeaponController.weaponAnimator.SetTrigger("Attack");
        _playerWeaponController.weaponAnimator.SetFloat("ChargeTier", 0);

        // play sound fx
        _playerWeaponController.fireFX.Play();

        // get projectile in weapon controller
        var projectile = _playerWeaponController.weapon.projectile;

        // check charge tier
        var chargeTier = _playerWeaponController.chargeTier;
        if (chargeTier > 0)
        {
            // instantiate and fire (add force)
            var dir = Utilities.Math.DegreeToVector2(fireDirDeg);
            var projectilePf = Instantiate(projectile.GetProjectilePrefab(), firePos, Quaternion.identity);
            if (!_PV.IsMine && projectilePf.gameObject.layer == LayerMask.NameToLayer("Projectile")) projectilePf.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
            projectilePf.eulerAngles = new Vector3(0f, 0f, fireDirDeg);

            // set parent
            projectilePf.parent = GameManager.singleton.spawnedProjectileParent;

            // add force based on if windy weather
            switch (GameManager.singleton.weather)
            {
                case GameManager.Weather.Windy_East:
                    projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed * chargeTier + Vector2.left * PlayerBuffController.WIND_FORCE_PROJECTILE, ForceMode2D.Impulse);
                    break;
                case GameManager.Weather.Windy_West:
                    projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed * chargeTier + Vector2.right * PlayerBuffController.WIND_FORCE_PROJECTILE, ForceMode2D.Impulse);
                    break;
                case GameManager.Weather.Windy_North:
                    projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed * chargeTier + Vector2.down * PlayerBuffController.WIND_FORCE_PROJECTILE, ForceMode2D.Impulse);
                    break;
                case GameManager.Weather.Windy_South:
                    projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed * chargeTier + Vector2.up * PlayerBuffController.WIND_FORCE_PROJECTILE, ForceMode2D.Impulse);
                    break;
                default:
                    projectilePf.GetComponent<Rigidbody2D>().AddForce(dir * projectile.speed * chargeTier, ForceMode2D.Impulse);
                    break;
            }

            // set projectile world script
            var projectileWorld = projectilePf.GetComponent<ProjectileWorld>();
            projectileWorld.SetProjectile(projectile);
            if (!projectile.isExplosive)
                projectileWorld.SetDamageRatio((float)chargeTier / (float)_playerWeaponController.weapon.maxChargeTier);
            else
                projectileWorld.SetDamageRatio(1f);
            projectileWorld.SetAttackerPV(GetComponent<PhotonView>());
            projectileWorld.PerishInTime();

            // reset chargeTier
            _playerWeaponController.chargeTier = 0;
        }
    }

    [PunRPC]
    void RPC_DeployWeapon(Vector2 deployPos)
    {
        _playerWeaponController.weaponAnimator.SetTrigger("Deploy");

        // get deployable object in weapon controller
        var deployableObj = _playerWeaponController.weapon.deployableObject;

        // instantiate and fire (add force)
        int requestedItemWorldIndex = ObjectPool.objectPool.RequestDeployableIndexFromPool();

        if (requestedItemWorldIndex != -1)
        {
            var obj = ObjectPool.objectPool.pooledDeployableObjs[requestedItemWorldIndex].gameObject;

            // set deployable object world
            var deployableWorld = obj.GetComponent<DeployableObject_World>();
            deployableWorld.SetDeployableObject(deployableObj);
            deployableWorld.SetDeployerPV(_PV);

            // set deployable active
            obj.SetActive(true);
            obj.transform.position = deployPos;

            // set deployable perish
            deployableWorld.PerishInTime();
        }
    }

    [PunRPC]
    void RPC_PlayOneShotSFX_Projectile()
    {
        // play sound fx
        var clip = _playerWeaponController.fireFX.clip;
        _playerWeaponController.fireFX.PlayOneShot(clip);
    }

    [PunRPC]
    void RPC_PlayOneShotSFX_Deploy()
    {
        // play sound fx
        var clip = _playerWeaponController.fireFX.clip;
        _playerWeaponController.fireFX.PlayOneShot(clip);
    }

    #region Spell
    [PunRPC]
    void RPC_ShowChannelingAnimation()
    {
        // reset charge and play attack animation
        _playerWeaponController.weaponAnimator.SetTrigger("Channel");
    }

    [PunRPC]
    void RPC_ShowUnleashAnimation()
    {
        // reset charge and play attack animation
        _playerWeaponController.weaponAnimator.SetTrigger("Unleash");
    }

    [PunRPC]
    void RPC_BlinkSpell(Vector2 targetPos)
    {
        transform.position = targetPos;
        _playerEffectController.BlinkEffect();
    }

    [PunRPC]
    void RPC_TornadoSpell(Vector2 targetPos)
    {
        // instantiate
        Vector2 initPos = transform.position;
        var pfTornado = Instantiate(ItemAssets.itemAssets.pfSpell_Tornado, initPos + (targetPos - initPos) * 0.15f, Quaternion.identity, GameManager.singleton.FXParent);

        // set fx controller
        var tornadoFX = pfTornado.GetComponent<TornadoFX>();
        tornadoFX.MoveTowards(initPos, targetPos);
        tornadoFX.Expire();
    }

    [PunRPC]
    void RPC_MeteorSpell(Vector2 targetPos)
    {
        // instantiate
        var pfMeteor = Instantiate(ItemAssets.itemAssets.pfSpell_Meteor, targetPos, Quaternion.identity, GameManager.singleton.FXParent);

        // set fx controller
        var spellMeteor = pfMeteor.GetComponent<SpellMeteor>();
        spellMeteor.attackerPV = _PV;
        spellMeteor.spellID = 18;
        spellMeteor.explosiveRadius = 4f;
        spellMeteor.explosiveSize_x = 7.5f;
        spellMeteor.explosiveSize_y = 5.5f;
    }
    #endregion

    #region Vision
    [PunRPC]
    void RPC_UpdateVision(float newVisionRadius)
    {
        _playerVisionController.UpdateVision(newVisionRadius);
    }
    #endregion

    [PunRPC]
    void RPC_UseHealthPotion(int healingAmount)
    {
        _playerBuffController.ConsumeHealingPotion(healingAmount);
    }

    [PunRPC]
    void RPC_UseSpeedPotion(float boostAmount, float effectTime)
    {
        _playerBuffController.ConsumeSpeedBoostPotion(boostAmount, effectTime);
    }

    [PunRPC]
    void RPC_UseInvinciblePotion(float effectTime)
    {
        _playerBuffController.ConsumeInvinciblePotion(effectTime);
    }
}