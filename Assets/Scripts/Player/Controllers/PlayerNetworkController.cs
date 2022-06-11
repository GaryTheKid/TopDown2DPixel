using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworkController : MonoBehaviour
{
    [SerializeField] private PhotonView _PV;
    [SerializeField] private PlayerMovementController _movementController;
    [SerializeField] private PlayerWeaponController _playerWeaponController;
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject _audioListener;
    [SerializeField] private GameObject _ui_Canvas;
    [SerializeField] private GameObject _HitBox;
    [SerializeField] private GameObject _CharacterCollider;
    [SerializeField] private SpriteRenderer _characterSprite;

    private void Start()
    {
        if (!_PV.IsMine)
        {
            _playerWeaponController.EquipHands();
            Destroy(_movementController);
            Destroy(_playerWeaponController);
            Destroy(_playerCamera);
            Destroy(_audioListener);
            _ui_Canvas.SetActive(false);
            _CharacterCollider.layer = LayerMask.NameToLayer("Default");
            transform.parent = GameManager.gameManager.spawnedPlayerParent;
        }
        else 
        {
            _HitBox.tag = "Untagged";
            //characterSprite.color = Color.green;
        }
    }
}