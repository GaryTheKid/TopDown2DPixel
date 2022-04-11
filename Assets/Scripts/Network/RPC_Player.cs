using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_Player : MonoBehaviour
{
    private PlayerWeaponController _playerWeaponController;
    private PlayerBuffController _playerBuffController;
    private HashSet<int> targets;

    private void Awake()
    {
        _playerWeaponController = GetComponent<PlayerWeaponController>();
        _playerBuffController = GetComponent<PlayerBuffController>();
    }

    private void Start()
    {
        targets = GetComponent<TargetList>().targets;
    }

    [PunRPC]
    void RPC_LockTarget(int targetID)
    {
        targets.Add(targetID);
    }

    [PunRPC]
    void RPC_UnlockTarget(int targetID)
    {
        targets.Remove(targetID);
    }

    [PunRPC]
    void RPC_DealDamage()
    {
        DamageInfo info = _playerWeaponController.weapon.damageInfo;

        foreach (int id in targets)
        {
            PhotonView.Find(id).transform.GetComponent<PlayerBuffController>().ReceiveDamage(info, transform.position);
        }
    }

    [PunRPC]
    void RPC_FireWeapon()
    {
        _playerWeaponController.weaponAnimator.SetTrigger("Attack");
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
            _playerWeaponController.weapon = null;

        if (_playerWeaponController.weaponAnimator != null)
            _playerWeaponController.weaponAnimator = null;

        if (_playerWeaponController.weaponPrefab != null)
        {
            Destroy(_playerWeaponController.weaponPrefab.gameObject);
            _playerWeaponController.weaponPrefab = null;
        }
    }
}