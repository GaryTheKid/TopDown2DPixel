using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class UI_MultiKillIcon : MonoBehaviour
{
    private const float KILLING_ICON_LASTING_TIME = 1f;

    [SerializeField] private Image _icon;
    [SerializeField] private CharacterSoundFX _fx;
    [SerializeField] private MMF_Player _multiKillFeedBacks;
    [SerializeField] private Sprite _multiKill_Icon_1;
    [SerializeField] private Sprite _multiKill_Icon_2;
    [SerializeField] private Sprite _multiKill_Icon_3;

    private IEnumerator _co_KillingSpreeIcon;

    public void SetMultiKillIcon(int multiKillCount)
    {
        switch (multiKillCount)
        {
            case 0:
                _icon.sprite = null;
                _icon.color = new Color(1f, 1f, 1f, 0f);
                break;

            case 1:
                _icon.sprite = _multiKill_Icon_1;
                _multiKillFeedBacks.PlayFeedbacks();
                PlayCoroutine();
                _fx.KillFeedBack();
                break;

            case 2:
                _icon.sprite = _multiKill_Icon_2;
                _multiKillFeedBacks.PlayFeedbacks();
                PlayCoroutine();
                _fx.KillFeedBack();
                break;

            case 3:
                _icon.sprite = _multiKill_Icon_3;
                _multiKillFeedBacks.PlayFeedbacks();
                PlayCoroutine();
                _fx.KillFeedBack();
                break;

            default:
                _icon.sprite = _multiKill_Icon_3;
                _multiKillFeedBacks.PlayFeedbacks();
                PlayCoroutine();
                _fx.KillFeedBack();
                break;
        }
    }

    private void PlayCoroutine()
    {
        if (_co_KillingSpreeIcon != null)
        {
            StopCoroutine(_co_KillingSpreeIcon);
            _co_KillingSpreeIcon = null;
        }
        _co_KillingSpreeIcon = Co_KillingSpreeIcon();
        StartCoroutine(_co_KillingSpreeIcon);
    }

    IEnumerator Co_KillingSpreeIcon()
    {
        _icon.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSecondsRealtime(KILLING_ICON_LASTING_TIME);

        var timer = 0f;
        while (timer < 1f)
        {
            timer += Time.fixedDeltaTime;
            _icon.color = new Color(1f, 1f, 1f, 1f - (timer / 0.5f));
            yield return new WaitForFixedUpdate();
        }
        _icon.color = new Color(1f, 1f, 1f, 0f);
    }
}
