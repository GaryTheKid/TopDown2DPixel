using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using MoreMountains.Feedbacks;

public class PlayerNetworkController : MonoBehaviourPunCallbacks
{
    public string playerID;
    public ScoreboardTag scoreboardTag;
    public TextMeshPro TMP_Name;

    private PlayerEffectController _playerEffectController;
    private PlayerMovementController _playerMovementController;
    private PhotonView _PV;
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject _playerVCam;
    [SerializeField] private GameObject _minimapCam;
    [SerializeField] private GameObject _minimap;
    [SerializeField] private GameObject _audioListener;
    [SerializeField] private GameObject _ui_Canvas;
    [SerializeField] private GameObject _HitBox;
    [SerializeField] private GameObject _CharacterCollider;
    [SerializeField] private GameObject _ScreenSpaceFX;
    [SerializeField] private GameObject _rainingFX;
    [SerializeField] private GameObject _postProcessing;
    [SerializeField] private GameObject _ui_channeling;
    [SerializeField] private GameObject _ui_CastText;
    [SerializeField] private GameObject _castIndicators;
    [SerializeField] private GameObject _ui_WeaponCursors;
    [SerializeField] private GameObject _WeaponIndicators;
    [SerializeField] private GameObject _playerVision;
    [SerializeField] private SpriteRenderer _ringSprite;
    [SerializeField] private SpriteRenderer _minimap_Indicator;
    [SerializeField] private MMF_Player _mmf_ReceiveDamage;
    [SerializeField] private MMF_Player _mmf_ReceiveHealing;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Gradient _enemyHpBarGradient;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _characterLight;
    [SerializeField] private Transform[] _emojiAnchors;
    [SerializeField] private Transform[] _socialWheelMenuAnchors;
    [SerializeField] private WindyFX _windyFX;
    [SerializeField] private RainningFX _RainningFX;
    [SerializeField] private AudioSource _sfx;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _playerEffectController = GetComponent<PlayerEffectController>();
        _playerMovementController = GetComponent<PlayerMovementController>();
    }

    private void Start()
    {
        // instantiate character avatar
        StartCoroutine(_co_instantiate_playerCustomProperties());

        // configure networked objs
        ConfigureNetworkedObjs();

        // update name
        UpdatePlayerInfo();
    }

    private new void OnEnable()
    {
        SFXManager.singleton.Add(_sfx);
        if (_PV.IsMine)
        {
            SFXManager.singleton.SetWeatherSFXs(_windyFX, _RainningFX);
        }
    }

    private new void OnDisable()
    {
        SFXManager.singleton.Remove(_sfx);
    }

    private void OnDestroy()
    {
        SFXManager.singleton.Remove(_sfx);
    }

    public void UpdatePlayerInfo()
    {
        string nameAndID = _PV.Owner.ToString().Split('\'')[1];
        TMP_Name.text = nameAndID.Substring(0, nameAndID.IndexOf("#"));
    }

    private void ConfigureNetworkedObjs()
    {
        if (!_PV.IsMine)
        {
            var _playerWeaponController = GetComponent<PlayerWeaponController>();
            _playerWeaponController.EquipHands();
            _playerWeaponController.enabled = false;
            GetComponent<PlayerInteractionController>().enabled = false;
            _audioListener.SetActive(false);
            _ScreenSpaceFX.SetActive(false);
            _postProcessing.SetActive(false);
            _rainingFX.SetActive(false);
            _playerCamera.SetActive(false);
            _playerVCam.SetActive(false);
            _minimapCam.SetActive(false);
            _minimap.SetActive(false);
            _ui_Canvas.SetActive(false);
            _ui_channeling.SetActive(false);
            _ui_CastText.SetActive(false);
            _ui_WeaponCursors.SetActive(false);
            _WeaponIndicators.SetActive(false);
            _castIndicators.SetActive(false);
            _playerVision.SetActive(false);
            _CharacterCollider.layer = LayerMask.NameToLayer("Enemy");
            transform.parent = GameManager.singleton.spawnedPlayerParent;
            _ringSprite.color = Color.red;
            _minimap_Indicator.color = Color.red;
            _hpBar.color = Color.red;
            _mmf_ReceiveDamage.GetFeedbackOfType<MMF_Image>().ColorOverTime = _enemyHpBarGradient;
            _mmf_ReceiveHealing.GetFeedbackOfType<MMF_Image>().ColorOverTime = _enemyHpBarGradient;
            _characterLight.color = Color.red;
        }
        else
        {
            _HitBox.tag = "Untagged";
            _ringSprite.color = Color.green;
            _minimap_Indicator.color = Color.green;
            _characterLight.color = Color.white;
        }
    }

    private IEnumerator _co_instantiate_playerCustomProperties()
    {
        // instantiate the avatar
        var avatar = Instantiate(PlayerAssets.singleton.PlayerCharacterAvatarList[(int)_PV.Owner.CustomProperties["CharacterIndex"]], transform.position, Quaternion.identity);
        avatar.transform.parent = transform;

        // wait until avatar has been instantiated
        yield return new WaitUntil( () => { return avatar != null; });

        // setup avatar references
        Transform avatarBody = avatar.transform.Find("Body");
        Transform avatarGhostRunTrailFX = avatar.transform.Find("GhostRunTrailFX");
        _mmf_ReceiveDamage.GetFeedbackOfType<MMF_SpriteRenderer>().BoundSpriteRenderer = avatarBody.GetComponent<SpriteRenderer>();
        _mmf_ReceiveDamage.GetFeedbackOfType<MMF_SquashAndStretch>().SquashAndStretchTarget = avatarBody;
        _mmf_ReceiveDamage.GetFeedbackOfType<MMF_Wiggle>().TargetWiggle = avatar.GetComponent<MMWiggle>();
        _mmf_ReceiveHealing.GetFeedbackOfType<MMF_SpriteRenderer>().BoundSpriteRenderer = avatarBody.GetComponent<SpriteRenderer>();
        _playerEffectController.SetAvatarAnimation(avatar.GetComponent<Animator>(), avatarGhostRunTrailFX.gameObject, avatarBody.GetComponent<SpriteRenderer>());
        _playerMovementController.SetAvatarAnimation(avatar.GetComponent<Animator>(), avatarGhostRunTrailFX.GetComponent<Animator>());

        // instantiate emojis
        for (int i = 0; i < _emojiAnchors.Length; i++)
        {
            var list = (int[])_PV.Owner.CustomProperties["SocialIndexList"];
            if (list[i] != -1)
            {
                Instantiate(PlayerAssets.singleton.SocialInteractionList[list[i]].worldObj, _emojiAnchors[i]);
                Instantiate(PlayerAssets.singleton.SocialInteractionList[list[i]].staticObj, _socialWheelMenuAnchors[i]);
            }
        }
    }
}