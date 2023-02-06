using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Scroll : Weapon
{
    public Scroll()
    {
        itemName = "Scroll";
        amount = 1;
        itemType = ItemType.Scroll;
        castTargetType = CastTargetType.Global;
        castIndicatorType = CastIndicatorType.Circle;
        castTargetAmount = 1;
        castChannelTime = 1f;
        castChannelMovementSlotRate = 0.5f;
        castRange = 10f;
        castLinearWidth = 5f;
        castCircleRadius = 5f;
        unleashDelay = 0.5f;
    }

    public override bool IsStackable()
    {
        return false;
    }

    public override abstract Transform GetEquipmentPrefab();
}
