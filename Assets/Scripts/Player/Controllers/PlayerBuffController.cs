using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Photon.Pun;
using NetworkCalls;

public class PlayerBuffController : MonoBehaviour
{
    [SerializeField] private GameObject _ghostCollider;
    [SerializeField] private GameObject _characterCollider;
    [SerializeField] private GameObject _hitBox;

    private PlayerStats _playerStats;
    private PlayerStatsController _statsController;
    private PlayerEffectController _effectController;
    private PlayerInventoryController _inventoryController;
    private PCInputActions _inputActions;
    private Rigidbody2D _rb;
    private PhotonView _PV;

    private IEnumerator invincible_Co;
    private IEnumerator regeneration_Co;
    private IEnumerator RespawnCountDown_Co;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _statsController = GetComponent<PlayerStatsController>();
        _playerStats = _statsController.playerStats;
        _effectController = GetComponent<PlayerEffectController>();
        _inventoryController = GetComponent<PlayerInventoryController>();
        _inputActions = GetComponent<PlayerInputActions>().inputActions;
        _rb = GetComponent<Rigidbody2D>();
    }

    public void RespawnCountDown()
    {
        // check if player is dead
        if (_playerStats.isDead)
        {
            Debug.Log("Player is dead, can't receive speed boost!");
            return;
        }

        if (RespawnCountDown_Co != null)
        {
            StopCoroutine(RespawnCountDown_Co);
            RespawnCountDown_Co = null;
        }
        RespawnCountDown_Co = Co_CountDown();
        StartCoroutine(RespawnCountDown_Co);
    }
    IEnumerator Co_CountDown()
    {
        _playerStats.isRespawnable = false;
        _effectController.TurnOnOffRespawnCDBar(true);
        _effectController.WaitForRespawnCDEffect(_playerStats.respawnCD);
        yield return new WaitForSecondsRealtime(_playerStats.respawnCD);
        _effectController.TurnOnOffRespawnCDBar(false);
        _playerStats.isRespawnable = true;

        RespawnCountDown_Co = null;
    }

    public void ReceiveDamage(int attackerID, DamageInfo damageInfo, Vector3 attackerPos)
    {
        // check if player is dead
        if (_playerStats.isDead || _playerStats.hp <= 0)
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

        // check if damage overflow, minus damage amount from hp
        _statsController.UpdateHP(-dmg, out bool isKilled);

        // giving the attacker hit feedback
        var attackerPV = PhotonView.Find(attackerID);
        var attacker = attackerPV.transform;
        var attackerEffectController = attacker.GetComponent<PlayerEffectController>();
        var attackerStatsController = attacker.GetComponent<PlayerStatsController>();
        if (attackerEffectController != null)
        {
            // combo indicator
            attackerEffectController.ComboTextEffect();

            // if this dead, giving the attacker feedback
            if (isKilled && _PV.ViewID != attackerID)
            {
                attackerEffectController.MultiKillEffect();
                if (attackerStatsController != null)
                {
                    attackerStatsController.UpdateExp(_playerStats.expWorth);
                    Player_NetWork.GainGold(attackerPV, (short)_playerStats.goldWorth);

                    // TODO: show gold pop text effect



                }
            }
        }

        // show knock back effect
        _effectController.KnockbackEffect(attackerPos, damageInfo.KnockBackDist);
    }

    public void ReceiveHealing(int healingAmount)
    {
        // check if player is dead
        if (_playerStats.isDead)
        {
            Debug.Log("Player is dead, can't receive healing!");
            return;
        }

        var hpBeforeChange = _playerStats.hp;

        // check if hp overflow, add healing amount to hp
        _statsController.UpdateHP(healingAmount, out bool isKilled);
    }

    public void SpeedBoost(float boostAmount, float effectTime)
    {
        // check if player is dead
        if (_playerStats.isDead)
        {
            Debug.Log("Player is dead, can't receive speed boost!");
            return;
        }

        // speedBoost
        StartCoroutine(Co_SpeedBoost(boostAmount, effectTime));

        // show the visual effect
        _effectController.SpeedBoostEffect(effectTime);
    }
    IEnumerator Co_SpeedBoost(float boostAmount, float effectTime)
    {
        _playerStats.baseSpeed += boostAmount;
        yield return new WaitForSecondsRealtime(effectTime);
        _playerStats.baseSpeed -= boostAmount;
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

    public void Regeneration(int healingAmount)
    {
        if (regeneration_Co != null)
        {
            StopCoroutine(regeneration_Co);
        }
        regeneration_Co = Co_Regeneration(healingAmount);
        StartCoroutine(regeneration_Co);
    }
    IEnumerator Co_Regeneration(int healingAmount)
    {
        while (!_playerStats.isDead)
        {
            yield return new WaitForSecondsRealtime(5f);
            _statsController.Regeneration(healingAmount);
        }

        regeneration_Co = null;
    }

    public void Stealth_HalfTransparent()
    {
        // show Stealth Effect (half transparent)
        _effectController.StealthEffect_HalfTransparent();
    }

    public void Stealth_FullyTransparent()
    {
        // show Stealth Effect (fully transparent)
        _effectController.StealthEffect_FullyTransparent();
    }

    public void RevealFromStealth()
    {
        // reveal from the stealth effect
        _effectController.RevealStealthEffect();
    }

    public void WeatherBuff(byte prevWeatherCode, byte newWeatherCode)
    {
        // stop previous weather
        switch ((GameManager.Weather)prevWeatherCode)
        {
            case GameManager.Weather.Sunny:
                break;
            case GameManager.Weather.Rainning:

                // TODO: add weather buff

                _effectController.StopRainningEffect();
                break;
        }

        // start new weather
        switch ((GameManager.Weather)newWeatherCode)
        {
            case GameManager.Weather.Sunny:
                break;
            case GameManager.Weather.Rainning:

                // TODO: add weather buff

                _effectController.StartRainningEffect();
                break;
        }
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
