using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun_Shotgun : Gun
{
    public Gun_Shotgun()
    {
        itemName = "Shotgun";
        itemID = 19;
        amount = 1;
        itemType = ItemType.RangedWeapon;
        durability = 25;

        attackRange = 10f;
        attackSpeed = 3.5f;
        moveSlowDownModifier = 0.1f;
        moveSlowDownTime = 0.15f;
        accuracy = 0.97f;
        recoilForce = 4.5f;
        recoilTime = 0.03f;
        recoilRecoverTime = 0.12f;

        // projectile info
        projectile = new Bullet_SemiAuto();
        projectile.spawnWeapon = this;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfGun_M4;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.gunSprite_M4;
    }

    public override Sprite GetDurabilitySprite()
    {
        return ItemAssets.itemAssets.ui_icon_semiAutoBullets;
    }

    public override void Attack(PhotonView attackerPV, Vector2 firePos, float fireDirDeg)
    {
        base.Attack(attackerPV, firePos, fireDirDeg + 15f);
        base.Attack(attackerPV, firePos, fireDirDeg + 5f);
        base.Attack(attackerPV, firePos, fireDirDeg - 5f);
        base.Attack(attackerPV, firePos, fireDirDeg - 15f);
    }
}
