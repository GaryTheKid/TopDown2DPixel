using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[Serializable]
public class PlayerStats
{
    public bool isDead;
    public bool isInvincible;
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
    }

    private void Update()
    {
        if (!playerStats.isDead)
            return;

        // handle respawn
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
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
    public void Respawn()
    {
        NetworkCalls.Player_NetWork.Respawn(_PV);
    }
}
