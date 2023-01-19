using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cinemachine;
using MoreMountains.Feedbacks;
using Photon.Pun;
using TMPro;

public class PlayerEffectController : MonoBehaviour
{
    private const float KILLING_SPREE_LAST_TIME = 2f;
    private const float COMBO_TEXT_LAST_TIME = 1.5f;
    
    [SerializeField] private Transform _WorldCanvasParent;
    [SerializeField] private TextMeshPro _nameTag;
    [SerializeField] private Image _bar;
    [SerializeField] private Image _barGrid;
    [SerializeField] private Image _hp;
    [SerializeField] private SpriteRenderer _bodySprite;
    [SerializeField] private UI_MultiKillIcon _ui_killIcon;
    [SerializeField] private UI_ComboText _ui_ComboText;
    [SerializeField] private Image _ui_hpBar;
    [SerializeField] private TextMeshProUGUI _ui_hpText;
    [SerializeField] private TextMeshProUGUI _ui_hpMaxText;
    [SerializeField] private TextMeshProUGUI _ui_expMaxText;
    [SerializeField] private Image _ui_expBar;
    [SerializeField] private Text _goldText;
    [SerializeField] private GameObject _ring;
    [SerializeField] private GameObject _shadow;
    [SerializeField] private CharacterSoundFX _characterSoundFX;
    [SerializeField] private GameObject _ghostRunFXAnimator;
    [SerializeField] private GameObject _BlinkFX;
    [SerializeField] private ScreenFogMask _screenFogFX;
    [SerializeField] private RainningFX _rainningFX;
    [SerializeField] private Animator _avatarAnimator;
    [SerializeField] private CinemachineVirtualCamera _vCam;
    [SerializeField] private GameObject _popTextTemplate;
    [SerializeField] private MMF_Player mmf_receiveDamage;
    [SerializeField] private MMF_Player mmf_receiveHealing;
    [SerializeField] private MMF_Player mmf_receiveGold;
    [SerializeField] private MMF_Player mmf_loseGold;
    [SerializeField] private MMF_Player mmf_regeneration;
    [SerializeField] private MMF_Player mmf_updateExp;
    [SerializeField] private MMF_Player mmf_levelUp;

    private PhotonView _PV;
    private Rigidbody2D _rb;
    private int _comboCount;
    private int _multiKillCount;

