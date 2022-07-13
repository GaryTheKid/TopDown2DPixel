using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class UI_MultiKillIcon : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private CharacterSoundFX _fx;
    [SerializeField] private MMF_Player _multiKillFeedBacks;
    [SerializeField] private Sprite _multiKill_Icon_1;
    [SerializeField] private Sprite _multiKill_Icon_2;
    [SerializeField] private Sprite _multiKill_Icon_3;

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
                _icon.color = new Color(1f, 1f, 1f, 1f);
                _multiKillFeedBacks.StopAllCoroutines();
                _multiKillFeedBacks.PlayFeedbacks();
                _fx.KillFeedBack();
                break;

            case 2:
                _icon.sprite = _multiKill_Icon_2;
                _icon.color = new Color(1f, 1f, 1f, 1f);
                _multiKillFeedBacks.StopAllCoroutines();
                _multiKillFeedBacks.PlayFeedbacks();
                _fx.KillFeedBack();
                break;

            case 3:
                _icon.sprite = _multiKill_Icon_3;
                _icon.color = new Color(1f, 1f, 1f, 1f);
                _multiKillFeedBacks.StopAllCoroutines();
                _multiKillFeedBacks.PlayFeedbacks();
                _fx.KillFeedBack();
                break;

            default:
                _icon.sprite = _multiKill_Icon_3;
                _icon.color = new Color(1f, 1f, 1f, 1f);
                _multiKillFeedBacks.StopAllCoroutines();
                _multiKillFeedBacks.PlayFeedbacks();
                _fx.KillFeedBack();
                break;
        }
    }
}
