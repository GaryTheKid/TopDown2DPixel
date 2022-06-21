using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Gun : Weapon
{
    public Gun()
    {
        itemName = "Gun";
        amount = 1;
        itemType = ItemType.RangedWeapon;
        attackRange = 10f;
        attackSpeed = 12f;
        attackMoveSlowRate = 0.1f;
        accuracy = 0.95f;
        recoilForce = 0.5f;
        recoilTime = 0.05f;
        recoilRecoverTime = 0.15f;

        // projectile info
        projectile = new Bullet();
        projectile.spawnWeapon = this;
    }

    public override void Attack(PhotonView attackerPV, Vector2 firePos, float fireDirDeg)
    {
        // shoot projectiles
        NetworkCalls.Weapon_Network.FireProjectile(attackerPV, firePos, fireDirDeg);
    }

    public override abstract Transform GetEquipmentPrefab();
}
