using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sword : Weapon
{
    public Sword()
    {
        itemName = "Sword";
        itemID = 1;
        amount = 1;
        itemType = ItemType.MeleeWeapon;
        cursorType = CursorType.Melee;
        durability = 10;

        accuracy = 1f;
        attackRange = 10f;
        attackSpeed = 1.8f;
        moveSlowDownModifier = 0.8f;
        moveSlowDownTime = 0.35f;

        damageInfo = new DamageInfo 
        { 
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 80f,
            knockBackDist = 2f,
        };
    }

    public override void Attack(PhotonView attackerPV, DamageInfo damageInfo)
    {
        // play the animation at userTransform
        NetworkCalls.Weapon_Network.FireWeapon(attackerPV);

        // deal damage to all targets
        NetworkCalls.Player_NetWork.DealDamage(attackerPV, damageInfo);
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfSword;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.swordSprite;
    }

    public override Sprite GetDurabilitySprite()
    {
        return ItemAssets.itemAssets.ui_icon_melee;
    }
}
