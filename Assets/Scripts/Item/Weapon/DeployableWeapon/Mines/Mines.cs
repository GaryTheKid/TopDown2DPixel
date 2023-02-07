using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Mines : DeployableWeapon
{
    public Mines()
    {
        itemName = "Mine-A3";
        itemID = 22;
        amount = 1;
        itemType = ItemType.DeployableWeapon;
        attackRange = 10f;
        attackSpeed = 1f;
        moveSlowDownModifier = 0.05f;
        castChannelTime = 0.2f;

        // deployable info
        deployableObject = new Mines_dpl();
        deployableObject.spawnWeapon = this;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfDeployableMine;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.deployableMineSprite;
    }

    public override Sprite GetDurabilitySprite()
    {
        return ItemAssets.itemAssets.ui_icon_none;
    }
}
