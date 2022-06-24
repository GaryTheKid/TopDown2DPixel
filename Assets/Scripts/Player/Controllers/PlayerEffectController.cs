using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private CharacterSoundFX _characterSoundFX;
    [SerializeField] private GameObject _ghostRunFXAnimator;
    private Rigidbody2D _rb;

    private IEnumerator speedBoost_Co;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ConsumePotionEffect()
    {
        // play sound fx
        _characterSoundFX.ConsumePotion();
    }

    public void ReceiveDamageEffect(int currHP, int maxHP, Vector3 attackerPos, float knockBackDist)
    {
        // play sound fx
        _characterSoundFX.BeingDamaged();

        // TODO: blink red

        // TODO: pop up text

        // adjust hp bar
        _hpBar.fillAmount = (float)currHP / (float)maxHP;

        // knock back: apply impulse force attacker -> player
        Vector3 myPos = transform.position;
        Vector2 knockBackDir = new Vector2(myPos.x - attackerPos.x, myPos.x - attackerPos.x).normalized * knockBackDist;
        _rb.AddForce(knockBackDir, ForceMode2D.Impulse);
    }

    public void ReceiveHealingEffect(int currHP, int maxHP)
    {
        // TODO: blink green

        // TODO: pop up text

        // adjust hp bar
        _hpBar.fillAmount = (float)currHP / (float)maxHP;
    }

    public void SpeedBoostEffect(float effectTime)
    {
        // play sound fx
        _characterSoundFX.SpeedBoost();

        if (speedBoost_Co == null)
        {
            speedBoost_Co = Co_SpeedBoost(effectTime);
            StartCoroutine(speedBoost_Co);
        }
    }
    IEnumerator Co_SpeedBoost(float effectTime)
    {
        // show ghost run trail fx
        _ghostRunFXAnimator.SetActive(true);
        yield return new WaitForSecondsRealtime(effectTime);
        _ghostRunFXAnimator.SetActive(false);
        speedBoost_Co = null;
    }

    public void DeathEffect()
    {
        // TODO: death visual effect
        // Play Death Animation

        // Hp Bar disappear
    }

    public void RespawnEffect()
    {
        // TODO: visual effect


        // adjust hp bar
        _hpBar.fillAmount = 1f;
    }
}
