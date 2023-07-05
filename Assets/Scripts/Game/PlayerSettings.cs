/* Last Edition: 12/20/2022
 * Editor: Chongyang Wang
 * Collaborators: 
 * References: 
 * Description: 
 *    The singleton holds player's custom info for PhotonNetworking.
 * Lastest Update:
 *     Migirated from Chongyang's multiplayer project.
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
using System.Collections;

public class PlayerSettings : MonoBehaviour
{
    #region Fields
    // singleton
    public static PlayerSettings singleton;

    // private fields
    private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
    private AudioSource bgmManager_Menu;
    private BGMManager bgmManager_Game;
    private SFXManager sfxManager_Game;
    private IEnumerator Co_SyncPlayerCustomProperty;

    [Header("Local Settings")]
    [Header("Audio")]
    [SerializeField] private float _BGMVolume;
    public float BGMVolume
    {
        get
        {
            return _BGMVolume;
        }
        set
        {
            _BGMVolume = value;
        }
    }

    [SerializeField] private float _FXVolume;
    public float FXVolume
    {
        get
        {
            return _FXVolume;
        }
        set
        {
            _FXVolume = value;
        }
    }

    [Header("Video")]
    [SerializeField] private bool _isFullScreen;
    public bool FullScreen
    {
        get
        {
            return _isFullScreen;
        }
        set
        {
            _isFullScreen = value;
        }
    }

    [SerializeField] private int _resolution_x;
    public int Resolution_X
    {
        get
        {
            return _resolution_x;
        }
        set
        {
            _resolution_x = value;
        }
    }

    [SerializeField] private int _resolution_y;
    public int Resolution_Y
    {
        get
        {
            return _resolution_y;
        }
        set
        {
            _resolution_y = value;
        }
    }

    [SerializeField] private int _resolution_option;
    public int ResolutionOption
    {
        get
        {
            return _resolution_option;
        }
        set
        {
            _resolution_option = value;
        }
    }

    [SerializeField] private int _frameRate;
    public int FrameRate
    {
        get
        {
            return _frameRate;
        }
        set
        {
            _frameRate = value;
        }
    }

    [Header("Player Data")]
    [SerializeField] private long _gold;
    public long Gold
    {
        get
        {
            return _gold;
        }
        set
        {
            _gold = value;
            UpdateCustomProperty();
        }
    }

    [SerializeField] private long _gem;
    public long Gem
    {
        get
        {
            return _gem;
        }
        set
        {
            _gem = value;
            UpdateCustomProperty();
        }
    }

    [SerializeField] private int _playerCharacterIndex; 
    public int PlayerCharacterIndex
    { 
        get 
        { 
            return _playerCharacterIndex; 
        }
        set 
        { 
            _playerCharacterIndex = value;
            UpdateCustomProperty();
        } 
    }

    [SerializeField] private int _playerCharacterSkinIndex;
    public int PlayerCharacterSkinIndex
    {
        get 
        { 
            return _playerCharacterSkinIndex; 
        }
        set 
        { 
            _playerCharacterSkinIndex = value;
            UpdateCustomProperty();
        }
    }

    [SerializeField] private int _playerIconIndex;
    public int PlayerIconIndex
    {
        get 
        { 
            return _playerIconIndex; 
        }
        set 
        {   _playerIconIndex = value;
            UpdateCustomProperty();
        }
    }

    [SerializeField] private int[] _playerSocialIndexList;
    public int[] PlayerSocialIndexList
    {
        get
        {
            return _playerSocialIndexList;
        }
        set
        {
            _playerSocialIndexList = new int[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                _playerSocialIndexList[i] = value[i];
            }
            UpdateCustomProperty();
        }
    }
    #endregion


    #region Unity Functions
    /// <summary>
    /// Set singleton
    /// </summary>
    private void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(singleton.gameObject);
        }
        singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion


    #region Custom Functions
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // menu scene
        if (scene.buildIndex == Networking_GameSettings.singleton.menuSceneIndex)
        {
            bgmManager_Menu = GameObject.Find("BGMManager").GetComponent<AudioSource>();
            UpdateAudioVolumes();
        }

        // game scene
        if (scene.buildIndex == Networking_GameSettings.singleton.gameSceneIndex)
        {
            bgmManager_Game = GameObject.Find("BGMManager").GetComponent<BGMManager>();
            sfxManager_Game = GameObject.Find("SFXManager").GetComponent<SFXManager>();
            UpdateAudioVolumes();
        }
    }

    /// <summary>
    /// Update player's all custom properties to Photon
    /// </summary>
    public void UpdateCustomProperty()
    {
        // setup all player purchased custom assets
        _playerCustomProperties["CharacterIndex"] = _playerCharacterIndex;
        _playerCustomProperties["SkinIndex"] = _playerCharacterSkinIndex;
        _playerCustomProperties["IconIndex"] = _playerIconIndex;
        _playerCustomProperties["SocialIndexList"] = _playerSocialIndexList;
        PhotonNetwork.LocalPlayer.CustomProperties = _playerCustomProperties;
    }

    public void UpdateAudioVolumes()
    {
        UpdateSceneBGMVolume();
        UpdateSceneFXVolume();
    }

    private void UpdateSceneBGMVolume()
    {
        // bgm
        if (bgmManager_Menu != null)
        {
            bgmManager_Menu.volume = BGMVolume;
        }
        if (bgmManager_Game != null)
        {
            bgmManager_Game.SetVolumeModifier(BGMVolume);
        }
    }

    private void UpdateSceneFXVolume()
    {
        // fx
        if (sfxManager_Game != null)
        {
            sfxManager_Game.SetAllSFXVolume(FXVolume);
        }
    }

    public void UpdateResolution()
    {
        Debug.Log("Update Resolution:" + Resolution_X + "*" + Resolution_Y + " " +_isFullScreen);
        Screen.SetResolution(_resolution_x, _resolution_y, _isFullScreen);
    }

    public void UpdateFrameRate()
    {
        Debug.Log("Update FrameRate:" + _frameRate);
        Application.targetFrameRate = _frameRate;
    } 

    public void SyncPlayerCustomProperty_All()
    {
        if (Co_SyncPlayerCustomProperty != null)
        {
            StopCoroutine(Co_SyncPlayerCustomProperty);
            Co_SyncPlayerCustomProperty = null;
        }
        Co_SyncPlayerCustomProperty = SyncPlayerCustomProperty_All_Co();
        StartCoroutine(Co_SyncPlayerCustomProperty);
    }

    private IEnumerator SyncPlayerCustomProperty_All_Co()
    {
        yield return new WaitForSeconds(CloudCommunicator.singleton.singleRequestSyncCD);

        SetPlayerSettings_All();
    }

    public void SetPlayerSettings_All()
    {
        SetPlayerSettings_BGMVolume();
        SetPlayerSettings_FXVolume();
        SetPlayerSettings_FullScreen();
        SetPlayerSettings_Resolution_Option();
        SetPlayerSettings_FrameRate();
    }

    public void SetPlayerSettings_BGMVolume()
    {
        CloudCommunicator.singleton.BGMVolume = BGMVolume;
        CloudCommunicator.singleton.SyncPlayerCustomDataToCloud("BGMVolume", BGMVolume, (bool isSuccessful) =>
        {
            if (isSuccessful)
            {
                Debug.Log("BGMVolume sync to cloud successfully");
            }
            else
            {
                CloudCommunicator.singleton.PopCloudConnectionFailUI();
            }
        });
    }

    public void SetPlayerSettings_FXVolume()
    {
        CloudCommunicator.singleton.FXVolume = FXVolume;
        CloudCommunicator.singleton.SyncPlayerCustomDataToCloud("FXVolume", FXVolume, (bool isSuccessful) =>
        {
            if (isSuccessful)
            {
                Debug.Log("FXVolume sync to cloud successfully");
            }
            else
            {
                CloudCommunicator.singleton.PopCloudConnectionFailUI();
            }
        });
    }

    public void SetPlayerSettings_FullScreen()
    {
        CloudCommunicator.singleton.isFullScreen = FullScreen;
        CloudCommunicator.singleton.SyncPlayerCustomDataToCloud("isFullScreen", FullScreen, (bool isSuccessful) =>
        {
            if (isSuccessful)
            {
                Debug.Log("FullScreen sync to cloud successfully");
            }
            else
            {
                CloudCommunicator.singleton.PopCloudConnectionFailUI();
            }
        });
    }

    public void SetPlayerSettings_Resolution_Option()
    {
        CloudCommunicator.singleton.resolutionOption = ResolutionOption;
        CloudCommunicator.singleton.SyncPlayerCustomDataToCloud("resolutionOption", ResolutionOption, (bool isSuccessful) =>
        {
            if (isSuccessful)
            {
                Debug.Log("ResolutionOption sync to cloud successfully");
            }
            else
            {
                CloudCommunicator.singleton.PopCloudConnectionFailUI();
            }
        });
    }

    public void SetPlayerSettings_FrameRate()
    {
        CloudCommunicator.singleton.frameRate = FrameRate;
        CloudCommunicator.singleton.SyncPlayerCustomDataToCloud("frameRate", FrameRate, (bool isSuccessful) =>
        {
            if (isSuccessful)
            {
                Debug.Log("FrameRate sync to cloud successfully");
            }
            else
            {
                CloudCommunicator.singleton.PopCloudConnectionFailUI();
            }
        });
    }
    #endregion
}