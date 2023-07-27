/* Last Edition Date: 02/11/2023
 * Author: Chongyang Wang
 * Collaborators: 
 * Reference: 
 * Description: 
 *   The weapon controller attached to the player character, handling all actions relating to weapon.
 * Last Edition:
 *   Add weapon cursor logic.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using Utilities;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class PlayerWeaponController : MonoBehaviour
{
    public WeaponCursor[] weaponCursor;
    public Weapon weapon;
    public Animator weaponAnimator;
    public Transform weaponPrefab;
    public Transform bareHandsPrefab;
    public Transform aimTransform;
    public Transform fireTransform;
    public Transform spreadTransform;
    public Transform deployTransform;
    public Transform animationTransform;
    public GameObject deployIndicator;
    public GameObject meleeIndicator;
    public AudioSource fireFX;
    public CastIndicatorController castIndicatorController;
    public UI_ChannelingBar ui_ChannelingBar;
    public TextMeshProUGUI castText;
    private PhotonView _PV;
    private Rigidbody2D _rb;
    private Inventory _inventory;
    private PlayerInventoryController _playerInventoryController;
    private PlayerEffectController _playerEffectController;
    private PlayerStatsController _playerStatsController;
    private PlayerStats _playerStats;
    private IEnumerator _co_Attack;
    private IEnumerator _co_Charge;
    private IEnumerator _co_Cast;
    private IEnumerator _co_Channel;
    private IEnumerator _co_Unleash;
    private IEnumerator _co_Deploy;
    private IEnumerator _co_Slow;
    private bool _isFlipped;
    private bool _isHolding;
    private float _aimAngle;
    private Vector2 _castPos;
    private WeaponCursor currentWeaponCursor;

    public bool isDeployable;
    public int chargeTier;
    public Item.ItemType weaponType;
    public float weaponRecoilModifier;

    private PCInputActions _inputActions;

#if PLATFORM_ANDROID
    [SerializeField] private UI_MobileInput mobileInput;
#endif

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _playerStatsController = GetComponent<PlayerStatsController>();
        _playerStats = _playerStatsController.playerStats;
        _playerInventoryController = GetComponent<PlayerInventoryController>();
        _playerEffectController = GetComponent<PlayerEffectController>();
        _rb = GetComponent<Rigidbody2D>();
        weaponType = Item.ItemType.Null;

        _inputActions = GetComponent<PlayerInputActions>().inputActions;
        //_inputActions.Player.FireOrChargeWeapon.canceled += HandleWeaponRelease;
        //_inputActions.Player.TouchCanceled.performed += HandleWeaponRelease;
    }

    private void Start()
    {
        _inventory = gameObject.GetComponent<PlayerInventoryController>().GetInventory();
        
        // equip bare hands
        EquipHands();
    }

    private void Update()
    {
        float pressVal = _inputActions.Player.FireOrChargeWeapon.ReadValue<float>();

        // handle input
        HandleAiming();
        if (pressVal == 1)
        {
            HandleWeaponHolding();
        }
        else
        {
            HandleWeaponRelease();
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        UnequipWeapon();
        bareHandsPrefab.gameObject.SetActive(false);

        // get weapon
        this.weapon = weapon;
        weaponType = weapon.itemType;

        // set cursor type
        SetWeaponCursor(weapon.cursorType);

        // set weapon
        weaponPrefab = Instantiate(weapon.GetEquipmentPrefab(), spreadTransform);
        weaponAnimator = weaponPrefab.GetComponent<Animator>();
        fireTransform = weaponPrefab.Find("FirePos");
        fireFX = weaponPrefab.Find("FireFX").GetComponent<AudioSource>();
        SFXManager.singleton.Add(fireFX);

        // flip if rotate over 90 deg
        FilpWeapon(_aimAngle);
    }

    public void UnequipWeapon()
    {
        if (fireFX != null) { SFXManager.singleton.Remove(fireFX); }
        weapon = null;
        weaponType = Item.ItemType.Null;
        weaponAnimator = null;
        fireTransform = null;
        fireFX = null;
        deployIndicator.SetActive(false);
        castIndicatorController.DeactivateIndicator();
        ui_ChannelingBar.Deactivate();
        if (weaponPrefab != null && weaponPrefab != bareHandsPrefab)
        {
            Destroy(weaponPrefab.gameObject);
            weaponPrefab = null;
        }
        _isFlipped = false;

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

        // stop cast coroutine
        if (_co_Cast != null)
        {
            StopCoroutine(_co_Cast);
            _co_Cast = null;
        }

        // stop channel coroutine
        if (_co_Channel != null)
        {
            StopCoroutine(_co_Channel);
            _co_Channel = null;

            // restore movement speed
            _playerStats.speedModifier = 1f;
        }

        // stop deploy coroutine
        if(_co_Deploy != null)
        {
            StopCoroutine(_co_Deploy);
            _co_Deploy = null;

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
        currentWeaponCursor = null;
        DeactivateAllWeaponCursor();
        deployIndicator.SetActive(false);
        meleeIndicator.SetActive(true);
        weaponAnimator = bareHandsPrefab.GetComponent<Animator>();
        fireFX = weaponPrefab.Find("FireFX").GetComponent<AudioSource>();
        SFXManager.singleton.Add(fireFX);
    }

    public void UnequipHands()
    {
        bareHandsPrefab.gameObject.SetActive(false);
        weaponPrefab = null;
        weapon = null;
        weaponType = Item.ItemType.Null;
        weaponAnimator = null;
        SFXManager.singleton.Remove(fireFX);
        fireFX = null;
    }

    // handle the weapon aimming
    public void HandleAiming()
    {
        if (_playerStats.isDead)
            return;

        if (_playerStats.isWeaponLocked)
            return;

#if PLATFORM_ANDROID
        // rotate weapon
        var input = _inputActions.Player.Aim.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            _aimAngle = Math.Vector2ToDegree(input);
            aimTransform.eulerAngles = new Vector3(0f, 0f, _aimAngle);

            // flip if rotate over 90 deg
            FilpWeapon(_aimAngle);
        }

#elif UNITY_EDITOR_WIN || PLATFORM_STANDALONE_WIN
        // rotate weapon
        var angle = Common.GetMouseRotationEulerAngle(transform.position);
        aimTransform.eulerAngles = new Vector3(0f, 0f, angle);

        // flip if rotate over 90 deg
        FilpWeapon(angle);
#endif
    }

    // handle the weapon attack
    public void HandleWeaponHolding()
    {
        if (_playerStats.isDead)
            return;

#if UNITY_EDITOR_WIN
        if (EventSystem.current.IsPointerOverGameObject())
            return;
#endif

        if (_playerInventoryController.IsUsingUI)
            return;

        if (_playerStats.isWeaponLocked)
            return;

        if (weapon == null)
            return;

        switch (weaponType)
        {
            case Item.ItemType.MeleeWeapon:
            case Item.ItemType.RangedWeapon:
                // click to attack
                if (_co_Attack == null)
                {
                    _co_Attack = Co_Attack();
                    StartCoroutine(_co_Attack);
                }
                break;

            case Item.ItemType.ChargableRangedWeapon:
            case Item.ItemType.ThrowableWeapon:
                // hold to charge
                if (_co_Charge == null && _co_Attack == null)
                {
                    // start charge coroutine
                    _co_Charge = Co_Charge();
                    StartCoroutine(_co_Charge);
                }
                break;

            case Item.ItemType.Scroll:
                // hold to charge
                if (_co_Cast == null && _co_Channel == null)
                {
                    // start charge coroutine
                    _co_Cast = Co_Cast();
                    StartCoroutine(_co_Cast);
                }
                break;

            case Item.ItemType.DeployableWeapon:
                // click to deploy
                if (_co_Deploy == null)
                {
                    _co_Deploy = Co_Deploy();
                    StartCoroutine(_co_Deploy);
                }
                break;
        }
    }

    public void HandleWeaponRelease()
    {
        if (!(_isHolding))
            return;

        switch (weaponType)
        {
            case Item.ItemType.ChargableRangedWeapon:
            case Item.ItemType.ThrowableWeapon:
                // when release charge
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
                break;

            case Item.ItemType.Scroll:
                // stop cast coroutine
                if (_co_Cast != null)
                {
                    StopCoroutine(_co_Cast);
                    _co_Cast = null;
                }

                // start channeling
                if (_co_Cast == null && _co_Channel == null)
                {
                    // check if cast is valid
                    if (!castIndicatorController.isCastValid)
                    {
                        castText.gameObject.SetActive(false);
                        castIndicatorController.DeactivateIndicator();
                        castIndicatorController.ShowInvalidCastText();
                    }
                    else
                    {
                        // start charge coroutine
                        castIndicatorController.DisableInvalidCastText();
                        _co_Channel = Co_Channeling();
                        StartCoroutine(_co_Channel);
                    }
                }
                break;
        }

        _isHolding = false;
    }

    // clear the attack coroutine
    public void ClearAttackCo()
    {
        if (_co_Attack != null)
        {
            StopCoroutine(_co_Attack);
            _co_Attack = null;
        }
    }

    // clear the charge coroutine
    public void ClearChargeCo()
    {
        if (_co_Charge != null)
        {
            StopCoroutine(_co_Charge);
            _co_Charge = null;
        }
    }

    // Flip weapon transform if > 90 or < -90 deg
    private void FilpWeapon(float angle)
    {
        // flip if rotate over 90 deg
        if (weaponPrefab != null && (angle > 90f || angle < -90f))
        {
            if (!_isFlipped)
            {
                NetworkCalls.Weapon_Network.FlipWeapon(_PV);
                _isFlipped = true;
            }
        }
        else
        {
            if (_isFlipped)
            {
                NetworkCalls.Weapon_Network.UnflipWeapon(_PV);
                _isFlipped = false;
            }
        }
    }

    /// <summary>
    /// Set the weapon cursor based on type
    /// </summary>
    /// <param name="cursorType"></param>
    private void SetWeaponCursor(Weapon.CursorType cursorType)
    {
        switch (cursorType)
        {
            case Weapon.CursorType.SemiAuto:
                currentWeaponCursor = weaponCursor[0];
                ActivateWeaponCursor(0);
                deployIndicator.SetActive(false);
                meleeIndicator.SetActive(false);
                break;
            case Weapon.CursorType.Shotgun:
                currentWeaponCursor = weaponCursor[1];
                ActivateWeaponCursor(1);
                deployIndicator.SetActive(false);
                meleeIndicator.SetActive(false);
                break;
            case Weapon.CursorType.Rifle:
                currentWeaponCursor = weaponCursor[2];
                ActivateWeaponCursor(2);
                deployIndicator.SetActive(false);
                meleeIndicator.SetActive(false);
                break;
            case Weapon.CursorType.Pistol:
                currentWeaponCursor = weaponCursor[3];
                ActivateWeaponCursor(3);
                deployIndicator.SetActive(false);
                meleeIndicator.SetActive(false);
                break;
            case Weapon.CursorType.Bow:
                currentWeaponCursor = weaponCursor[4];
                ActivateWeaponCursor(4);
                deployIndicator.SetActive(false);
                meleeIndicator.SetActive(false);
                break;
            case Weapon.CursorType.Scroll:
                currentWeaponCursor = weaponCursor[5];
                ActivateWeaponCursor(5);
                deployIndicator.SetActive(false);
                meleeIndicator.SetActive(false);
                break;
            case Weapon.CursorType.Throwable:
                currentWeaponCursor = weaponCursor[6];
                ActivateWeaponCursor(6);
                deployIndicator.SetActive(false);
                meleeIndicator.SetActive(false);
                break;
            case Weapon.CursorType.Melee:
                currentWeaponCursor = null;
                DeactivateAllWeaponCursor();
                deployIndicator.SetActive(false);
                meleeIndicator.SetActive(true);
                break;
            case Weapon.CursorType.Deployable:
                currentWeaponCursor = null;
                DeactivateAllWeaponCursor();
                deployIndicator.SetActive(true);
                meleeIndicator.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// Activate the selected Weapon Cursor
    /// </summary>
    /// <param name="activeCursorIndex"></param>
    private void ActivateWeaponCursor(int activeCursorIndex)
    {
        for (int i = 0; i < weaponCursor.Length; i++)
        {
            if (i == activeCursorIndex)
            {
                weaponCursor[i].gameObject.SetActive(true);
            }
            else
            {
                weaponCursor[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Deactivate all weapon cursors
    /// </summary>
    /// <returns></returns>
    private void DeactivateAllWeaponCursor()
    {
        for (int i = 0; i < weaponCursor.Length; i++)
        {
            weaponCursor[i].gameObject.SetActive(false);
        }
    }

    // Coroutine: Weapon attack
    private IEnumerator Co_Attack()
    {
        // check if in combat (attacking)
        _playerStatsController.InCombatStateCheck();

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
                weapon.Attack(_PV, weapon.damageInfo);
                break;
            case Item.ItemType.RangedWeapon:
            case Item.ItemType.ChargableRangedWeapon:
            case Item.ItemType.ThrowableWeapon:
                weapon.Attack(_PV, fireTransform.position, spreadTransform.eulerAngles.z);
                break;
        }

        // fire cursor feedback
        if (currentWeaponCursor != null)
        {
            currentWeaponCursor.FireAttackFeedbacks();
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
        _playerEffectController.CameraShake(weapon.recoilForce * weaponRecoilModifier * 0.5f, weapon.recoilTime * weaponRecoilModifier * 1.5f);

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
        if (!_playerStats.isDead && !(weapon is Hands) && weapon.itemType != Item.ItemType.ThrowableWeapon)
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
        _isHolding = true;

        // slow down movement during charge
        _playerStats.speedModifier = weapon.chargeMoveSlowRate * _playerStats.slowModifier;

        // fire cursor feedback
        if (currentWeaponCursor != null)
        {
            currentWeaponCursor.FireChargeFeedbacks();
        }

        while (true)
        {
            // charge
            weapon.Charge(_PV);

            // wait cd
            yield return new WaitForSecondsRealtime(1f / weapon.chargeSpeed);
        }
    }

    // Coroutine: Weapon cast
    private IEnumerator Co_Cast()
    {
        _isHolding = true;

        // set cast text
        castText.text = weapon.castText;
        castText.gameObject.SetActive(true);

        // set indicator
        castIndicatorController.DisableInvalidCastText();
        castIndicatorController.invalidCastLayerMask = weapon.invalidCastLayerMask;
        castIndicatorController.indicatorType = weapon.castIndicatorType;
        switch (castIndicatorController.indicatorType)
        {
            case Weapon.CastIndicatorType.Line:
                castIndicatorController.SetIndicator_Line(weapon.castLinearWidth, weapon.castRange);
                break;

        }
        castIndicatorController.ActivateIndicator();

        while (true)
        {

#if UNITY_EDITOR_WIN || PLATFORM_STANDALONE_WIN
            // rotate weapon
            _castPos = Common.GetMouseWorldPosition();
#endif
            castIndicatorController.PositionIndicator(_castPos);

            yield return new WaitForEndOfFrame();
        }
    }

    // Coroutine: Weapon channel
    private IEnumerator Co_Channeling()
    {
        // check if in combat (Channeling spell)
        _playerStatsController.InCombatStateCheck();

        // disable cast text
        castText.gameObject.SetActive(false);

        // slow down movement during charge
        _playerStats.speedModifier = weapon.castChannelMovementSlotRate * _playerStats.slowModifier;

        print("Start Channeling");
        // lock inventory while channeling
        _playerStats.isInventoryLocked = true;

        // show channeling bar
        ui_ChannelingBar.Activate();

        // channel animation
        weapon.Channel(_PV);

        // reset indicator
        castIndicatorController.DeactivateIndicator();

        // channel time
        var timer = 0f;
        var totalChannelTime = weapon.castChannelTime;
        while (timer < totalChannelTime)
        {
            timer += Time.deltaTime;

            // channel bar progress
            ui_ChannelingBar.UpdateProgress(timer / totalChannelTime);

            // wait
            yield return new WaitForEndOfFrame();
        }

        print("End Channeling");

        // unleash
        if (_co_Unleash == null)
        {
            _co_Unleash = Co_Unleash();
            StartCoroutine(_co_Unleash);
        }

        // hide channeling bar
        ui_ChannelingBar.Deactivate();

        // clear co
        _co_Channel = null;
    }

    // Coroutine: Weapon unleash
    private IEnumerator Co_Unleash()
    {
        // check if in combat (unleash spell)
        _playerStatsController.InCombatStateCheck();

        _isHolding = false;

        // slow down movement during charge
        _playerStats.speedModifier = 1f;

        // unleash
        weapon.Unleash(_PV, _castPos);

        // clear cast pos
        _castPos = Vector3.zero;

        // update durability
        if (!_playerStats.isDead && !(weapon is Hands) && weapon.itemType != Item.ItemType.ThrowableWeapon)
            _playerInventoryController.UpdateItemDurability(-1);

        // wait cd
        yield return new WaitForSecondsRealtime(weapon.unleashDelay);

        // unlock inventory after channeling
        _playerStats.isInventoryLocked = false;

        // clear co
        _co_Unleash = null;
    }

    // Coroutine: Weapon Deploy
    private IEnumerator Co_Deploy()
    {
        if (!isDeployable)
        {
            yield return null;

            // clear co
            _co_Deploy = null;
        }
        else
        {
            // check if in combat (deploying)
            _playerStatsController.InCombatStateCheck();

            // lock inventory
            float attackCD = 1f / weapon.attackSpeed;
            _playerInventoryController.SetInventoryOnCD(attackCD);

            // Deploy time
            //yield return new WaitForSecondsRealtime(weapon.castChannelTime);

            // Deploy
            switch (weaponType)
            {
                case Item.ItemType.DeployableWeapon:
                    weapon.Deploy(_PV, deployTransform.position);
                    break;
            }

            // slow down movement speed
            if (_co_Slow == null)
            {
                _co_Slow = Co_Slow();
                StartCoroutine(_co_Slow);
            }

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

            // wait cd
            yield return new WaitForSecondsRealtime(attackCD);

            // clear co
            _co_Deploy = null;
        }
    }

    // Coroutine: Weapon slow movement speed
    private IEnumerator Co_Slow()
    {
        // slow down movement during charge
        _playerStats.speedModifier = weapon.moveSlowDownModifier * _playerStats.slowModifier;

        // wait cd
        yield return new WaitForSecondsRealtime(weapon.moveSlowDownTime);

        // slow down movement during charge
        _playerStats.speedModifier = 1f;

        // clear co
        _co_Slow = null;
    }
}
