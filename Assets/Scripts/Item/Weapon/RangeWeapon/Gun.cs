using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : Weapon
{
    public Gun()
    {
        itemName = "Gun";
        amount = 1;
        itemType = ItemType.MeleeWeapon;
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

    public override void Attack(PhotonView attackerPV)
    {
        // shoot projectiles
        NetworkCalls.Character.FireProjectile(attackerPV);
    }

    public override void Equip(PhotonView PV)
    {
        isEquiped = true;

        // equip when only in the equipment slots
        if (this.uiIndex < 0)
            NetworkCalls.Weapon.EquipGun(PV);
        else
            Debug.Log("Please drag this weapon into the equipment slots");
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfGun;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.gunSprite;
    }
}
