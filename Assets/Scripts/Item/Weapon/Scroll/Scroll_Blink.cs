using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NetworkCalls;

public class Scroll_Blink : Scroll
{
    public Scroll_Blink()
    {
        itemName = "Scroll: Blink";
        itemID = 16;
        amount = 1;
        itemType = ItemType.Scroll;
        castText = "Blink";
        scollType = ScollType.Light;
        castTargetType = CastTargetType.Self;
        castIndicatorType = CastIndicatorType.Point;
        castTargetAmount = 1;
        castChannelTime = 0.5f;
        castChannelMovementSlotRate = 0.8f;
        unleashDelay = 0.5f;
        invalidCastLayerMask = (1 << LayerMask.NameToLayer("Default")) |
            (1 << LayerMask.NameToLayer("Water")) |
            (1 << LayerMask.NameToLayer("Enemy")) |
            (1 << LayerMask.NameToLayer("Deco")) |
            (1 << LayerMask.NameToLayer("Map_Wall")) |
            (1 << LayerMask.NameToLayer("Character")) |
            (1 << LayerMask.NameToLayer("EnemyAI"));
    }

    public Scroll_Blink(short amount)
    {
        itemName = "Scroll: Blink";
        itemID = 16;
        this.amount = amount;
        itemType = ItemType.Scroll;
        castText = "Blink";
        scollType = ScollType.Light;
        castTargetType = CastTargetType.Self;
        castIndicatorType = CastIndicatorType.Point;
        castTargetAmount = 1;
        castChannelTime = 0.5f;
        castChannelMovementSlotRate = 0.8f;
        unleashDelay = 0.5f;
        invalidCastLayerMask = (1 << LayerMask.NameToLayer("Default")) |
            (1 << LayerMask.NameToLayer("Water")) |
            (1 << LayerMask.NameToLayer("Enemy")) |
            (1 << LayerMask.NameToLayer("Deco")) |
            (1 << LayerMask.NameToLayer("Map_Wall")) |
            (1 << LayerMask.NameToLayer("Character")) |
            (1 << LayerMask.NameToLayer("EnemyAI"));
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
