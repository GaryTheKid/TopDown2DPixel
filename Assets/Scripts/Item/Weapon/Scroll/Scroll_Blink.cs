using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NetworkCalls;

public class Scroll_Blink : Scroll
{
    public Scroll_Blink()
    {
        itemName = "Blink Scroll";
        itemID = 16;
        amount = 1;
        itemType = ItemType.Scroll;
        scollType = ScollType.Light;
        castTargetType = CastTargetType.Self;
        castIndicatorType = CastIndicatorType.Point;
        castTargetAmount = 1;
        castChannelTime = 0.5f;
        castChannelMovementSlotRate = 0.8f;
        castRange = 10f;
        castLinearWidth = 5f;
        castCircleRadius = 5f;
        unleashCD = 0.5f;
    }

    public Scroll_Blink(short amount)
    {
        itemName = "Blink Scroll";
        itemID = 16;
        this.amount = amount;
        itemType = ItemType.Scroll;
        scollType = ScollType.Light;
        castTargetType = CastTargetType.Self;
        castIndicatorType = CastIndicatorType.Point;
        castTargetAmount = 1;
        castChannelTime = 0.5f;
        castChannelMovementSlotRate = 0.8f;
        castRange = 10f;
        castLinearWidth = 5f;
        castCircleRadius = 5f;
        unleashCD = 0.5f;
    }

    public override void Channel(PhotonView PV)
    {
        // show animation
        Weapon_Network.ShowChannelingAnimation(PV);
    }

    public override void Unleash(PhotonView PV, Vector2 targetPos)
    {
        // show animation
        Weapon_Network.ShowUnleashAnimation(PV);

        // blink effect
        Spell_Network.Spell_Blink(PV, targetPos);
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.blinkScrollSprite;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfScroll;
    }
}
