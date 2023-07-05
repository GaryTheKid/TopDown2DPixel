using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingPanel : MonoBehaviour
{
    public bool hasInitialized;
    public bool isResetting;

    [Header("Audio")]
    public Slider BGMVolumeSlider;
    public Slider FXVolumeSlider;
    public TextMeshProUGUI BGMVolumeText;
    public TextMeshProUGUI FXVolumeText;

    [Header("Video")]
    [SerializeField] private TMP_Dropdown resolutionOptions;
    public List<(int, int)> resolutionOptionList;
    public Button FrameRateButton_30FPS;
    public Button FrameRateButton_55FPS;
    public Button FrameRateButton_60FPS;
    public Button FrameRateButton_144FPS;
    public Toggle FullScreenToggle;

    private void Awake()
    {
        BGMVolumeSlider.onValueChanged.AddListener(UpdateBGMVolumeText);
        FXVolumeSlider.onValueChanged.AddListener(UpdateFXVolumeText);

        resolutionOptionList = new List<(int, int)>();
        foreach (var option in resolutionOptions.options)
        {
            string optionText = option.text;
            string[] dimensions = optionText.Split(' ');
            string[] resolutionValues = dimensions[0].Split('*');

            int width = int.Parse(resolutionValues[0]);
            int height = int.Parse(resolutionValues[1]);

            resolutionOptionList.Add((width, height));
        }
    }

    private void OnDisable()
    {
        PlayerSettings.singleton.SyncPlayerCustomProperty_All();
    }

    private void Start()
    {
        StartCoroutine(GetCloud_PlayerCustomSettings());
    }

    private IEnumerator GetCloud_PlayerCustomSettings()
    {
        yield return new WaitUntil(() =>
        {
            return CloudCommunicator.singleton.hasDataSynced;
        });

        var BGMVolume = CloudCommunicator.singleton.BGMVolume;
        var FXVolume = CloudCommunicator.singleton.FXVolume;
        var fullScreen = CloudCommunicator.singleton.isFullScreen;
        var resolutionOption = CloudCommunicator.singleton.resolutionOption;
        var frameRate = CloudCommunicator.singleton.frameRate;

        PlayerSettings.singleton.BGMVolume = BGMVolume;
        PlayerSettings.singleton.FXVolume = FXVolume;
        PlayerSettings.singleton.FullScreen = fullScreen;
        PlayerSettings.singleton.ResolutionOption = resolutionOption;
        PlayerSettings.singleton.FrameRate = frameRate;

        (int, int) resolution = resolutionOptionList[resolutionOption];
        PlayerSettings.singleton.Resolution_X = resolution.Item1;
        PlayerSettings.singleton.Resolution_Y = resolution.Item2;

        yield return new WaitForEndOfFrame();

        GetCloudSettingsToLocal();

        hasInitialized = true;
    }

    private void GetCloudSettingsToLocal()
    {
        UpdateUI_Audio();
        UpdateUI_Video();
        PlayerSettings.singleton.UpdateResolution();
        PlayerSettings.singleton.UpdateFrameRate();
    }

    public void UpdateUI_Audio()
    {
        BGMVolumeSlider.value = PlayerSettings.singleton.BGMVolume;
        FXVolumeSlider.value = PlayerSettings.singleton.FXVolume;
    }

    public void UpdateUI_Video()
    {
        FullScreenToggle.isOn = PlayerSettings.singleton.FullScreen;
        resolutionOptions.value = PlayerSettings.singleton.ResolutionOption;
        resolutionOptions.RefreshShownValue();
        switch (PlayerSettings.singleton.FrameRate)
        {
            case 30:
                {
                    FrameRateButton_30FPS.interactable = false;
                    FrameRateButton_55FPS.interactable = true;
                    FrameRateButton_60FPS.interactable = true;
                    FrameRateButton_144FPS.interactable = true;
                    break;
                }
            case 55:
                {
                    FrameRateButton_30FPS.interactable = true;
                    FrameRateButton_55FPS.interactable = false;
                    FrameRateButton_60FPS.interactable = true;
                    FrameRateButton_144FPS.interactable = true;
                    break;
                }
            case 60:
                {
                    FrameRateButton_30FPS.interactable = true;
                    FrameRateButton_55FPS.interactable = true;
                    FrameRateButton_60FPS.interactable = false;
                    FrameRateButton_144FPS.interactable = true;
                    break;
                }
            case 144:
                {
                    FrameRateButton_30FPS.interactable = true;
                    FrameRateButton_55FPS.interactable = true;
                    FrameRateButton_60FPS.interactable = true;
                    FrameRateButton_144FPS.interactable = false;
                    break;
                }
        }
    }

    private void UpdateBGMVolumeText(float value)
    {
        int intValue = Mathf.RoundToInt(value * 100f); // Convert float value to integer
        BGMVolumeText.text = intValue.ToString(); // Update BGM volume text as integer
    }

    private void UpdateFXVolumeText(float value)
    {
        int intValue = Mathf.RoundToInt(value * 100f); // Convert float value to integer
        FXVolumeText.text = intValue.ToString(); // Update FX volume text as integer
    }

    public void ChangeResolution_WithPopWindow(int option)
    {
        ChangeResolution(option, true);
    }

    public void ChangeFullScreen_WithPopWindow(bool isFullScreen)
    {
        ChangeFullScreen(isFullScreen, true);
    }

    public void ChangeFrameRate_WithPopWindow(int frameRate)
    {
        ChangeFrameRate(frameRate, true);
    }

    public void ChangeResolution(int option, bool popWindow)
    {
        if (popWindow && hasInitialized && !isResetting)
        {
            PopWindowManager.singleton.Pop_ConfirmResolutionOrFrameRateChange(
                PlayerSettings.singleton.ResolutionOption,
                PlayerSettings.singleton.FrameRate,
                PlayerSettings.singleton.FullScreen,
                this
                );
        }

        PlayerSettings.singleton.Resolution_X = resolutionOptionList[option].Item1;
        PlayerSettings.singleton.Resolution_Y = resolutionOptionList[option].Item2;
        PlayerSettings.singleton.ResolutionOption = option;
        PlayerSettings.singleton.SetPlayerSettings_Resolution_Option();
        PlayerSettings.singleton.UpdateResolution();
    }

    public void ChangeFullScreen(bool isFullScreen, bool popWindow)
    {
        if (popWindow && hasInitialized && !isResetting)
        {
            PopWindowManager.singleton.Pop_ConfirmResolutionOrFrameRateChange(
                PlayerSettings.singleton.ResolutionOption,
                PlayerSettings.singleton.FrameRate,
                PlayerSettings.singleton.FullScreen,
                this
                );
        }

        PlayerSettings.singleton.FullScreen = isFullScreen;
        PlayerSettings.singleton.SetPlayerSettings_FullScreen();
        PlayerSettings.singleton.UpdateResolution();
    }

    public void ChangeFrameRate(int frameRate, bool popWindow)
    {
        if (popWindow && hasInitialized && !isResetting)
        {
            PopWindowManager.singleton.Pop_ConfirmResolutionOrFrameRateChange(
                PlayerSettings.singleton.ResolutionOption,
                PlayerSettings.singleton.FrameRate,
                PlayerSettings.singleton.FullScreen,
                this
                );
        }

        PlayerSettings.singleton.FrameRate = frameRate;
        PlayerSettings.singleton.SetPlayerSettings_FrameRate();
        PlayerSettings.singleton.UpdateFrameRate();

        switch (PlayerSettings.singleton.FrameRate)
        {
            case 30:
                {
                    FrameRateButton_30FPS.interactable = false;
                    FrameRateButton_55FPS.interactable = true;
                    FrameRateButton_60FPS.interactable = true;
                    FrameRateButton_144FPS.interactable = true;
                    break;
                }
            case 55:
                {
                    FrameRateButton_30FPS.interactable = true;
                    FrameRateButton_55FPS.interactable = false;
                    FrameRateButton_60FPS.interactable = true;
                    FrameRateButton_144FPS.interactable = true;
                    break;
                }
            case 60:
                {
                    FrameRateButton_30FPS.interactable = true;
                    FrameRateButton_55FPS.interactable = true;
                    FrameRateButton_60FPS.interactable = false;
                    FrameRateButton_144FPS.interactable = true;
                    break;
                }
            case 144:
                {
                    FrameRateButton_30FPS.interactable = true;
                    FrameRateButton_55FPS.interactable = true;
                    FrameRateButton_60FPS.interactable = true;
                    FrameRateButton_144FPS.interactable = false;
                    break;
                }
        }
    }

    public void ChangeBGMVolume(float value)
    {
        PlayerSettings.singleton.BGMVolume = value;
        PlayerSettings.singleton.SetPlayerSettings_BGMVolume();
        PlayerSettings.singleton.UpdateAudioVolumes();
    }

    public void ChangeFXVolume(float value)
    {
        PlayerSettings.singleton.FXVolume = value;
        PlayerSettings.singleton.SetPlayerSettings_FXVolume();
        PlayerSettings.singleton.UpdateAudioVolumes();
    }
}
