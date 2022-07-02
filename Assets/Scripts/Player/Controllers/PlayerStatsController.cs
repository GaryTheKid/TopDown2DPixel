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

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();

        GetComponent<PlayerInputActions>().inputActions.Player.Respawn.performed += Respawn;
    }

    // restore to max hp
    public void RestoreFullHp()
    {
        playerStats.hp = playerStats.maxHp;
    }

    // update hp after receive damage or healing
    public void UpdateHP(int deltaHP)
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
        if (playerStats.hp <= 0)
            Die();
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
