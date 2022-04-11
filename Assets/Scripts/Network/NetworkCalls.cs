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
        public static void EquipSword(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_EquipSword", RpcTarget.AllBuffered);
            }
        }

        public static void EquipAxe(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_EquipAxe", RpcTarget.AllBuffered);
            }
        }

        public static void EquipBow(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_EquipBow", RpcTarget.AllBuffered);
            }
        }

        public static void UnequipWeapon(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_UnequipWeapon", RpcTarget.AllBuffered);
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

        public static void DealDamage(PhotonView PV, Vector3 attackerPos)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DealDamage", RpcTarget.AllBuffered, attackerPos);
            }
        }
    }
}
