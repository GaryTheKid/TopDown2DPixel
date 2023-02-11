using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
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
    [SerializeField] private MMF_Player _mmf_ReceiveDamage;
    [SerializeField] private MMF_Player _mmf_ReceiveHealing;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Gradient _enemyHpBarGradient;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _characterLight;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _playerEffectController = GetComponent<PlayerEffectController>();
        _playerMovementController = GetComponent<PlayerMovementController>();
    }

    private void Start()
    {
        // instantiate character avatar
        StartCoroutine(_co_instantiate_avatar());

        // configure networked objs
        ConfigureNetworkedObjs();

        // update name
        UpdatePlayerInfo();
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
            _hpBar.color = Color.red;
            _mmf_ReceiveDamage.GetFeedbackOfType<MMF_Image>().ColorOverTime = _enemyHpBarGradient;
            _mmf_ReceiveHealing.GetFeedbackOfType<MMF_Image>().ColorOverTime = _enemyHpBarGradient;
            _characterLight.color = Color.red;
        }
        else
        {
            _HitBox.tag = "Untagged";
            _ringSprite.color = Color.green;
            _characterLight.color = Color.white;
        }
    }

    private IEnumerator _co_instantiate_avatar()
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
    }
}