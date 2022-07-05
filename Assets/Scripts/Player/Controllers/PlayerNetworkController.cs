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

    private PhotonView _PV;
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject _playerVCam;
    [SerializeField] private GameObject _audioListener;
    [SerializeField] private GameObject _ui_Canvas;
    [SerializeField] private GameObject _HitBox;
    [SerializeField] private GameObject _CharacterCollider;
    [SerializeField] private GameObject _ScreenSpaceFX;
    [SerializeField] private GameObject _postProcessing;
    [SerializeField] private SpriteRenderer _ringSprite;
    [SerializeField] private MMF_Player _mmf_hp;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Gradient _enemyHpBarGradient;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _characterLight;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>(); 
    }

    private void Start()
    {
        if (!_PV.IsMine)
        {
            var _playerWeaponController = GetComponent<PlayerWeaponController>();
            _playerWeaponController.EquipHands();
            _playerWeaponController.enabled = false;
            GetComponent<PlayerMovementController>().enabled = false;
            GetComponent<PlayerInteractionController>().enabled = false;
            Destroy(_audioListener);
            Destroy(_ScreenSpaceFX);
            Destroy(_postProcessing);
            _playerCamera.SetActive(false);
            _playerVCam.SetActive(false);
            _ui_Canvas.SetActive(false);
            _CharacterCollider.layer = LayerMask.NameToLayer("Enemy");
            transform.parent = GameManager.gameManager.spawnedPlayerParent;
            _ringSprite.color = Color.red;
            _hpBar.color = Color.red;
            _mmf_hp.GetFeedbackOfType<MMF_Image>().ColorOverTime = _enemyHpBarGradient;
            _characterLight.color = Color.red;
        }
        else 
        {
            _HitBox.tag = "Untagged";
            _ringSprite.color = Color.green;
            _characterLight.color = Color.white;
        }

        // update name
        UpdatePlayerInfo();
    }

    public void UpdatePlayerInfo()
    {
        string nameAndID = _PV.Owner.ToString().Split('\'')[1];
        TMP_Name.text = nameAndID.Substring(0, nameAndID.IndexOf("#"));
    }
}