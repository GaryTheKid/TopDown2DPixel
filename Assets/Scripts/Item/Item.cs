using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[Serializable]
public class Item
{
    public string itemName;
    public short itemID;
    public short amount;
    public short durability;
    public float useCD;
    public bool isEquipped;
    public ItemType itemType;
    public Action destroySelfAction;
    public enum ItemType
    {
        Null,
        Consumable,
        Material,
        MeleeWeapon,
        RangedWeapon,
        ChargableRangedWeapon,
        ThrowableWeapon,
        TargetWeapon,
        Scroll,
    }

    public virtual Sprite GetSprite()
    {
        return null;
    }

    public virtual Sprite GetDurabilitySprite()
    {
        return ItemAssets.itemAssets.ui_icon_none;
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
            case ItemType.ChargableRangedWeapon: return false;
        }
    }

    public virtual void UseItem(PhotonView PV)
    {
        Debug.Log(PV + " use this item");
    }

    public virtual void Equip(PhotonView PV){}

    public virtual void Unequip(PhotonView PV){}

    public virtual void DestroySelf()
    {
        destroySelfAction();
    }
}
