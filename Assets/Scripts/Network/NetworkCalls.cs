/* Last Edition: 06/11/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The collection of all networked function calls, each call is connected to one PRC function.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace NetworkCalls
{
    public class Game
    {
        public static void UpdatePlayerInfo(PhotonView PV, int viewID, string name)
        {
            PV.RPC("RPC_UpdatePlayerInfo", RpcTarget.AllBuffered, viewID, name);
        }

        public static void SpawnLootBox(PhotonView PV, Vector2 pos, int whichArea)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("RPC_SpawnLootBox", RpcTarget.AllBuffered, pos, whichArea);
            }
        }

        public static void SpawnItem(PhotonView PV, Vector2 pos, short itemID)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("RPC_SpawnItem", RpcTarget.AllBuffered, pos, itemID);
            }
        }

        public static void SpawnItems(PhotonView PV, Vector2 pos, short itemID, short amount)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("RPC_SpawnItems", RpcTarget.AllBuffered, pos, itemID, amount);
            }
        }

        public static void DestroyLootBox(PhotonView PV, short lootBoxID)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("RPC_DestroyLootBox", RpcTarget.AllBuffered, lootBoxID);
            }
        }
    }

    public class Consumables
    {
        public static void UseHealthPotion(PhotonView PV, int healingAmount)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_UseHealthPotion", RpcTarget.AllBuffered, healingAmount);
            }
        }

        public static void UseSpeedPotion(PhotonView PV, float boostAmount, float effectTime)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_UseSpeedPotion", RpcTarget.AllBuffered, boostAmount, effectTime);
            }
        }

        public static void UseInvinciblePotion(PhotonView PV, float effectTime)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_UseInvinciblePotion", RpcTarget.AllBuffered, effectTime);
            }
        }
    }

    public class Weapon
    {
        public static void EquipWeapon(PhotonView PV, short itemID)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_EquipWeapon", RpcTarget.AllBuffered, itemID);
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
        public static void SpawnScoreboardTag(PhotonView PV, string playerID)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_SpawnScoreboardTag", RpcTarget.AllBuffered, playerID);
            }
        }

        public static void RemoveScoreboardTag(PhotonView PV, string playerID)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_RemoveScoreboardTag", RpcTarget.AllBuffered, playerID);
            }
        }

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

        public static void FireProjectile(PhotonView PV, Vector2 firePos, float fireDirDeg)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_FireProjectile", RpcTarget.All, firePos, fireDirDeg);
            }
        }

        public static void FireChargedProjectile(PhotonView PV, Vector2 firePos, float fireDirDeg)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_FireChargedProjectile", RpcTarget.All, firePos, fireDirDeg);
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

        public static void OpenLootBox(PhotonView PV, short lootBoxWorldID)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_OpenLootBox", RpcTarget.AllBuffered, lootBoxWorldID);
            }
        }

        public static void PickItem(PhotonView PV, short itemWorldID)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_PickItem", RpcTarget.AllBuffered, itemWorldID);
            }
        }

        public static void DropItem(PhotonView PV, short itemID, short amount, short durability, Vector2 dropPos, float dropDirAngle)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DropItem", RpcTarget.AllBuffered, itemID, amount, durability, dropPos, dropDirAngle);
            }
        }
    }

    public class Physics 
    {

    }
}
