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

    public void ReceiveDamage(int damageAmount)
    {
        // check if player is dead
        if (playerStats.isDead)
        {
            Debug.Log("Player is dead, can't receive damage!");
            return;
        }

        // check if damage overflow, minus damage amount from hp
        Debug.Log("Hp - " + damageAmount);
        playerStats.hp = playerStats.hp - damageAmount >= 0 ?
            playerStats.hp - damageAmount : 0;

        // show the visual effect
        effectController.ReceiveDamageEffect();
    }

    public void ReceiveHealing(int healingAmount)
    {
        // check if player is dead
        if (playerStats.isDead)
        {
            Debug.Log("Player is dead, can't receive healing!");
            return;
        }

        // check if hp overflow, add healing amount to hp
        Debug.Log("HP + " + healingAmount);
        playerStats.hp = playerStats.hp + healingAmount <= playerStats.maxHp ? 
            playerStats.hp + healingAmount : playerStats.maxHp;

        // show the visual effect
        effectController.ReceiveHealingEffect();
    }
}
