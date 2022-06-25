using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerNetworkController : MonoBehaviourPunCallbacks
{
    public string playerID;
    public ScoreboardTag scoreboardTag;
    public TextMeshPro TMP_Name;

    private PhotonView _PV;
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject _audioListener;
    [SerializeField] private GameObject _ui_Canvas;
    [SerializeField] private GameObject _HitBox;
    [SerializeField] private GameObject _CharacterCollider;
    [SerializeField] private GameObject _ScreenSpaceFX;
    [SerializeField] private SpriteRenderer _characterSprite;

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
            Destroy(_playerWeaponController);
            Destroy(GetComponent<PlayerMovementController>());
            Destroy(GetComponent<PlayerInteractionController>());
            Destroy(_playerCamera);
            Destroy(_audioListener);
            Destroy(_ScreenSpaceFX);
            _ui_Canvas.SetActive(false);
            _CharacterCollider.layer = LayerMask.NameToLayer("Enemy");
            transform.parent = GameManager.gameManager.spawnedPlayerParent;
        }
        else 
        {
            _HitBox.tag = "Untagged";
            //characterSprite.color = Color.green;
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