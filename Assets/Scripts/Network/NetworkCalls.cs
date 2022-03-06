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
}
