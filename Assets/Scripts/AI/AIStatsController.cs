using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStats
{
    public bool isDead;
    public bool isInvincible;
    public bool isMovementLocked;
    public bool isWeaponLocked;
    public int maxHp;
    public int hp;
    public float baseSpeed;
    public float speedModifier;

    public AIStats()
    {
        isDead = false;
        isInvincible = false;
        isMovementLocked = false;
        isWeaponLocked = false;
        maxHp = 100;
        hp = 100;
        baseSpeed = 0.1f;
        speedModifier = 1f;
    }
}

public class AIStatsController : MonoBehaviour
{
    public AIStats aiStats = new AIStats();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // return the calculated speed
    public float GetCurrentSpeed()
    {
        return aiStats.baseSpeed * aiStats.speedModifier;
    }
}
