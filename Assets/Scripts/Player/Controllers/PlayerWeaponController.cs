using System.Collections;
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
    public Transform fireTransform;
    public Transform spreadTransform;
    public Transform animationTransform;
    private PhotonView _PV;
    private Rigidbody2D _rb;
    private Inventory _inventory;
    private PlayerStats _playerStats;
    private IEnumerator _co_Attack;
    private IEnumerator _co_Charge;
    private float chargeMoveSpeedBuffer;
    private float attackMoveSpeedBuffer;
    private bool isFlipped;

    public int chargeTier;

    public enum WeaponType
    {
        Null,
        Melee,
        Range,
        ChargableRange,
        Magic
    }

    public WeaponType weaponType;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _playerStats = GetComponent<PlayerStatsController>().playerStats;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _inventory = gameObject.GetComponent<PlayerInventoryController>().GetInventory();
    }

    private void Update()
    {
        if (_playerStats.isDead)
            return;

        if (weapon == null || weaponAnimator == null || aimTransform == null)
            return;

        HandleAiming();
        HandleAttack();
    }

    public void EquipWeapon(Weapon weapon)
    {
        bool isEquipping = true;
        if (this.weapon != null)
        {
            isEquipping = weapon.itemName != this.weapon.itemName;
            UnequipWeapon();
        }
        
        if (isEquipping)
        {
            this.weapon = weapon;
            weaponPrefab = Instantiate(weapon.GetEquipmentPrefab(), spreadTransform);
            weaponAnimator = weaponPrefab.GetComponent<Animator>();
            fireTransform = weaponPrefab.Find("FirePos");
        }
    }

    public void UnequipWeapon()
    {
        weapon = null;
        weaponAnimator = null;
        fireTransform = null;
        if (weaponPrefab != null)
        {
            Destroy(weaponPrefab.gameObject);
            weaponPrefab = null;
        }

        // reset
        chargeTier = 0;

        // stop charge coroutine
        if (_co_Charge != null)
        {
            StopCoroutine(_co_Charge);
            _co_Charge = null;

            // restore movement speed
            _playerStats.speed = chargeMoveSpeedBuffer;
        }
    }

    // handle the weapon aimming
    public void HandleAiming()
    {
        if (_playerStats.isWeaponLocked)
            return;

        Vector3 mousePosition = Common.GetMouseWorldPosition();
        Vector3 aimDir = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0f, 0f, angle);

        // flip if rotate over 90 deg
        if (weaponPrefab != null && (angle > 90f || angle < -90f))
        {
            if (!isFlipped)
            {
                NetworkCalls.Weapon.FlipWeapon(_PV);
                isFlipped = true;
            }
        }
        else
        {
            if (isFlipped)
            {
                NetworkCalls.Weapon.UnflipWeapon(_PV);
                weaponPrefab.localEulerAngles = new Vector3(0f, 0f, 0f);
                isFlipped = false;
            }
        }
    }

    // handle the weapon attack
    public void HandleAttack()
    {
        if (_playerStats.isWeaponLocked)
            return;

        if (weapon == null)
            return;

        switch (weaponType)
        {
            case WeaponType.Melee:
                // click to attack
                if (Input.GetMouseButtonDown(0) && _co_Attack == null)
                {
                    _co_Attack = Co_Attack();
                    StartCoroutine(_co_Attack);
                }
                break;

            case WeaponType.Range:
                // click to attack
                if (Input.GetMouseButton(0) && _co_Attack == null)
                {
                    _co_Attack = Co_Attack();
                    StartCoroutine(_co_Attack);
                }
                break;

            case WeaponType.ChargableRange:
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
                        _playerStats.speed = chargeMoveSpeedBuffer;
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

    // Coroutine: Weapon attack
    private IEnumerator Co_Attack()
    {
        // lock aim
        //_playerStats.isWeaponLocked = true;

        // attack
        weapon.Attack(_PV);

        // spread
        if (weapon.accuracy < 1f)
        {
            StartCoroutine(Co_Spread());
        }

        // slow down movement during charge
        attackMoveSpeedBuffer = _playerStats.speed;
        _playerStats.speed *= weapon.attackMoveSlowRate;

        // recoil force
        _rb.AddForce(-Math.DegreeToVector2(aimTransform.eulerAngles.z) * weapon.recoilForce, ForceMode2D.Impulse);

        // wait cd
        yield return new WaitForSecondsRealtime(1f / weapon.attackSpeed);

        // unlock aim
        //_playerStats.isWeaponLocked = false;

        // restore movement speed
        _playerStats.speed = attackMoveSpeedBuffer;

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
        chargeMoveSpeedBuffer = _playerStats.speed;
        _playerStats.speed *= weapon.chargeMoveSlowRate;

        while (true)
        {
            // charge
            weapon.Charge(_PV);

            // wait cd
            yield return new WaitForSecondsRealtime(1f / weapon.chargeSpeed);
        }
    }
}
