/* Last Edition: 05/22/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The weapon controller attached to the player character, handling all actions relating to weapon.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerWeaponController : MonoBehaviour
{
    public Weapon weapon;
    public Animator weaponAnimator;
    public Transform weaponPrefab;
    public Transform bareHandsPrefab;
    public Transform aimTransform;
    public Transform fireTransform;
    public Transform spreadTransform;
    public Transform animationTransform;
    public AudioSource fireFX;
    private PhotonView _PV;
    private Rigidbody2D _rb;
    private Inventory _inventory;
    private PlayerInventoryController _playerInventoryController;
    private PlayerEffectController _playerEffectController;
    private PlayerStats _playerStats;
    private IEnumerator _co_Attack;
    private IEnumerator _co_Charge;
    private IEnumerator _co_Slow;
    private bool isFlipped;

    public int chargeTier;
    public Item.ItemType weaponType;
    public float weaponRecoilModifier;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _playerStats = GetComponent<PlayerStatsController>().playerStats;
        _playerInventoryController = GetComponent<PlayerInventoryController>();
        _playerEffectController = GetComponent<PlayerEffectController>();
        _rb = GetComponent<Rigidbody2D>();
        weaponType = Item.ItemType.Null;
    }

    private void Start()
    {
        _inventory = gameObject.GetComponent<PlayerInventoryController>().GetInventory();
        
        // equip bare hands
        EquipHands();
    }

    private void Update()
    {
        if (_playerStats.isDead)
            return;

        HandleAiming();
        HandleAttack();
    }

    public void EquipWeapon(Weapon weapon)
    {
        /*// if has equipped weapon, unequip it
        bool isEquipping = true;
        if (this.weapon != null)
        {
            isEquipping = (weapon.itemName != this.weapon.itemName) && ();
            UnequipWeapon();
        }
        
        // equip new weapon
        if (isEquipping)
        {
            bareHandsPrefab.gameObject.SetActive(false);
            this.weapon = weapon;
            weaponType = weapon.itemType;
            weaponPrefab = Instantiate(weapon.GetEquipmentPrefab(), spreadTransform);
            weaponAnimator = weaponPrefab.GetComponent<Animator>();
            fireTransform = weaponPrefab.Find("FirePos");
            fireFX = weaponPrefab.Find("FireFX").GetComponent<AudioSource>();
        }*/

        UnequipWeapon();
        bareHandsPrefab.gameObject.SetActive(false);
        this.weapon = weapon;
        weaponType = weapon.itemType;
        weaponPrefab = Instantiate(weapon.GetEquipmentPrefab(), spreadTransform);
        weaponAnimator = weaponPrefab.GetComponent<Animator>();
        fireTransform = weaponPrefab.Find("FirePos");
        fireFX = weaponPrefab.Find("FireFX").GetComponent<AudioSource>();
    }

    public void UnequipWeapon()
    {
        weapon = null;
        weaponType = Item.ItemType.Null;
        weaponAnimator = null;
        fireTransform = null;
        fireFX = null;
        if (weaponPrefab != null && weaponPrefab != bareHandsPrefab)
        {
            Destroy(weaponPrefab.gameObject);
            weaponPrefab = null;
        }
        isFlipped = false;

        // reset charge Tier
        chargeTier = 0;

        // stop charge coroutine
        if (_co_Charge != null)
        {
            StopCoroutine(_co_Charge);
            _co_Charge = null;

            // restore movement speed
            _playerStats.speedModifier = 1f;
        }

        // equip bare hands
        EquipHands();
    }

    public void EquipHands()
    {
        bareHandsPrefab.gameObject.SetActive(true);
        weaponPrefab = bareHandsPrefab;
        weapon = new Hands();
        weaponType = Item.ItemType.MeleeWeapon;
        weaponAnimator = bareHandsPrefab.GetComponent<Animator>();
        fireFX = weaponPrefab.Find("FireFX").GetComponent<AudioSource>();
    }

    public void UnequipHands()
    {
        bareHandsPrefab.gameObject.SetActive(false);
        weaponPrefab = null;
        weapon = null;
        weaponType = Item.ItemType.Null;
        weaponAnimator = null;
        fireFX = null;
    }

    // handle the weapon aimming
    public void HandleAiming()
    {
        if (_playerStats.isWeaponLocked)
            return;

        // rotate weapon
        float angle = Common.GetMouseRotationEulerAngle(transform.position);
        aimTransform.eulerAngles = new Vector3(0f, 0f, angle);

        // flip if rotate over 90 deg
        FilpWeapon(angle);
    }

    // handle the weapon attack
    public void HandleAttack()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (_playerInventoryController.IsUsingUI)
            return;

        if (_playerStats.isWeaponLocked)
            return;

        if (weapon == null)
            return;

        switch (weaponType)
        {
            case Item.ItemType.MeleeWeapon:
                // click to attack
                if (Input.GetMouseButtonDown(0) && _co_Attack == null)
                {
                    _co_Attack = Co_Attack();
                    StartCoroutine(_co_Attack);
                }
                break;

            case Item.ItemType.RangedWeapon:
                // click to attack
                if (Input.GetMouseButton(0) && _co_Attack == null)
                {
                    _co_Attack = Co_Attack();
                    StartCoroutine(_co_Attack);
                }
                break;

            case Item.ItemType.ChargableRangedWeapon:
            case Item.ItemType.ThrowableWeapon:
                // hold to charge
                if (Input.GetMouseButton(0) && _co_Charge == null && _co_Attack == null)
                {
                    // start charge coroutine
                    _co_Charge = Co_Charge();
                    StartCoroutine(_co_Charge);
                }

                // when release charge
                if (Input.GetMouseButtonUp(0))
                {
                    // stop charge coroutine
                    if (_co_Charge != null)
                    {
                        StopCoroutine(_co_Charge);
                        _co_Charge = null;

                        // restore movement speed
                        _playerStats.speedModifier = 1f;
                    }

                    // initiate the attack coroutine 
                    if (_co_Attack == null)
                    {
                        _co_Attack = Co_Attack();
                        StartCoroutine(_co_Attack);
                    }
                    
                    // reset charge Tier
                    chargeTier = 0;
                }
                break;
            default: 
                return;
        }
        
    }

    // Flip weapon transform if > 90 or < -90 deg
    private void FilpWeapon(float angle)
    {
        // flip if rotate over 90 deg
        if (weaponPrefab != null && (angle > 90f || angle < -90f))
        {
            if (!isFlipped)
            {
                NetworkCalls.Weapon_Network.FlipWeapon(_PV);
                isFlipped = true;
            }
        }
        else
        {
            if (isFlipped)
            {
                NetworkCalls.Weapon_Network.UnflipWeapon(_PV);
                isFlipped = false;
            }
        }
    }

    // Coroutine: Weapon attack, melee
    private IEnumerator Co_Attack()
    {
        // lock inventory
        float attackCD = 1f / weapon.attackSpeed;
        _playerInventoryController.SetInventoryOnCD(attackCD);

        // spread
        if (weapon.accuracy < 1f)
        {
            StartCoroutine(Co_Spread());
        }

        // attack
        switch (weaponType)
        {
            case Item.ItemType.MeleeWeapon:
                weapon.Attack(_PV);
                break;
            case Item.ItemType.RangedWeapon:
            case Item.ItemType.ChargableRangedWeapon:
            case Item.ItemType.ThrowableWeapon:
                weapon.Attack(_PV, fireTransform.position, spreadTransform.eulerAngles.z);
                break;
        }

        // slow down movement speed
        if (_co_Slow == null)
        {
            _co_Slow = Co_Slow();
            StartCoroutine(_co_Slow);
        }

        // recoil force
        _rb.AddForce(-Math.DegreeToVector2(aimTransform.eulerAngles.z) * weapon.recoilForce, ForceMode2D.Impulse);

        // cam shake
        _playerEffectController.CameraShake(weapon.recoilForce * weaponRecoilModifier, weapon.recoilTime * weaponRecoilModifier * 2f);

        // check if throwable
        if (weaponType == Item.ItemType.ThrowableWeapon)
        {
            // throwing this weapon reduces its amount
            _inventory.RemoveOneItem(weapon);

            if (_inventory.GetItemAmountListById(weapon.itemID) <= 0)
            {
                weapon.Unequip(_PV);

                // restore movement speed
                _playerStats.speedModifier = 1f;

                // clear co
                yield return null;
            }
        }

        // wait cd
        yield return new WaitForSecondsRealtime(attackCD);

        // update durability
        if (!(weapon is Hands && weapon.itemType != Item.ItemType.ThrowableWeapon))
            _playerInventoryController.UpdateItemDurability(-1);

        // clear co
        _co_Attack = null;
    }

    // Coroutine: Weapon spread
    private IEnumerator Co_Spread()
    {
        // init
        float spreadVal = (1f - weapon.accuracy) * 180f;
        float spread = Random.Range(-spreadVal, spreadVal);
        int maxframeCount = (int)(weapon.recoilTime / Time.fixedDeltaTime);
        int counter = 0;
        float step = spread / maxframeCount;

        // rotate aim transform
        while (counter < maxframeCount)
        {
            spreadTransform.eulerAngles = new Vector3(0f, spreadTransform.eulerAngles.y, spreadTransform.eulerAngles.z + step);
            yield return new WaitForFixedUpdate();
            counter++;
        }

        // wait recover time
        yield return new WaitForSecondsRealtime(weapon.recoilRecoverTime);

        // recover
        spreadTransform.eulerAngles = new Vector3(0f, spreadTransform.eulerAngles.y, spreadTransform.eulerAngles.z - spread);
    }

    // Coroutine: Weapon charge
    private IEnumerator Co_Charge()
    {
        // slow down movement during charge
        _playerStats.speedModifier = weapon.chargeMoveSlowRate;

        while (true)
        {
            // charge
            weapon.Charge(_PV);

            // wait cd
            yield return new WaitForSecondsRealtime(1f / weapon.chargeSpeed);
        }
    }

    // Coroutine: Weapon slow movement speed
    private IEnumerator Co_Slow()
    {
        // slow down movement during charge
        _playerStats.speedModifier = weapon.moveSlowDownModifier;

        // wait cd
        yield return new WaitForSecondsRealtime(weapon.moveSlowDownTime);

        // slow down movement during charge
        _playerStats.speedModifier = 1f;

        // clear co
        _co_Slow = null;
    }
}
