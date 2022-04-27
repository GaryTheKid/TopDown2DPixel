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
    public bool isWeaponLocked;
    public int maxHp;
    public int hp;
    public float speed;
    public int score;

    public PlayerStats()
    {
        isDead = false;
        isWeaponLocked = false;
        maxHp = 100;
        hp = 100;
        speed = 30f;
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

    // death coroutine
    public void Die()
    {
        NetworkCalls.Character.Die(_PV);
    }

    // respawn
    public void Respawn()
    {
        NetworkCalls.Character.Respawn(_PV);
    }
}
