using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerBuffController : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerEffectController effectController;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStatsController>().playerStates;
        effectController = GetComponent<PlayerEffectController>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void ReceiveDamage(DamageInfo damageInfo, Vector3 attackerPos)
    {
        // check if player is dead
        if (playerStats.isDead)
        {
            Debug.Log("Player is dead, can't receive damage!");
            return;
        }

        // TODO: delay dmg


        // check if damage overflow, minus damage amount from hp
        int dmg = Convert.ToInt32(damageInfo.damageAmount);
        Debug.Log("Hp - " + damageInfo.damageAmount);
        playerStats.hp = playerStats.hp - dmg >= 0 ?
            playerStats.hp - dmg : 0;

        // knock back: apply force attacker -> player
        Vector3 myPos = transform.position;
        Vector2 knockBackDir = new Vector2(myPos.x - attackerPos.x, myPos.x - attackerPos.x).normalized * damageInfo.KnockBackDist;
        rb.AddForce(knockBackDir, ForceMode2D.Impulse);

        // TODO: dmg duration


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
