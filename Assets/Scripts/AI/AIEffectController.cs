using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class AIEffectController : MonoBehaviour
{
    private const float CORPSE_EXPIRE_TIME = 3f;

    [SerializeField] private GameObject _hitBox;
    [SerializeField] private Transform _hpBarParent;
    [SerializeField] private Image _hpBar;
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private AISoundFX _aiSoundFX;
    [SerializeField] private GameObject _popTextTemplate;
    [SerializeField] private MMF_Player mmf_hp;
    [SerializeField] private GameObject _minimapIndicator;
    private Animator _avatarAnimator;
    private GameObject _ring;
    private GameObject _shadow;
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

        // disable ring
        _ring.SetActive(false);

        // disable shadow
        _shadow.SetActive(false);

        // disable hitbox
        _hitBox.SetActive(false);

        // disable minimap indicator
        _minimapIndicator.SetActive(false);

        // set speed 0
        _rb.velocity = Vector2.zero;

        // Play Death Animation
        _avatarAnimator.SetBool("isDead", true);

        // Hp Bar disappear
        foreach (var sprite in _hpBarParent.GetComponentsInChildren<Image>())
        {
            var col = sprite.color;
            col.a = 0f;
            sprite.color = col;
        }

        // set active to false after corpse expires
        CorpseExpire();
    }

    public void CorpseExpire()
    {
        StartCoroutine(Co_DeathDelay());
    }
    IEnumerator Co_DeathDelay()
    {
        yield return new WaitForSeconds(CORPSE_EXPIRE_TIME);
        gameObject.SetActive(false);
    }

    public void RespawnEffect()
    {
        // reset feedback
        mmf_hp.Initialization();

        // set ring
        _ring.SetActive(true);

        // set shadow
        _shadow.SetActive(true);

        // set hitbox
        _hitBox.SetActive(true);

        // disable minimap indicator
        _minimapIndicator.SetActive(true);

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

    public void SetAIEffectFields(GameObject avatar)
    {
        _ring = avatar.transform.Find("Ring").gameObject;
        _shadow = avatar.transform.Find("Shadow").gameObject;
        _avatarAnimator = avatar.GetComponent<Animator>();
        mmf_hp.GetFeedbackOfType<MMF_SpriteRenderer>().BoundSpriteRenderer = avatar.transform.Find("Body").GetComponent<SpriteRenderer>();
        mmf_hp.GetFeedbackOfType<MMF_SquashAndStretch>().SquashAndStretchTarget = avatar.transform.Find("Body");
        mmf_hp.GetFeedbackOfType<MMF_Wiggle>().TargetWiggle = avatar.GetComponent<MMWiggle>();
    }
}
