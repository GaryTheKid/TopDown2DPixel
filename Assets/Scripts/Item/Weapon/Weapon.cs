using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public struct DamageInfo
{
    public enum DamageType 
    {
        Physics,
        Spell
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
    public float moveSlowDownModifier;
    public float moveSlowDownTime;
    public float recoilForce;
    public float recoilSpread;
    public float recoilTime;
    public float recoilRecoverTime;

    // ranged
    public Projectile projectile;
    public DeployableObject deployableObject;
    public float chargeSpeed;
    public float chargeMoveSlowRate;
    public float accuracy;
    public int maxChargeTier;

    // castable
    public enum CastTargetType
    {
        Self,
        Other,
        Global
    }
    public enum CastIndicatorType
    {
        Line,
        Point,
        Circle,

    }
    public CastTargetType castTargetType;
    public CastIndicatorType castIndicatorType;
    public LayerMask invalidCastLayerMask;
    public string castText;
    public short castTargetAmount;
    public float castChannelTime;
    public float castChannelMovementSlotRate;
    public float castRange;
    public float castLinearWidth;
    public float castCircleRadius;
    public float unleashDelay;

    // scroll
    public enum ScollType
    {
        Light,
        Air,
        Fire,
        Earth,
        Water
    }
    public ScollType scollType;

    public virtual void Attack(PhotonView PV) { }
    public virtual void Attack(PhotonView PV, Vector2 firePos, float fireDirDeg) { }
    public virtual void Charge(PhotonView PV) { }
    public virtual void Channel(PhotonView PV) { }
    public virtual void Unleash(PhotonView PV, Vector2 targetPos) { }
    public virtual void Deploy(PhotonView PV, Vector2 deployPos) { }
    public override void UseItem(PhotonView PV)
    {
        if (!isEquipped)
            Equip(PV);
        else
            Unequip(PV);
    }
    public override void Equip(PhotonView PV)
    {
        isEquipped = true;
        NetworkCalls.Weapon_Network.EquipWeapon(PV, itemID);
    }
    public override void Unequip(PhotonView PV)
    {
        isEquipped = false;
        NetworkCalls.Weapon_Network.UnequipWeapon(PV);
    }
    public abstract Transform GetEquipmentPrefab();
    public override bool IsStackable()
    {
        return false;
    }
}
