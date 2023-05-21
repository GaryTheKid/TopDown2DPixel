/* Last Edition: 08/02/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The stats controller that holds all player stats 
 * Last Edition:
 *   Add exp, level up.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Photon.Pun;

[Serializable]
public class PlayerStats
{
    public bool isDead;
    public bool isInvincible;
    public bool isInventoryLocked;
    public bool isMovementLocked;
    public bool isWeaponLocked;
    public bool isRespawnable;
    public int maxHp;
    public int hp;
    public int maxExp;
    public int exp;
    public short level;
    public int expWorth;
    public int gold;
    public int goldWorth;
    public int maxActiveDeployableObjects;
    public float baseSpeed;
    public float speedModifier;
    public float respawnCD;
    public int score;

    public PlayerStats()
    {
        isDead = false;
        isInvincible = false;
        isInventoryLocked = false;
        isMovementLocked = false;
        isWeaponLocked = false;
        isRespawnable = false;
        maxHp = 100;
        hp = 100;
        maxExp = 50;
        exp = 0;
        level = 1;
        expWorth = 30;
        gold = 0;
        goldWorth = 10;
        maxActiveDeployableObjects = 5;
        baseSpeed = 30f;
        speedModifier = 1f;
        respawnCD = 8f;
        score = 0;
    }
}

public class PlayerStatsController : MonoBehaviour
{
    private const int BASE_EXP = 50;
    private const int BASE_WORTH_EXP = 30;
    private const int BASE_HP = 100;
    private const int BASE_WORTH_GOLD = 10;

    public static int GetMaxHpBasedOnLevel(short level)
    {
        return BASE_HP + level * 10 + 2 * level * level;
    }

    public static int GetMaxExpBasedOnLevel(short level)
    {
        return BASE_EXP + level * 30 + 5 * level * level;
    }

    public static int GetWorthExpBasedOnLevel(short level)
    {
        return BASE_WORTH_EXP + level * 30 + 2 * level * level;
    }

    public static int GetWorthGoldBasedOnLevel(short level)
    {
        return BASE_WORTH_GOLD + level * 3 + (int)(0.5f * level * level);
    }

    public PlayerStats playerStats = new PlayerStats();
    public UnityEvent OnDeath;
    public UnityEvent OnRespawn;

    private PhotonView _PV;
    private PlayerEffectController _playerEffectController;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _playerEffectController = GetComponent<PlayerEffectController>();
        GetComponent<PlayerInputActions>().inputActions.Player.Respawn.performed += Respawn;
    }

    // restore to max hp
    public void RestoreFullHp()
    {
        playerStats.hp = playerStats.maxHp;
        _playerEffectController.ReceiveHealingEffect(playerStats.maxHp, playerStats.hp, playerStats.maxHp);
    }

    // regeneration hp
    public void Regeneration(int deltaHP)
    {
        _playerEffectController.RegenerationEffect(deltaHP, playerStats.hp, playerStats.maxHp);

        playerStats.hp = playerStats.hp + deltaHP <= playerStats.maxHp ?
            playerStats.hp + deltaHP : playerStats.maxHp;
    }

    // update hp after receive damage or healing
    public void UpdateHP(int deltaHP, out bool isKilled)
    {
        // receive dmg
        if (deltaHP < 0)
        {
            Debug.Log("Hp " + deltaHP);
            _playerEffectController.ReceiveDamageEffect(deltaHP, playerStats.hp, playerStats.maxHp);

            playerStats.hp = playerStats.hp + deltaHP >= 0 ?
                playerStats.hp + deltaHP : 0;
        }
        // receive healing
        else
        {
            Debug.Log("HP +" + deltaHP);
            _playerEffectController.ReceiveHealingEffect(deltaHP, playerStats.hp, playerStats.maxHp);
            
            playerStats.hp = playerStats.hp + deltaHP <= playerStats.maxHp ?
                playerStats.hp + deltaHP : playerStats.maxHp;
        }

        // check death
        isKilled = false;
        if (playerStats.hp <= 0)
        {
            isKilled = true;
            Die();
        }
            
    }

    // update the max Hp
    public void UpdateMaxHP(int deltaHPMax)
    {
        if (deltaHPMax < 0)
            return;

        playerStats.hp += deltaHPMax;
        playerStats.maxHp += deltaHPMax;

        // show visual
        _playerEffectController.UpdateMaxHPEffect(playerStats.hp, playerStats.maxHp);
    }

    // update hp after receive damage or healing
    public void UpdateExp(int deltaExp)
    {
        if (deltaExp < 0)
            return;

        int totalExp = playerStats.exp + deltaExp;

        // check if level up
        while (totalExp >= playerStats.maxExp)
        {
            totalExp -= playerStats.maxExp;
            LevelUp();
        }

        // add leftover exp
        playerStats.exp = totalExp;

        // show visual
        _playerEffectController.UpdateExpEffect(deltaExp, playerStats.exp, playerStats.maxExp);
    }

    // update the max exp for leveling up
    public void UpdateMaxExp(int deltaExpMax)
    {
        if (deltaExpMax < 0)
            return;

        playerStats.maxExp += deltaExpMax;

        // show visual
        _playerEffectController.UpdateMaxExpEffect(playerStats.exp, playerStats.maxExp);
    }

    // update player's worth exp once level up
    public void UpdateWorthExp(int deltaExpWorth)
    {
        if (deltaExpWorth < 0)
            return;

        playerStats.expWorth += deltaExpWorth;
    }

    // update player gold
    public void UpdateGold(int deltaGold)
    {
        playerStats.gold += deltaGold;

        // show visual
        _playerEffectController.UpdateGoldEffect(deltaGold, playerStats.gold);
    }

    // update player's worth gold once level up
    public void UpdateWorthGold(int deltaGoldWorth)
    {
        if (deltaGoldWorth < 0)
            return;

        playerStats.goldWorth += deltaGoldWorth;
    }

    // update player score
    public void UpdateScore(int deltaScore)
    {
        playerStats.score += deltaScore;
    }

    // return the calculated speed
    public float GetCurrentSpeed()
    {
        return playerStats.baseSpeed * playerStats.speedModifier;
    }

    // level up
    public void LevelUp()
    {
        playerStats.level += 1;

        // show visual
        NetworkCalls.Player_NetWork.LevelUp(_PV, playerStats.level);
    }

    // death coroutine
    public void Die()
    {
        NetworkCalls.Player_NetWork.Die(_PV);
    }

    // respawn
    public void Respawn(InputAction.CallbackContext context)
    {
        if (!playerStats.isDead || !playerStats.isRespawnable)
            return;

        NetworkCalls.Player_NetWork.Respawn(_PV);
    }

    // lock all actions
    public void LockAllActions()
    {
        playerStats.isInvincible = true;
        playerStats.isInventoryLocked = true;
        playerStats.isMovementLocked = true;
        playerStats.isWeaponLocked = true;
        playerStats.isRespawnable = false;
    }
}
