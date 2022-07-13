using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerBuffController : MonoBehaviour
{
    [SerializeField] private GameObject _ghostCollider;
    [SerializeField] private GameObject _characterCollider;
    [SerializeField] private GameObject _hitBox;

    private PlayerStats _playerStats;
    private PlayerStatsController _statsController;
    private PlayerEffectController _effectController;
    private PlayerInventoryController _inventoryController;
    private Rigidbody2D _rb;

    private IEnumerator speedBoost_Co;
    private IEnumerator invincible_Co;

    private void Awake()
    {
        _statsController = GetComponent<PlayerStatsController>();
        _playerStats = _statsController.playerStats;
        _effectController = GetComponent<PlayerEffectController>();
        _inventoryController = GetComponent<PlayerInventoryController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ReceiveDamage(int attackerID, DamageInfo damageInfo, Vector3 attackerPos)
    {
        // check if player is dead
        if (_playerStats.isDead)
        {
            Debug.Log("Player is dead, can't receive damage!");
            return;
        }

        // check if player is invincible
        if (_playerStats.isInvincible)
        {
            Debug.Log("Player is invincible! can't receive damage!");

            // TODO: pop an invincible text by using the effect controller

            return;
        }

        // TODO: delay dmg

        // convert dmg to int
        var dmg = Convert.ToInt32(damageInfo.damageAmount);
        var hpBeforeChange = _playerStats.hp;
        var maxHp = _playerStats.maxHp;

        // check if damage overflow, minus damage amount from hp
        _statsController.UpdateHP(-dmg, out bool isKilled);

        // if dead, giving the attacker feedback
        if (isKilled)
        {
            print(attackerID);

            var effectController = PhotonView.Find(attackerID).transform.GetComponent<PlayerEffectController>();
            if (effectController != null)
            {
                effectController.MultiKillEffect();
            }
        }

        // show the visual effect
        _effectController.ReceiveDamageEffect(maxHp, hpBeforeChange, dmg, attackerPos, damageInfo.KnockBackDist);
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
        _statsController.UpdateHP(healingAmount, out bool isKilled);

        // show the visual effect
        _effectController.ReceiveHealingEffect(_playerStats.hp, _playerStats.maxHp);
    }

    public void SpeedBoost(float boostAmount, float effectTime)
    {
        // check if player is dead
        if (_playerStats.isDead)
        {
            Debug.Log("Player is dead, can't receive speed boost!");
            return;
        }

        // check if hp overflow, add healing amount to hp
        if (speedBoost_Co != null)
        {
            StopCoroutine(speedBoost_Co);
        }
        speedBoost_Co = Co_SpeedBoost(boostAmount, effectTime);
        StartCoroutine(speedBoost_Co);

        // show the visual effect
        _effectController.SpeedBoostEffect(effectTime);
    }
    IEnumerator Co_SpeedBoost(float boostAmount, float effectTime)
    {
        _playerStats.baseSpeed += boostAmount;
        yield return new WaitForSecondsRealtime(effectTime);
        _playerStats.baseSpeed -= boostAmount;
        speedBoost_Co = null;
    }

    public void Invincible(float effectTime)
    {
        // check if player is dead
        if (_playerStats.isDead)
        {
            Debug.Log("Player is dead, can't receive invincible buff!");
            return;
        }

        // check if hp overflow, add healing amount to hp
        if (invincible_Co != null)
        {
            StopCoroutine(invincible_Co);
        }
        invincible_Co = Co_Invincible(effectTime);
        StartCoroutine(invincible_Co);

        // TODO: show the visual effect

    }
    IEnumerator Co_Invincible(float effectTime)
    {
        _playerStats.isInvincible = true;
        yield return new WaitForSecondsRealtime(effectTime);
        _playerStats.isInvincible = false;
        invincible_Co = null;
    }

    public void ConsumeHealingPotion(int healingAmount)
    {
        // consume potion effect
        _effectController.ConsumePotionEffect();

        // receive healing
        ReceiveHealing(healingAmount);
    }

    public void ConsumeSpeedBoostPotion(float boostAmount, float effectTime)
    {
        // consume potion effect
        _effectController.ConsumePotionEffect();

        // speedBoost
        SpeedBoost(boostAmount, effectTime);
    }
    
    public void ConsumeInvinciblePotion(float effectTime)
    {
        // consume potion effect
        _effectController.ConsumePotionEffect();

        // be invincible
        Invincible(effectTime);
    }

    public void Ghostify()
    {
        // close inventory
        _inventoryController.CloseUIInventory();

        // switch colliders
        _characterCollider.SetActive(false);
        _hitBox.SetActive(false);
        _ghostCollider.SetActive(true);

        // show the visual effect
        _effectController.DeathEffect();
    }

    public void Solify()
    {
        // switch colliders
        _characterCollider.SetActive(true);
        _hitBox.SetActive(true);
        _ghostCollider.SetActive(false);

        // show the visual effect
        _effectController.RespawnEffect();

        // stay invincible for a while
        Invincible(2f);
    }
}
