using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace NetworkCalls
{
    public class Consumables
    {
        public static void UseHealthPotion(PhotonView PV, int healingAmount)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_UseHealthPotion", RpcTarget.AllBuffered, healingAmount);
            }
        }
    }

    public class Weapon
    {
        public static void EquipWeapon(PhotonView PV, int index)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_EquipWeapon", RpcTarget.AllBuffered, index);
            }
        }

        public static void UnequipWeapon(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_UnequipWeapon", RpcTarget.AllBuffered);
            }
        }

        public static void FlipWeapon(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_FlipWeapon", RpcTarget.All);
            }
        }

        public static void UnflipWeapon(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_UnflipWeapon", RpcTarget.All);
            }
        }
    }

    public class Character
    {
        public static void LockTarget(PhotonView PV, int targetID)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_LockTarget", RpcTarget.AllBuffered, targetID);
            }
        }

        public static void UnlockTarget(PhotonView PV, int targetID)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_UnlockTarget", RpcTarget.AllBuffered, targetID);
            }
        }

        public static void DealDamage(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DealDamage", RpcTarget.AllBuffered);
            }
        }

        public static void DealProjectileDamage(PhotonView PV, int targetID, float dmgRatio)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DealProjectileDamage", RpcTarget.AllBuffered, targetID, dmgRatio);
            }
        }

        public static void ChargeWeapon(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_ChargeWeapon", RpcTarget.All);
            }
        }

        public static void FireWeapon(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_FireWeapon", RpcTarget.All);
            }
        }

        public static void FireProjectile(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_FireProjectile", RpcTarget.All);
            }
        }

        public static void FireChargedProjectile(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_FireChargedProjectile", RpcTarget.All);
            }
        }

        public static void Die(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_Die", RpcTarget.AllBuffered);
            }
        }

        public static void Respawn(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_Respawn", RpcTarget.AllBuffered);
            }
        }

        public static void PickItem(PhotonView PV, short itemID, int itemWorldID, int amount, int durability)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_PickItem", RpcTarget.AllBuffered, itemID, itemWorldID, amount, durability);
            }
        }

        public static void DropItem(PhotonView PV, short itemID, int amount, int durability, Vector3 dropDir)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DropItem", RpcTarget.AllBuffered, itemID, amount, durability, dropDir);
            }
        }
    }

    public class Physics 
    {

    }
}
