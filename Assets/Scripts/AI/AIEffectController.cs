using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class AIEffectController : MonoBehaviour
{
    [SerializeField] private GameObject _ring;
    [SerializeField] private Transform _hpBarParent;
    [SerializeField] private Image _hpBar;
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private AISoundFX _aiSoundFX;
    [SerializeField] private Animator _avatarAnimator;
    [SerializeField] private GameObject _popTextTemplate;
    [SerializeField] private MMF_Player mmf_hp;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ReceiveDamageEffect(int maxHp, int hpBeforeChange, int dmgAmount, Vector3 attackerPos, float knockBackDist)
    {
        // play sound fx
        _aiSoundFX.BeingDamaged();

        // pop up text
        GameObject popText = Instantiate(_popTextTemplate, _popTextTemplate.transform.position, Quaternion.identity, _popTextTemplate.transform.parent);
        UI_PopText ui_popText = popText.GetComponent<UI_PopText>();
        ui_popText.textAmount = dmgAmount;
        ui_popText.textType = UI_PopText.TextType.Damage;
        popText.SetActive(true);

        // knock back: apply impulse force attacker -> player
        Vector3 myPos = transform.position;
        Vector2 knockBackDir = new Vector2(myPos.x - attackerPos.x, myPos.x - attackerPos.x).normalized * knockBackDist;
        _rb.AddForce(knockBackDir, ForceMode2D.Impulse);

        // feedback effect
        var end = (float)(hpBeforeChange - dmgAmount) / (float)maxHp;
        mmf_hp.GetFeedbackOfType<MMF_ImageFill>().DestinationFill = end;
        mmf_hp.PlayFeedbacks();

        // adjust hp bar
        _hpBar.fillAmount = end;
    }

    public void DeathEffect()
    {
        // reset feedback
        mmf_hp.ResetFeedbacks();

        // set ring
        _ring.SetActive(false);

        // Play Death Animation
        _avatarAnimator.SetBool("isDead", true);

        // Hp Bar disappear
        foreach (var sprite in _hpBarParent.GetComponentsInChildren<Image>())
        {
            var col = sprite.color;
            col.a = 0f;
            sprite.color = col;
        }

        StartCoroutine(Co_DeathDelay());
    }
    IEnumerator Co_DeathDelay()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    public void RespawnEffect()
    {
        // reset feedback
        mmf_hp.Initialization();

        // set ring
        _ring.SetActive(true);

        // Play Death Animation
        _avatarAnimator.SetBool("isDead", false);

        // Hp Bar disappear
        foreach (var sprite in _hpBarParent.GetComponentsInChildren<Image>())
        {
            var col = sprite.color;
            col.a = 1f;
            sprite.color = col;
        }

        // reset body transparency
        var bodyCol = _body.color;
        bodyCol.a = 1f;
        _body.color = bodyCol;

        // adjust hp bar
        _hpBar.fillAmount = 1f;
    }
}
