using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopWindowManager : MonoBehaviour
{
    public static PopWindowManager singleton;

    [SerializeField] private UI_PopWindow_ConfirmResolutionOrFrameRateChange confirmResolutionOrFrameRateChange;

    private void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(singleton.gameObject);
        }
        singleton = this;
    }

    public void Pop_ConfirmResolutionOrFrameRateChange(int prev_resolutionOption, int prev_frameRate, bool prev_isFullScreen, SettingPanel settingPanel)
    {
        confirmResolutionOrFrameRateChange.CachePreviousResolutionAndFrameRate(prev_resolutionOption, prev_frameRate, prev_isFullScreen, settingPanel);
        confirmResolutionOrFrameRateChange.gameObject.SetActive(true);
        confirmResolutionOrFrameRateChange.StartCountTime();
    }

    public void Close_ConfirmResolutionOrFrameRateChange()
    {
        confirmResolutionOrFrameRateChange.StopAllCoroutines();
        confirmResolutionOrFrameRateChange.gameObject.SetActive(false);
    }
}
