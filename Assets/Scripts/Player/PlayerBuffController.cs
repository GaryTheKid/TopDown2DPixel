using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBuffController : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerEffectController effectController;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStatsController>().playerStates;
        effectController = GetComponent<PlayerEffectController>();
    }

    public void ReceiveHealing(int healingAmount)
    {
        Debug.Log("HP +" + healingAmount);
        playerStats.hp += healingAmount;
        effectController.ReceiveHealingEffect();
    }
}
