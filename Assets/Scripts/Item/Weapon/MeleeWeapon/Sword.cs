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
        accuracy = 1f;
        attackRange = 10f;
        attackSpeed = 1f;
        attackMoveSlowRate = 0.7f;
        damageInfo = new DamageInfo 
        { 
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 100f,
            damageDelay = 0.3f,
            damageEffectTime = 0f,
            KnockBackDist = 2f,
        };
    }

    public override void Attack(PhotonView attackerPV)
    {
        // play the animation at userTransform
        NetworkCalls.Character.FireWeapon(attackerPV);

        // deal damage to all targets
        NetworkCalls.Character.DealDamage(attackerPV);
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfSword;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.swordSprite;
    }
}
