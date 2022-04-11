using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    public bool isDead;
    public bool isWeaponLocked;
    public int maxHp;
    public int hp;
    public float speed;
}

public class PlayerStatsController : MonoBehaviour
{
    public PlayerStats playerStates = new PlayerStats {
        isDead = false,
        isWeaponLocked = false,
        maxHp = 100, 
        hp = 100, 
        speed = 1f 
    };

    private void Awake()
    {

    }
}
