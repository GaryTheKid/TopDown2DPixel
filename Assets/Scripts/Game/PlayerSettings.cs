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
using Photon.Pun;

public class PlayerSettings : MonoBehaviour
{
    #region Fields
    // singleton
    public static PlayerSettings singleton;

    // private fields
    private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();



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

    /// <summary>
    /// Init local settings
    /// </summary>
    private void Start()
    {

    }
    #endregion


    #region Custom Functions
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
    #endregion
}