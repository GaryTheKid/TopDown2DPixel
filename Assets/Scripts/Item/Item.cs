using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[Serializable]
public class Item
{
    public string itemName;
    public int amount;
    public float useCD;
    public enum ItemType 
    {
        Consumable,
        Material,
        MeleeWeapon,
        RangedWeapon
    }
    public ItemType itemType;
    public Action destroySelfAction;

    // if greater than 0 => in inventory; smaller => in equipment slots 
    public int uiIndex;

    public virtual Sprite GetSprite()
    {
        switch (itemName)
        {
            default:
            case "Sword": return ItemAssets.itemAssets.swordSprite;
        }
    }

    public virtual bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Consumable: return true;
            case ItemType.Material: return true;
            case ItemType.MeleeWeapon: return false;
            case ItemType.RangedWeapon: return false;
        }
    }

    public virtual void UseItem(PhotonView PV)
    {
        Debug.Log(PV + " use this item");
    }

    public virtual void DestroySelf()
    {
        destroySelfAction();
    }
}
