using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AIBuffController : MonoBehaviour
{
    private AIStats _aiStats;
    private AIStatsController _statsController;
    private AIEffectController _effectController;

    private void Awake()
    {
        _statsController = GetComponent<AIStatsController>();
        _aiStats = _statsController.aiStats;
        _effectController = GetComponent<AIEffectController>();
    }

    public void ReceiveDamage(int attackerID, DamageInfo damageInfo, Vector3 attackerPos)
    {
        // check if player is dead
        if (_aiStats.isDead)
        {
            Debug.Log("Player is dead, can't receive damage!");
            return;
        }

        // check if player is invincible
        if (_aiStats.isInvincible)
        {
            Debug.Log("Player is invincible! can't receive damage!");

            // TODO: pop an invincible text by using the effect controller

            return;
        }

        // TODO: delay dmg

        // convert dmg to int
        var dmg = Convert.ToInt32(damageInfo.damageAmount);
        var hpBeforeChange = _aiStats.hp;
        var maxHp = _aiStats.maxHp;

        // check if damage overflow, minus damage amount from hp
        _statsController.UpdateHP(-dmg, out bool isKilled);

        // giving the attacker hit feedback
        var attacker = PhotonView.Find(attackerID).transform;
        var attackerEffectController = attacker.GetComponent<PlayerEffectController>();
        var attackerStatsController = attacker.GetComponent<PlayerStatsController>();
        if (attackerEffectController != null)
        {
            // combo indicator
            attackerEffectController.ComboTextEffect();

            // if this dead, giving the attacker feedback
            if (isKilled)
            {
                attackerEffectController.MultiKillEffect();
                if (attackerStatsController != null)
                {
                    attackerStatsController.UpdateExp(_aiStats.expWorth);
                    attackerStatsController.UpdateGold(_aiStats.goldWorth);

                    // TODO: show gold pop text effect



                }
            }
        }

        // show the visual effect
        _effectController.ReceiveDamageEffect(maxHp, hpBeforeChange, dmg, attackerPos, damageInfo.KnockBackDist);
    }
}
