using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _dashAmount;
    [SerializeField] private LayerMask _dashLayerMask;
    [SerializeField] private Animator _animator;

    private PlayerStatsController _playerStatsController;
    private PlayerStats _playerStats;
    private Rigidbody2D _rb;
    private Vector3 _moveDir;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStatsController = GetComponent<PlayerStatsController>();
        _playerStats = _playerStatsController.playerStats;
    }

    // Update is called once per frame
    private void Update()
    {
        /// player dead will turn to ghost, and move even faster!

        if (_playerStats.isMovementLocked)
            return;

        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
        }

        _moveDir = new Vector3(moveX, moveY).normalized;

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            isDashing = true;
        }*/
    }

    private void FixedUpdate()
    {
        /// player dead will turn to ghost, and move even faster!

        if (_playerStats.isMovementLocked)
            return;

        bool isIdle = _moveDir == Vector3.zero;

        if (isIdle)
        {
            // Idle
            if (_animator.GetBool("isMoving"))
                _animator.SetBool("isMoving", false);
        }
        else
        {
            // is moving
            _rb.AddForce(_moveDir * _playerStatsController.GetCurrentSpeed());
            if (!_animator.GetBool("isMoving"))
            {
                _animator.SetBool("isMoving", true);
            }
            _animator.SetFloat("moveX", _moveDir.x);
        }

        /*if (isDashing)
        {
            Vector3 dashPosition = transform.position + moveDir * dashAmount;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, moveDir, dashAmount, dashLayerMash);
            if (raycastHit2D.collider != null)
            {
                dashPosition = raycastHit2D.point;
            }
            rigidbody2D.MovePosition(dashPosition);
            isDashing = false;
        }*/
    }
}
