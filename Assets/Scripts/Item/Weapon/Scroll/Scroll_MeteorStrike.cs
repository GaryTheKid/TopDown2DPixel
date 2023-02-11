using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NetworkCalls;

public class Scroll_MeteorStrike : Scroll
{
    public Scroll_MeteorStrike()
    {
        itemName = "Scroll: Meteor Strike";
        itemID = 18;
        amount = 1;
        itemType = ItemType.Scroll;
        cursorType = CursorType.Scroll;
        castText = "Summon Meteor Strike";
        scollType = ScollType.Fire;
        castTargetType = CastTargetType.Other;
        castIndicatorType = CastIndicatorType.Point;
        castTargetAmount = 1;
        castChannelTime = 0.8f;
        castChannelMovementSlotRate = 0.8f;
        unleashDelay = 0.5f;
        invalidCastLayerMask = (1 << LayerMask.NameToLayer("Nothing"));

        damageInfo = new DamageInfo
        {
            damageAmount = 150f,
            damageType = DamageInfo.DamageType.Spell,
            KnockBackDist = 15f
        };
    }

    public Scroll_MeteorStrike(short amount)
    {
        itemName = "Scroll: Meteor Strike";
        itemID = 18;
        this.amount = amount;
        itemType = ItemType.Scroll;
        cursorType = CursorType.Scroll;
        castText = "Summon Meteor Strike";
        scollType = ScollType.Fire;
        castTargetType = CastTargetType.Other;
        castIndicatorType = CastIndicatorType.Point;
        castTargetAmount = 1;
        castChannelTime = 0.8f;
        castChannelMovementSlotRate = 0.8f;
        unleashDelay = 0.5f;
        invalidCastLayerMask = (1 << LayerMask.NameToLayer("Nothing"));

        damageInfo = new DamageInfo
        {
            damageAmount = 150f,
            damageType = DamageInfo.DamageType.Spell,
            KnockBackDist = 15f
        };
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
        Spell_Network.Spell_Meteor(PV, targetPos);
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.meteorStrikeScrollSprite;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfScroll;
    }
}
