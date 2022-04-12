using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 60f;
    [SerializeField] private float _dashAmount = 20f;
    [SerializeField] private LayerMask _dashLayerMask;
    [SerializeField] private Animator _animator;

    private PlayerStats _playerStats;
    private Rigidbody2D _rb;
    private Vector3 _moveDir;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStats = GetComponent<PlayerStatsController>().playerStats;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_playerStats.isDead)
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
        if (_playerStats.isDead)
            return;

        bool isIdle = _moveDir == Vector3.zero;

        if (isIdle)
        {
            // Idle
            _animator.SetBool("isMoving", false);
        }
        else
        {
            // is moving
            _rb.AddForce(_moveDir * _moveSpeed);
            _animator.SetFloat("xMovement", _moveDir.x);
            _animator.SetFloat("yMovement", _moveDir.y);
            _animator.SetBool("isMoving", true);
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
