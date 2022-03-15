using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Photon.Pun;

public class PlayerWeaponController : MonoBehaviour
{
    public Weapon weapon;
    public Animator weaponAnimator;
    public Transform weaponPrefab;
    public Transform aimTransform;
    private PhotonView _PV;
    private Inventory _inventory;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _inventory = gameObject.GetComponent<PlayerInventoryController>().GetInventory();
    }

    private void Update()
    {
        HandleAiming();
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
        Vector3 mousePosition = Common.GetMouseWorldPosition();
        Vector3 aimDir = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void HandleAttack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (weapon == null)
            {
                Debug.Log("Please equip a weapon first!");
                return;
            }

            weapon.Attack();
        }
    }
}