    private IEnumerator speedBoost_Co;
    private IEnumerator camShake_Co;
    private IEnumerator combo_Co;
    private IEnumerator multiKill_Co;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(Co_initHpBar());
    }
    IEnumerator Co_initHpBar()
    {
        _bar.gameObject.SetActive(false);
        _barGrid.gameObject.SetActive(false);
        _hp.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(0.1f);

        _bar.gameObject.SetActive(true);
        _barGrid.gameObject.SetActive(true);
        _hp.gameObject.SetActive(true);
    }

    public void CameraShake(float intensity, float time)
    {
        if (time <= 0 || intensity <= 0)
            return;

        if (camShake_Co != null)
        {
            StopCoroutine(camShake_Co);
        }

        camShake_Co = Co_camShake(intensity, time);
        StartCoroutine(camShake_Co);
    }
    IEnumerator Co_camShake(float intensity, float time)
    {
        var timer = time;
        var perlin = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        while (timer >= 0f)
        {
            timer -= Time.deltaTime;
            perlin.m_AmplitudeGain = intensity * (timer / time);
            yield return new WaitForEndOfFrame();
        }
        perlin.m_AmplitudeGain = 0f;
    }

    public void ConsumePotionEffect()
    {
        // play sound fx
        _characterSoundFX.ConsumePotion();
    }

    public void KnockbackEffect(Vector3 attackerPos, float knockBackDist)
    {
        // knock back: apply impulse force attacker -> player
        Vector3 myPos = transform.position;
        Vector2 knockBackDir = new Vector2(myPos.x - attackerPos.x, myPos.x - attackerPos.x).normalized * knockBackDist;
        _rb.AddForce(knockBackDir, ForceMode2D.Impulse);
    }

    public void ReceiveDamageEffect(int dmgAmount, int currHp, int maxHp)
    {
        // play sound fx
        _characterSoundFX.BeingDamaged();

        // camera shake
        CameraShake(dmgAmount / 20f, 0.15f);

        // pop up text
        GameObject popText = Instantiate(_popTextTemplate, _popTextTemplate.transform.position, Quaternion.identity, _popTextTemplate.transform.parent);
        UI_PopText ui_popText = popText.GetComponent<UI_PopText>();
        ui_popText.textAmount = dmgAmount;
        ui_popText.textType = UI_PopText.TextType.Damage;
        popText.SetActive(true);

        // feedback effect
        var updatedHp = currHp + dmgAmount;
        if (updatedHp < 0)
            updatedHp = 0;
        mmf_receiveDamage.GetFeedbackOfType<MMF_TMPText>().NewText = updatedHp.ToString();
        var end = (float)updatedHp / (float)maxHp;
        foreach (var feedBack in mmf_receiveDamage.GetFeedbacksOfType<MMF_ImageFill>())
        {
            feedBack.DestinationFill = end;
        }
        mmf_receiveDamage.PlayFeedbacks();

        // adjust hp bar
        _hp.fillAmount = end;
        _ui_hpBar.fillAmount = end;
    }

    public void ReceiveHealingEffect(int healingAmount, int currHP, int maxHp)
    {
        // play sound fx
        _characterSoundFX.ReceiveHealing();

        // pop up text
        GameObject popText = Instantiate(_popTextTemplate, _popTextTemplate.transform.position, Quaternion.identity, _popTextTemplate.transform.parent);
        UI_PopText ui_popText = popText.GetComponent<UI_PopText>();
        ui_popText.textAmount = healingAmount;
        ui_popText.textType = UI_PopText.TextType.Heal;
        popText.SetActive(true);

        // feedback effect
        var updatedHp = currHP + healingAmount;
        if (updatedHp > maxHp)
            updatedHp = maxHp;
        mmf_receiveHealing.GetFeedbackOfType<MMF_TMPText>().NewText = updatedHp.ToString();
        var end = (float)updatedHp / (float)maxHp;
        foreach (var feedBack in mmf_receiveHealing.GetFeedbacksOfType<MMF_ImageFill>())
        {
            feedBack.DestinationFill = end;
        }
        mmf_receiveHealing.PlayFeedbacks();

        // adjust hp bar
        _hp.fillAmount = end;
        _ui_hpBar.fillAmount = end;
    }

    public void RegenerationEffect(int healingAmount, int currHP, int maxHp)
    {
        // pop up text
        GameObject popText = Instantiate(_popTextTemplate, _popTextTemplate.transform.position, Quaternion.identity, _popTextTemplate.transform.parent);
        UI_PopText ui_popText = popText.GetComponent<UI_PopText>();
        ui_popText.textAmount = healingAmount;
        ui_popText.textType = UI_PopText.TextType.Heal;
        popText.SetActive(true);

        // feedback effect
        var updatedHp = currHP + healingAmount;
        if (updatedHp > maxHp)
            updatedHp = maxHp;
        mmf_regeneration.GetFeedbackOfType<MMF_TMPText>().NewText = updatedHp.ToString();
        var end = (float)updatedHp / (float)maxHp;
        foreach (var feedBack in mmf_regeneration.GetFeedbacksOfType<MMF_ImageFill>())
        {
            feedBack.DestinationFill = end;
        }
        mmf_regeneration.PlayFeedbacks();

        // adjust hp bar
        _hp.fillAmount = end;
        _ui_hpBar.fillAmount = end;
    }

    public void UpdateMaxHPEffect(int currentHp, int newMaxHP)
    {
        _ui_hpText.text = currentHp.ToString();
        _ui_hpMaxText.text = newMaxHP.ToString();
        var newFillAmount = (float)currentHp / (float)newMaxHP;
        _ui_hpBar.fillAmount = newFillAmount;
        _hp.fillAmount = newFillAmount;
    }

    public void UpdateExpEffect(int expDelta, int currExp, int maxExp)
    {
        /*// pop up text
        GameObject popText = Instantiate(_popTextTemplate, _popTextTemplate.transform.position, Quaternion.identity, _popTextTemplate.transform.parent);
        UI_PopText ui_popText = popText.GetComponent<UI_PopText>();
        ui_popText.textAmount = expDelta;
        ui_popText.textType = UI_PopText.TextType.Heal;
        popText.SetActive(true);*/

        // feedback effect
        mmf_updateExp.GetFeedbackOfType<MMF_TMPText>().NewText = currExp.ToString();
        var end = (float)currExp / (float)maxExp;
        foreach (var feedBack in mmf_updateExp.GetFeedbacksOfType<MMF_ImageFill>())
        {
            feedBack.DestinationFill = end;
        }
        mmf_updateExp.PlayFeedbacks();

        // adjust exp bar
        _ui_expBar.fillAmount = end;
    }

    public void UpdateMaxExpEffect(int currentHp, int newMaxExp)
    {
        _ui_expMaxText.text = newMaxExp.ToString();
        _ui_expBar.fillAmount = (float)currentHp / (float)newMaxExp;
    }

    public void UpdateGoldEffect(int deltaGold, int newGold)
    {
        if (deltaGold > 0)
        {
            mmf_receiveGold.PlayFeedbacks();
        }
        else if (deltaGold < 0)
        {
            mmf_loseGold.PlayFeedbacks();
        }

        _goldText.text = newGold.ToString();
    }

    public void LevelUpEffect(short newLevel)
    {
        mmf_levelUp.GetFeedbackOfType<MMF_TMPText>().NewText = "lv. " + newLevel.ToString();
        mmf_levelUp.PlayFeedbacks();
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

    public void StartRainningEffect()
    {
        _rainningFX.StartRainning();
    }

    public void StopRainningEffect()
    {
        _rainningFX.StopRainning();
    }

    public void BlinkEffect()
    {
        _BlinkFX.SetActive(true);
        _characterSoundFX.Blink();
    }

    public void InvincibleEffect()
    {

    }

    public void StealthEffect_HalfTransparent()
    {
        _bar.color = new Color(1f, 1f, 1f, 0.5f);
        _barGrid.color = new Color(1f, 1f, 1f, 0.5f);
        _hp.color = new Color(1f, 1f, 1f, 0.5f);
        _nameTag.alpha = 0.5f;
        _ring.SetActive(false);
        _shadow.SetActive(false);
    }

    public void StealthEffect_FullyTransparent()
    {
        _bar.color = new Color(1f, 1f, 1f, 0f);
        _barGrid.color = new Color(1f, 1f, 1f, 0f);
        _hp.color = new Color(1f, 1f, 1f, 0f);
        _nameTag.alpha = 0f;
        _ring.SetActive(false);
        _shadow.SetActive(false);
    }

    public void RevealStealthEffect()
    {
        _bar.color = new Color(1f, 1f, 1f, 1f);
        _barGrid.color = new Color(1f, 1f, 1f, 1f);
        _hp.color = new Color(1f, 1f, 1f, 1f);
        _nameTag.alpha = 1f;
        _ring.SetActive(true);
        _shadow.SetActive(true);
    }

    public void DeathEffect()
    {
        // visual effect
        _ring.SetActive(false);
        _shadow.SetActive(false);

        // Play Death Animation
        _avatarAnimator.SetBool("isDead", true);

        // Hp Bar disappear
        foreach (var sprite in _WorldCanvasParent.GetComponentsInChildren<Image>())
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
        foreach (var sprite in _WorldCanvasParent.GetComponentsInChildren<Image>())
        {
            var col = sprite.color;
            col.a = 1f;
            sprite.color = col;
        }

        // adjust hp bar
        _hp.fillAmount = 1f;
        _ui_hpBar.fillAmount = 1f;
    }

    public void MultiKillEffect()
    {
        if (!_PV.IsMine)
            return;

        if (multiKill_Co != null)
        {
            StopCoroutine(multiKill_Co);
            multiKill_Co = null;
        }
        multiKill_Co = Co_KillingSpree();
        StartCoroutine(multiKill_Co);
    }
    IEnumerator Co_KillingSpree()
    {
        _multiKillCount++;
        _ui_killIcon.SetMultiKillIcon(_multiKillCount);
        yield return new WaitForSecondsRealtime(KILLING_SPREE_LAST_TIME);
        _multiKillCount = 0;
    }

    public void ComboTextEffect()
    {
        if (!_PV.IsMine)
            return;

        if (combo_Co != null)
        {
            StopCoroutine(combo_Co);
            combo_Co = null;
        }
        combo_Co = Co_Combo();
        StartCoroutine(combo_Co);
    }
    IEnumerator Co_Combo()
    {
        _comboCount++;
        _ui_ComboText.SetComboText(_comboCount);
        yield return new WaitForSecondsRealtime(COMBO_TEXT_LAST_TIME);
        _comboCount = 0;
    }
}
