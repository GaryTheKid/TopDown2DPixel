using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public bool isDead;
    public int maxHp;
    public int hp;
    public float speed;
}

public class PlayerStatsController : MonoBehaviour
{
    public PlayerStats playerStates = new PlayerStats { 
        isDead = false, 
        maxHp = 100, 
        hp = 100, 
        speed = 1f 
    };

    private void Awake()
    {

    }
}
