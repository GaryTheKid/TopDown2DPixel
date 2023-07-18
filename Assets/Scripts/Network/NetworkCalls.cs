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
        public static void SwitchGameState(PhotonView PV, byte newState)
        {
            PV.RPC("RPC_SwitchGameState", RpcTarget.AllBuffered, newState);
        }

        public static void UpdatePlayerInfo(PhotonView PV, int viewID, string name)
        {
            PV.RPC("RPC_UpdatePlayerInfo", RpcTarget.AllBuffered, viewID, name);
        }

        public static void UpdatePlayerPingTier(PhotonView PV, byte playerActorNumber, byte pingTier)
        {
            PV.RPC("RPC_UpdatePlayerPingTier", RpcTarget.AllBuffered, playerActorNumber, pingTier);
        }

        public static void ReceiveChatMessage(PhotonView PV, string message, byte actorNumber)
        {
            PV.RPC("ReceiveChatMessage", RpcTarget.All, message, actorNumber);
        }

        public static void SpawnLootBox(PhotonView PV, int requestedLootBoxIndex, Vector2 pos)
        {
            PV.RPC("RPC_SpawnLootBox", RpcTarget.AllBuffered, requestedLootBoxIndex, pos);
        }

        public static void SpawnAI(PhotonView PV, int requestedAIIndex, Vector2 pos, byte enemyID)
        {
            PV.RPC("RPC_SpawnAI", RpcTarget.AllBuffered, requestedAIIndex, pos, enemyID);
        }

        public static void SpawnMerchant(PhotonView PV, int requestedMerchantIndex, Vector2 pos)
        {
            PV.RPC("RPC_SpawnMerchant", RpcTarget.AllBuffered, requestedMerchantIndex, pos);
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

        public static void ChangeWeather(PhotonView PV, byte weatherCode)
        {
            PV.RPC("RPC_ChangeWeather", RpcTarget.AllBuffered, weatherCode);
        }

        public static void LockAllPlayersActions(PhotonView PV)
        {
            PV.RPC("RPC_LockAllPlayersActions", RpcTarget.All);
        }

        public static void GoToResultScene(PhotonView PV)
        {
            PV.RPC("RPC_GoToResultScene", RpcTarget.AllBuffered);
        }

        public static void IndividualObjectiveBeforeActivationNotification(PhotonView PV, byte index)
        {
            PV.RPC("RPC_GlobalAnnouncement_IndividualObjectiveBeforeActivationNotification", RpcTarget.All, index);
        }

        public static void ObjectiveBeforeActivationNotification(PhotonView PV)
        {
            PV.RPC("RPC_GlobalAnnouncement_ObjectiveBeforeActivationNotification", RpcTarget.All);
        }

        public static void ObjectiveHasBeenActivatedNotification(PhotonView PV)
        {
            PV.RPC("RPC_GlobalAnnouncement_ObjectiveActivation", RpcTarget.All);
        }

        public static void ShowLevelNameNotification(PhotonView PV)
        {
            PV.RPC("RPC_GlobalAnnouncement_ShowLevelName", RpcTarget.All);
        }

        public static void AnnounceGameEnd(PhotonView PV)
        {
            PV.RPC("RPC_GlobalAnnouncement_AnnounceGameEnd", RpcTarget.All);
        }
    }

    public class Objective_Network
    {
        public static void SendObjectiveCaptureMessage(PhotonView PV, byte playerActorNumber)
        {
            PV.RPC("RPC_SendObjectiveCaptureMessage", RpcTarget.AllBufferedViaServer, playerActorNumber);
        }

        public static void ResetAndActivateObjective(PhotonView PV)
        {
            PV.RPC("RPC_ResetAndActivateObjective", RpcTarget.AllBufferedViaServer);
        }

        public static void ObjectiveActivationGlobalAnnouncement(PhotonView PV)
        {
            PV.RPC("RPC_GlobalAnnouncement_ObjectiveActivation", RpcTarget.All);
        }
    }

    public class AI_NetWork
    {
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

        public static void Respawn(PhotonView PV, byte enemyID)
        {
            PV.RPC("RPC_AIRespawn", RpcTarget.AllViaServer, enemyID);
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

    public class Merchant_NetWork 
    {
        public static void SetVenderItems(PhotonView PV, byte itemIndex, short randItemID, short amount, short durability, short price)
        {
            PV.RPC("RPC_SetVenderItems", RpcTarget.AllBuffered, itemIndex, randItemID, amount, durability, price);
        }

        public static void SetVenderGambleItems(PhotonView PV, byte itemIndex, short randItemID, short amount, short durability, short price)
        {
            PV.RPC("RPC_SetVenderGambleItems", RpcTarget.AllBuffered, itemIndex, randItemID, amount, durability, price);
        }

        public static void RemoveVenderItem(PhotonView PV, byte itemIndex)
        {
            PV.RPC("RPC_RemoveVenderItem", RpcTarget.AllBuffered, itemIndex);
        }

        public static void Expire(PhotonView PV)
        {
            PV.RPC("RPC_MerchantExpire", RpcTarget.AllBuffered);
        }

        public static void DestroyMerchant(PhotonView PV)
        {
            PV.RPC("RPC_DestroyMerchant", RpcTarget.AllBuffered);
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

    public class MapObj_Network
    {
        public static void DrinkFromWell(PhotonView PV)
        {
            PV.RPC("RPC_DrinkFromWell", RpcTarget.AllBuffered);
        }
        
        public static void Prepare(PhotonView PV, int teleportTargetID)
        {
            PV.RPC("RPC_TeleportPrepare", RpcTarget.AllBuffered, teleportTargetID);
        }

        public static void Reset(PhotonView PV)
        {
            PV.RPC("RPC_TeleportReset", RpcTarget.AllBuffered);
        }

        public static void Functioning(PhotonView PV)
        {
            PV.RPC("RPC_TeleportFunctioning", RpcTarget.AllBuffered);
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

        public static void DeployWeapon(PhotonView PV, Vector2 deployPos)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DeployWeapon", RpcTarget.All, deployPos);
            }
        }

        public static void FireProjectile(PhotonView PV, Vector2 firePos, float fireDirDeg)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_FireProjectile", RpcTarget.All, firePos, fireDirDeg);
            }
        }

        public static void PlayOneShotSFX_Deploy(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_PlayOneShotSFX_Deploy", RpcTarget.All);
            }
        }

        public static void PlayOneShotSFX_Projectile(PhotonView PV)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_PlayOneShotSFX_Projectile", RpcTarget.All);
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

    public class Skill_Network
    {
        public static void SturdyBody(PhotonView PV, int bonusHP)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_SturdyBody", RpcTarget.AllBuffered, bonusHP);
            }
        }

        public static void Regeneration(PhotonView PV, int regenAmount)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_Regeneration", RpcTarget.AllBuffered, regenAmount);
            }
        }

        public static void HolySacrifice(PhotonView PV, float dmgAmount)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_HolySacrifice", RpcTarget.AllBuffered, dmgAmount);
            }
        }

        public static void SecondLife(PhotonView PV, float respawnReductionTime)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_SecondLife", RpcTarget.AllBuffered, respawnReductionTime);
            }
        }

        public static void PiggyBank(PhotonView PV, int goldAmount)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_PiggyBank", RpcTarget.AllBuffered, goldAmount);
            }
        }

        public static void EagleEyes(PhotonView PV, float deltaVision)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_EagleEyes", RpcTarget.AllBuffered, deltaVision);
            }
        }
    }

    public class Deployable_Network
    {
        public static void ActivateDeployable(PhotonView PV)
        {
            PV.RPC("RPC_ActivateDeployable", RpcTarget.AllBuffered);
        }

        public static void DeactivateDeployable(PhotonView PV)
        {
            PV.RPC("RPC_DeactivateDeployable", RpcTarget.AllBuffered);
        }
    }

    public class Player_NetWork
    {
        public static void SpawnScoreboardTag(PhotonView PV, byte playerActorNumber)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_SpawnScoreboardTag", RpcTarget.AllBuffered, playerActorNumber);
            }
        }

        public static void RemoveScoreboardTag(PhotonView PV, byte playerActorNumber)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_RemoveScoreboardTag", RpcTarget.AllBuffered, playerActorNumber);
            }
        }

        public static void Emote(PhotonView PV, byte index)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_Emote", RpcTarget.All, index);
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

        public static void DealDamage(PhotonView PV, DamageInfo damageInfo)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DealDamage_Melee", RpcTarget.AllBuffered, damageInfo);
            }
        }

        public static void DealDamage(PhotonView PV, int targetID, DamageInfo damageInfo)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_DealDamage", RpcTarget.AllBuffered, targetID, damageInfo);
            }
        }

        public static void LevelUp(PhotonView PV, short newLevel)
        {
            if (PV.IsMine)
            {
                PV.RPC("RPC_LevelUp", RpcTarget.AllBuffered, newLevel);
            }
        }

        public static void GainGold(PhotonView PV, short amount)
        {
            PV.RPC("RPC_GainGold", RpcTarget.AllBuffered, amount);
        }

        public static void LoseGold(PhotonView PV, short amount)
        {
            PV.RPC("RPC_LoseGold", RpcTarget.AllBuffered, amount);
        }

        public static void UpdateVision(PhotonView PV, float newVisionRadius)
        {
            PV.RPC("RPC_UpdateVision", RpcTarget.AllBuffered, newVisionRadius);
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
