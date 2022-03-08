using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public int hp;
    public float speed;
}

public class PlayerStatsController : MonoBehaviour
{
    public PlayerStats playerStates = new PlayerStats { hp = 100, speed = 1f };

    private void Awake()
    {

    }
}
