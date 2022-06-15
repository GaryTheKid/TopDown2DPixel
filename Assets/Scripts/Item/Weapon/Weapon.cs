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
    // universal
    public DamageInfo damageInfo;
    public float attackRange;
    public float attackSpeed;
    public float attackMoveSlowRate;
    public float recoilForce;
    public float recoilSpread;
    public float recoilTime;
    public float recoilRecoverTime;

    // ranged
    public Projectile projectile;
    public float chargeSpeed;
    public float chargeMoveSlowRate;
    public float accuracy;
    public int maxChargeTier;

    public virtual void Attack(PhotonView PV) { }
    public virtual void Charge(PhotonView PV) { }
    public override void UseItem(PhotonView PV)
    {
        Equip(PV);
    }
    public override void Equip(PhotonView PV)
    {
        isEquipped = true;
        NetworkCalls.Weapon.EquipWeapon(PV, itemID);
    }
    public override void Unequip(PhotonView PV)
    {
        isEquipped = false;
        NetworkCalls.Weapon.UnequipWeapon(PV);
    }
    public abstract Transform GetEquipmentPrefab();
    public override bool IsStackable()
    {
        return false;
    }
}
