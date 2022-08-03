using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class AIStats
{
    public bool isDead;
    public bool isInvincible;
    public bool isMovementLocked;
    public bool isWeaponLocked;
    public int maxHp;
    public int hp;
    public int expWorth;
    public float baseSpeed;
    public float speedModifier;

    public AIStats()
    {
        isDead = false;
        isInvincible = false;
        isMovementLocked = false;
        isWeaponLocked = false;
        maxHp = 50;
        hp = 50;
        expWorth = 10;
        baseSpeed = 27f;
        speedModifier = 1f;
    }
}

public class AIStatsController : MonoBehaviour
{
    public AIStats aiStats = new AIStats();
    public UnityEvent OnDeath;
    public UnityEvent OnRespawn;

    private PhotonView _PV;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
    }

    // return the calculated speed
    public float GetCurrentSpeed()
    {
        return aiStats.baseSpeed * aiStats.speedModifier;
    }

    // restore to max hp
    public void RestoreFullHp()
    {
        aiStats.hp = aiStats.maxHp;
    }

    // update hp after receive damage or healing
    public void UpdateHP(int deltaHP, out bool isKilled)
    {
        // receive dmg
        if (deltaHP < 0)
        {
            Debug.Log("Hp " + deltaHP);
            aiStats.hp = aiStats.hp + deltaHP >= 0 ?
                aiStats.hp + deltaHP : 0;
        }
        // receive healing
        else
        {
            Debug.Log("HP +" + deltaHP);
            aiStats.hp = aiStats.hp + deltaHP <= aiStats.maxHp ?
                aiStats.hp + deltaHP : aiStats.maxHp;
        }

        // check death
        isKilled = false;
        if (aiStats.hp <= 0)
        {
            isKilled = true;
            Die();
        }
    }

    // death coroutine
    public void Die()
    {
        NetworkCalls.AI_NetWork.Die(_PV);
    }

    // respawn
    public void Respawn()
    {
        if (!aiStats.isDead)
            return;

        NetworkCalls.AI_NetWork.Respawn(_PV);
    }
}
