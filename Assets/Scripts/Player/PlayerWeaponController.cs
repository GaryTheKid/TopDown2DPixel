using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerWeaponController : MonoBehaviour
{
    public Weapon weapon;
    public Animator weaponAnimator;
    public Transform weaponPrefab;
    public Transform aimTransform;
    public Transform animationTransform;
    private PhotonView _PV;
    private Inventory _inventory;
    private PlayerStats _playerStats;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _playerStats = GetComponent<PlayerStatsController>().playerStates;
    }

    private void Start()
    {
        _inventory = gameObject.GetComponent<PlayerInventoryController>().GetInventory();
    }

    private void Update()
    {
        HandleAiming();
        HandleAttack();
    }

    public void EquipWeapon(Weapon weapon)
    {
        UnequipWeapon();
        weapon.Equip(_PV);
    }

    public void UnequipWeapon()
    {
        if (weapon != null)
        {
            weapon.Unequip(_PV);
        }
    }

    public void HandleAiming()
    {
        if (_playerStats.isWeaponLocked)
            return;

        Vector3 mousePosition = Common.GetMouseWorldPosition();
        Vector3 aimDir = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void HandleAttack()
    {
        if (_playerStats.isWeaponLocked)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (weapon == null)
            {
                Debug.Log("Please equip a weapon first!");
                return;
            }

            weapon.Attack(_PV, weaponAnimator, transform.position);
        }
    }
}
