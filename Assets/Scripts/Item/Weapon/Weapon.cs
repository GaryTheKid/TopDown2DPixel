using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public struct DamageInfo
{
    public enum DamageType 
    {
        Physics,
    }
    public DamageType damageType;
    public float damageAmount;
    public float damageDelay;
    public float damageEffectTime;
    public float KnockBackDist;
};

[Serializable]
public abstract class Weapon : Item, IEquipable
{
    public bool isEquiped;
    public DamageInfo damageInfo;
    public float attackRange;
    public float attackSpeed;
    public float frontCD;
    public float midCD;
    public float backCD;

    public abstract void Attack(PhotonView attackerPV, Animator animator, Vector3 attackerPos);
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
