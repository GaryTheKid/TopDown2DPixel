using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{
    public Axe()
    {
        itemName = "Axe";
        itemID = 3;
        amount = 1;
        itemType = ItemType.MeleeWeapon;
        cursorType = CursorType.Melee;
        durability = 8;

        accuracy = 1f;
        attackRange = 8f;
        attackSpeed = 1f;
        moveSlowDownModifier = 0.7f;
        moveSlowDownTime = 0.4f;

        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 100f,
            knockBackDist = 5f,
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
        return ItemAssets.itemAssets.pfAxe;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.axeSprite;
    }

    public override Sprite GetDurabilitySprite()
    {
        return ItemAssets.itemAssets.ui_icon_melee;
    }
}
    
