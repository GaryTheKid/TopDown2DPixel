using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hands : Weapon
{
    public Hands()
    {
        itemName = "Hands";
        itemID = 0;
        amount = 1;
        itemType = ItemType.MeleeWeapon;
        cursorType = CursorType.Melee;
        accuracy = 1f;
        attackRange = 10f;
        attackSpeed = 2f;
        moveSlowDownModifier = 0.95f;
        moveSlowDownTime = 0.05f;
        recoilForce = 0f;
        recoilTime = 0f;

        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 5f,
            damageDelay = 0.3f,
            damageEffectTime = 0f,
            KnockBackDist = 0.5f,
        };
    }

    public override void Attack(PhotonView attackerPV)
    {
        // play the animation at userTransform
        NetworkCalls.Weapon_Network.FireWeapon(attackerPV);

        // deal damage to all targets
        NetworkCalls.Player_NetWork.DealDamage(attackerPV, 0);
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfHands;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.handsSprite;
    }
}
