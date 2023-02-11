using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NetworkCalls;

public class Scroll_Tornado : Scroll
{
    public Scroll_Tornado()
    {
        itemName = "Scroll: Tornado";
        itemID = 17;
        amount = 1;
        itemType = ItemType.Scroll;
        cursorType = CursorType.Scroll;
        castText = "Summon Tornado";
        scollType = ScollType.Air;
        castTargetType = CastTargetType.Other;
        castIndicatorType = CastIndicatorType.Line;
        castTargetAmount = 1;
        castChannelTime = 1.5f;
        castChannelMovementSlotRate = 0.05f;
        castRange = 5f;
        castLinearWidth = 5f;
        unleashDelay = 0.2f;
    }

    public Scroll_Tornado(short amount)
    {
        itemName = "Scroll: Tornado";
        itemID = 17;
        this.amount = amount;
        itemType = ItemType.Scroll;
        cursorType = CursorType.Scroll;
        castText = "Summon Tornado";
        scollType = ScollType.Air;
        castTargetType = CastTargetType.Other;
        castIndicatorType = CastIndicatorType.Line;
        castTargetAmount = 1;
        castChannelTime = 1.5f;
        castChannelMovementSlotRate = 0.05f;
        castRange = 5f;
        castLinearWidth = 5f;
        unleashDelay = 0.2f;
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
        Spell_Network.Spell_Tornado(PV, targetPos);
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.tornadoScrollSprite;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfScroll;
    }
}
