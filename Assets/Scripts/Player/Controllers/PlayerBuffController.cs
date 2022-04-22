using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerBuffController : MonoBehaviour
{
    private PlayerStats _playerStats;
    private PlayerStatsController _statsController;
    private PlayerEffectController _effectController;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _statsController = GetComponent<PlayerStatsController>();
        _playerStats = _statsController.playerStats;
        _effectController = GetComponent<PlayerEffectController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ReceiveDamage(DamageInfo damageInfo, Vector3 attackerPos)
    {
        // check if player is dead
        if (_playerStats.isDead)
        {
            Debug.Log("Player is dead, can't receive damage!");
            return;
        }

        // TODO: delay dmg


        // check if damage overflow, minus damage amount from hp
        _statsController.UpdateHP(-Convert.ToInt32(damageInfo.damageAmount));

        // TODO: check if dead


        // TODO: dmg duration


        // show the visual effect
        _effectController.ReceiveDamageEffect(_playerStats.hp, _playerStats.maxHp, attackerPos, damageInfo.KnockBackDist);
    }

    public void ReceiveHealing(int healingAmount)
    {
        // check if player is dead
        if (_playerStats.isDead)
        {
            Debug.Log("Player is dead, can't receive healing!");
            return;
        }

        // check if hp overflow, add healing amount to hp
        _statsController.UpdateHP(healingAmount);

        // show the visual effect
        _effectController.ReceiveHealingEffect(_playerStats.hp, _playerStats.maxHp);
    }
}
