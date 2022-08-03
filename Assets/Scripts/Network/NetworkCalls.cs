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
    public class Game_Network
    {
        public static void UpdatePlayerInfo(PhotonView PV, int viewID, string name)
        {
            PV.RPC("RPC_UpdatePlayerInfo", RpcTarget.AllBuffered, viewID, name);
        }

        public static void SpawnLootBox(PhotonView PV, int requestedLootBoxIndex, Vector2 pos)
        {
            PV.RPC("RPC_SpawnLootBox", RpcTarget.AllBuffered, requestedLootBoxIndex, pos);
        }

        public static void SpawnAI(PhotonView PV, int requestedAIIndex, Vector2 pos)
        {
            PV.RPC("RPC_SpawnAI", RpcTarget.AllBuffered, requestedAIIndex, pos);
        }

        public static void SpawnItemWorld(PhotonView PV, int requestedAIIndex, Vector2 pos, short itemID, short amount, short durability)
        {
            PV.RPC("RPC_SpawnItemWorld", RpcTarget.AllBuffered, requestedAIIndex, pos, itemID, amount, durability);
        }

        public static void DropItemWorld(PhotonView PV, int requestedAIIndex, float forceDir, Vector2 pos, short itemID, short amount, short durability)
        {
            PV.RPC("RPC_DropItemWorld", RpcTarget.AllBuffered, requestedAIIndex, forceDir, pos, itemID, amount, durability);
        }

        public static void DayToNight(PhotonView PV)
        {
            PV.RPC("RPC_DayToNight", RpcTarget.AllBuffered);
        }

        public static void NightToDay(PhotonView PV)
        {
            PV.RPC("RPC_NightToDay", RpcTarget.AllBuffered);
        }
    }

    public class AI_NetWork
    {
        public static void SetAI(PhotonView PV)
        {
            PV.RPC("RPC_SetAI", RpcTarget.AllBuffered);
        }

        public static void Halt(PhotonView PV)
        {
            PV.RPC("RPC_Halt", RpcTarget.MasterClient);
        }

        public static void Move(PhotonView PV, Vector2 position)
        {
            PV.RPC("RPC_Move", RpcTarget.MasterClient, position);
        }

        public static void Attack(PhotonView PV, Transform target)
        {
            var targetPV = target.GetComponent<PhotonView>();
            if (targetPV.IsMine)
            {
                PV.RPC("RPC_AIDealDamage", RpcTarget.All, targetPV.ViewID);
            }
        }

        public static void Die(PhotonView PV)
        {
            PV.RPC("RPC_AIDie", RpcTarget.AllViaServer);
        }

        public static void Respawn(PhotonView PV)
        {
            PV.RPC("RPC_AIRespawn", RpcTarget.AllViaServer);
        }
    }

    public class LootBox_NetWork
    {
        public static void OpenLootBox(PhotonView PV)
        {
            PV.RPC("RPC_OpenLootBox", RpcTarget.AllBuffered);
        }

        public static void SetLootBox(PhotonView PV, int areaIndex)
        {
            PV.RPC("RPC_SetLootBox", RpcTarget.AllBuffered, areaIndex);
        }

        public static void Expire(PhotonView PV)
        {
            PV.RPC("RPC_LootBoxExpire", RpcTarget.AllBuffered);
        }

        public static void DestroyLootBox(PhotonView PV)
        {
            PV.RPC("RPC_DestroyLootBox", RpcTarget.AllBuffered);
        }
    }

    public class ItemWorld_Network
    {

        // TODO: change this to something inside SpawnItemWorld in Game manager

        /*public static void SetItem(PhotonView PV, short itemID, short amount, short durability)
        {
            PV.RPC("RPC_SetItem", RpcTarget.AllBuffered, itemID, amount, durability);
        }*/

        /*public static void AddForce(PhotonView PV, float dirDeg)
        {
            PV.RPC("RPC_ItemWorldAddForce", RpcTarget.AllBuffered, dirDeg);
        }*/

        public static void Expire(PhotonView PV)
        {
            PV.RPC("RPC_ItemWorldExpire", RpcTarget.AllBuffered);
        }

        public static void DestroyItemWorld(PhotonView PV)
        {
            PV.RPC("RPC_DestroyItemWorld", RpcTarget.AllBuffered);
        }
    }

    public class Consumables_NetWork
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

    public class Spell_Network
    {
        public static void Spell_Blink(PhotonView PV, Vector2 targetPos)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_BlinkSpell", RpcTarget.All, targetPos);
            }
        }

        public static void Spell_Tornado(PhotonView PV, Vector2 targetPos)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_TornadoSpell", RpcTarget.All, targetPos);
            }
        }

        public static void Spell_Meteor(PhotonView PV, Vector2 targetPos)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_MeteorSpell", RpcTarget.All, targetPos);
            }
        }
    }

    public class Weapon_Network
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

        public static void ShowChannelingAnimation(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_ShowChannelingAnimation", RpcTarget.All);
            }
        }

        public static void ShowUnleashAnimation(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_ShowUnleashAnimation", RpcTarget.All);
            }
        }
    }

    public class Player_NetWork
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
        public static void DropItem(PhotonView PV, Vector2 dropPos, short itemID, short amount, short durability)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DropItem", RpcTarget.MasterClient, dropPos, itemID, amount, durability);
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

        public static void DealDamage(PhotonView PV, short whichWeapon)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DealDamage", RpcTarget.AllBuffered, whichWeapon);
            }
        }

        public static void DealProjectileDamage(PhotonView PV, int targetID, float dmgRatio, short whichProjectile)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DealProjectileDamage", RpcTarget.AllBuffered, targetID, dmgRatio, whichProjectile);
            }
        }

        public static void DealSpellDamage(PhotonView PV, int targetID, float dmgRatio, short whichSpell)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DealSpellDamage", RpcTarget.AllBuffered, targetID, dmgRatio, whichSpell);
            }
        }

        public static void LevelUp(PhotonView PV, short newLevel)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_LevelUp", RpcTarget.AllBuffered, newLevel);
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
    }

    public class Physics 
    {

    }
}
