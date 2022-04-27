using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ReceiveMeleeWeaponDamageEffect()
    {
        
    }

    public void ReceiveDamageEffect(int currHP, int maxHP, Vector3 attackerPos, float knockBackDist)
    {
        // TODO: blink red

        // TODO: pop up text

        // adjust hp bar
        hpBar.fillAmount = (float)currHP / (float)maxHP;

        // knock back: apply impulse force attacker -> player
        Vector3 myPos = transform.position;
        Vector2 knockBackDir = new Vector2(myPos.x - attackerPos.x, myPos.x - attackerPos.x).normalized * knockBackDist;
        rb.AddForce(knockBackDir, ForceMode2D.Impulse);
    }

    public void ReceiveHealingEffect(int currHP, int maxHP)
    {
        // TODO: blink green

        // TODO: pop up text

        // adjust hp bar
        hpBar.fillAmount = (float)currHP / (float)maxHP;
    }

    public void DeathEffect()
    {
        // TODO: death visual effect


    }

    public void RespawnEffect()
    {
        // TODO: visual effect


        // adjust hp bar
        hpBar.fillAmount = 1f;
    }
}
