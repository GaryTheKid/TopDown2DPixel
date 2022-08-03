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
    public int maxHp;
    public int hp;
    public int maxExp;
    public int exp;
    public short level;
    public int expWorth;
    public float baseSpeed;
    public float speedModifier;
    public int score;

    public PlayerStats()
    {
        isDead = false;
        isInvincible = false;
        isInventoryLocked = false;
        isMovementLocked = false;
        isWeaponLocked = false;
        maxHp = 100;
        hp = 100;
        maxExp = 100;
        exp = 0;
        level = 1;
        expWorth = 50;
        baseSpeed = 30f;
        speedModifier = 1f;
        score = 0;
    }
}

public class PlayerStatsController : MonoBehaviour
{
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
    }

    // update hp after receive damage or healing
    public void UpdateHP(int deltaHP, out bool isKilled)
    {
        // receive dmg
        if (deltaHP < 0)
        {
            Debug.Log("Hp " + deltaHP);
            playerStats.hp = playerStats.hp + deltaHP >= 0 ?
                playerStats.hp + deltaHP : 0;
        }
        // receive healing
        else
        {
            Debug.Log("HP +" + deltaHP);
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

    // update hp after receive damage or healing
    public void UpdateExp(int deltaExp)
    {
        // receive dmg
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
        playerStats.level++;

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
        if (!playerStats.isDead)
            return;

        NetworkCalls.Player_NetWork.Respawn(_PV);
    }
}
