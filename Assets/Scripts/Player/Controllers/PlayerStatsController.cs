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
    public PlayerStats playerStats = new PlayerStats {
        isDead = false,
        isWeaponLocked = false,
        maxHp = 100, 
        hp = 100, 
        speed = 1f 
    };

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
            playerStats.isDead = true;
    }
}
