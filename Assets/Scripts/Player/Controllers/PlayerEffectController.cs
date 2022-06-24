using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private CharacterSoundFX _characterSoundFX;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ReceiveDamageEffect(int currHP, int maxHP, Vector3 attackerPos, float knockBackDist)
    {
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

        // play sound fx
        _characterSoundFX.ConsumePotion();
    }

    public void SpeedBoostEffect()
    {
        // TODO: add ghost trail (shader)


    }

    public void DeathEffect()
    {
        // TODO: death visual effect


    }

    public void RespawnEffect()
    {
        // TODO: visual effect


        // adjust hp bar
        _hpBar.fillAmount = 1f;
    }
}
