using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class DeployableWeapon : Weapon
{
    public DeployableWeapon()
    {
        itemName = "DeployableWeapon";
        amount = 1;
        itemType = ItemType.DeployableWeapon;
        attackRange = 10f;
        attackSpeed = 12f;
        moveSlowDownModifier = 0.05f;
        castChannelTime = 1f;

        // projectile info
        deployableObject = new Mines_dpl();
        deployableObject.spawnWeapon = this;
    }

    public override void Deploy(PhotonView PV, Vector2 deployPos)
    {
        // shoot projectiles
        NetworkCalls.Weapon_Network.DeployWeapon(PV, deployPos);

        // play sfx
        NetworkCalls.Weapon_Network.PlayOneShotSFX_Deploy(PV);
    }

    public override abstract Transform GetEquipmentPrefab();

    public override bool IsStackable()
    {
        return true;
    }
}
