using System.Collections;
using UnityEngine;
using TMPro;

public class UI_PopWindow_ConfirmResolutionOrFrameRateChange : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;

    private SettingPanel settingPanel;
    private int resolutionOption, frameRate;
    private bool isFullScreen;

    public void CachePreviousResolutionAndFrameRate(int prev_resolutionOption, int prev_frameRate, bool prev_isFullScreen, SettingPanel ref_settingPanel)
    {
        resolutionOption = prev_resolutionOption;
        frameRate = prev_frameRate;
        isFullScreen = prev_isFullScreen;
        settingPanel = ref_settingPanel;
    }

    public void StartCountTime()
    {
        StartCoroutine(CountTimer_Co());
    }

    public void ResetChange()
    {
        StartCoroutine(ResetChange_Co());
    }

    IEnumerator CountTimer_Co()
    {
        int time = 10;
        while (time >= 0)
        {
            countText.text = time.ToString();
            yield return new WaitForSecondsRealtime(1f);
            time--;
        }
        ResetChange();
    }

    IEnumerator ResetChange_Co()
    {
        yield return new WaitForEndOfFrame();

        settingPanel.ChangeResolution(resolutionOption, false);
        settingPanel.ChangeFrameRate(frameRate, false);
        settingPanel.ChangeFullScreen(isFullScreen, false);
        settingPanel.isResetting = true;
        settingPanel.UpdateUI_Video();
        settingPanel.isResetting = false;

        gameObject.SetActive(false);
    }
}
