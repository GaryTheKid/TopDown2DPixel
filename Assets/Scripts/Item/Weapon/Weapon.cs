using System;
using UnityEngine;
using Photon.Pun;

[Serializable]
public abstract class Weapon : Item, IEquipable
{
    public int attackDmg;
    public int attackRange;
    public int durability;

    public abstract void Attack();
    public abstract void Equip(PhotonView PV);
    public override void UseItem(PhotonView PV)
    {
        Equip(PV);
    }
    public virtual void Unequip(PhotonView PV)
    {
        NetworkCalls.Weapon.UnequipWeapon(PV);
    }
    public abstract Transform GetEquipmentPrefab();
}
