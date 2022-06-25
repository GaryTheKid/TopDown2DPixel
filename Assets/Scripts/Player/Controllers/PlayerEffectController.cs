using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] private Transform _hpBarParent;
    [SerializeField] private Image _hpBar;
    [SerializeField] private GameObject _ring;
    [SerializeField] private GameObject _shadow;
    [SerializeField] private CharacterSoundFX _characterSoundFX;
    [SerializeField] private GameObject _ghostRunFXAnimator;
    [SerializeField] private ScreenFogMask _screenFogFX;
    [SerializeField] private Animator _avatarAnimator;
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

    public void ScreenSmokeOn()
    {
        _screenFogFX.SetFogOn();
    }

    public void ScreenSmokeOff()
    {
        _screenFogFX.SetFogOff();
    }

    public void DeathEffect()
    {
        // visual effect
        _ring.SetActive(false);
        _shadow.SetActive(false);

        // Play Death Animation
        _avatarAnimator.SetBool("isDead", true);

        // Hp Bar disappear
        foreach (var sprite in _hpBarParent.GetComponentsInChildren<Image>())
        {
            var col = sprite.color;
            col.a = 0f;
            sprite.color = col;
        } 
    }

    public void RespawnEffect()
    {
        // visual effect
        _ring.SetActive(true);
        _shadow.SetActive(true);

        // Play Death Animation
        _avatarAnimator.SetBool("isDead", false);

        // Hp Bar disappear
        foreach (var sprite in _hpBarParent.GetComponentsInChildren<Image>())
        {
            var col = sprite.color;
            col.a = 1f;
            sprite.color = col;
        }

        // adjust hp bar
        _hpBar.fillAmount = 1f;
    }
}
