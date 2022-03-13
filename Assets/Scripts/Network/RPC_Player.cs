using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_Player : MonoBehaviour
{
    private PlayerWeaponController _playerWeaponController;
    private PlayerBuffController _playerBuffController;

    private void Awake()
    {
        _playerWeaponController = GetComponent<PlayerWeaponController>();
        _playerBuffController = GetComponent<PlayerBuffController>();
    }

    [PunRPC]
    void RPC_DealDamage()
    {

    }

    [PunRPC]
    void RPC_UseHealthPotion(int healingAmount)
    {
        _playerBuffController.ReceiveHealing(healingAmount);
    }

    [PunRPC]
    void RPC_EquipSword()
    {
        Weapon sword = new Sword();
        _playerWeaponController.weapon = sword;
        _playerWeaponController.weaponPrefab = Instantiate(sword.GetEquipmentPrefab(), _playerWeaponController.aimTransform);
        _playerWeaponController.weaponAnimator = _playerWeaponController.weaponPrefab.GetComponent<Animator>();
    }

    [PunRPC]
    void RPC_EquipAxe()
    {
        Weapon axe = new Axe();
        _playerWeaponController.weapon = axe;
        _playerWeaponController.weaponPrefab = Instantiate(axe.GetEquipmentPrefab(), _playerWeaponController.aimTransform);
        _playerWeaponController.weaponAnimator = _playerWeaponController.weaponPrefab.GetComponent<Animator>();
    }

    [PunRPC]
    void RPC_EquipBow()
    {
        Weapon bow = new Bow();
        _playerWeaponController.weapon = bow;
        _playerWeaponController.weaponPrefab = Instantiate(bow.GetEquipmentPrefab(), _playerWeaponController.aimTransform);
        _playerWeaponController.weaponAnimator = _playerWeaponController.weaponPrefab.GetComponent<Animator>();
    }

    [PunRPC]
    void RPC_UnequipWeapon()
    {
        if (_playerWeaponController.weapon != null)
        {
            _playerWeaponController.weapon = null;
            _playerWeaponController.weaponAnimator = null;
            Destroy(_playerWeaponController.weaponPrefab);
        }
    }
}
