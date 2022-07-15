using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class UI_ComboText : MonoBehaviour
{
    private const float COMBO_TEXT_LASTING_TIME = 1.5f;

    [SerializeField] private float _R;
    [SerializeField] private float _G;
    [SerializeField] private float _B;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private MMF_Player _ComboFeedBacks;

    private IEnumerator _co_showComboText;

    private void Awake()
    {
        _R /= 255f;
        _G /= 255f;
        _B /= 255f;
    }

    public void SetComboText(int count)
    {
        switch (count)
        {
            case 0:
                _text.text = "";
                _text.color = new Color(_R, _G, _B, 0f);
                break;

            default:
                _text.text = "Combo x" + count.ToString();
                _ComboFeedBacks.PlayFeedbacks();
                PlayCoroutine();
                break;
        }
    }

    private void PlayCoroutine()
    {
        if (_co_showComboText != null)
        {
            StopCoroutine(_co_showComboText);
            _co_showComboText = null;
        }
        _co_showComboText = Co_ComboText();
        StartCoroutine(_co_showComboText);
    }

    IEnumerator Co_ComboText()
    {
        _text.color = new Color(_R, _G, _B, 1f);
        yield return new WaitForSecondsRealtime(COMBO_TEXT_LASTING_TIME);

        var timer = 0f;
        while (timer < 1f)
        {
            timer += Time.fixedDeltaTime;
            _text.color = new Color(_R, _G, _B, 1f - (timer / 0.5f));
            yield return new WaitForFixedUpdate();
        }
        _text.color = new Color(_R, _G, _B, 0f);
    }
}
