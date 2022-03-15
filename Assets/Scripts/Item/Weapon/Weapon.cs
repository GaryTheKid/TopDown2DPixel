using System;
using UnityEngine;
using Photon.Pun;

[Serializable]
public abstract class Weapon : Item, IEquipable
{
    public bool isEquiped;
    public float attackDmg;
    public float attackRange;
    public float attackSpeed;

    public abstract void Attack();
    public override void UseItem(PhotonView PV)
    {
        if (!isEquiped)
            Equip(PV);
        else
            Unequip(PV);
    }
    public override void Unequip(PhotonView PV)
    {
        isEquiped = false;
        NetworkCalls.Weapon.UnequipWeapon(PV);
    }
    public abstract Transform GetEquipmentPrefab();
}
