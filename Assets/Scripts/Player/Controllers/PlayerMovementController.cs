using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _dashAmount;
    [SerializeField] private LayerMask _dashLayerMask;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _ghostRunAnimator;

    private PlayerStatsController _playerStatsController;
    private PlayerStats _playerStats;
    private Rigidbody2D _rb;
    private Vector3 _moveDir;

    private PCInputActions _inputActions;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStatsController = GetComponent<PlayerStatsController>();
        _playerStats = _playerStatsController.playerStats;

        // bind and enable input
        _inputActions = GetComponent<PlayerInputActions>().inputActions;
    }

    private void FixedUpdate()
    {
        /// player dead will turn to ghost, and move even faster!

        if (_playerStats.isMovementLocked)
        {
            // Idle
            if (_animator.GetBool("isMoving"))
            {
                _animator.SetBool("isMoving", false);
                _animator.SetFloat("moveX", 0f);
            }
            return;
        }

        _moveDir = _inputActions.Player.Move.ReadValue<Vector2>().normalized;

        bool isIdle = _moveDir == Vector3.zero;

        if (isIdle)
        {
            // Idle
            if (_animator.GetBool("isMoving"))
            {
                _animator.SetBool("isMoving", false);
                _animator.SetFloat("moveX", 0f);
            }
            
            // ghost run
            if (_ghostRunAnimator.isActiveAndEnabled && _ghostRunAnimator.GetBool("isGhostRunning"))
            {
                _ghostRunAnimator.SetBool("isGhostRunning", false);
                _ghostRunAnimator.SetFloat("moveX", 0f);
                _ghostRunAnimator.SetFloat("moveY", 0f);
            }
        }
        else
        {
            // is moving
            _rb.AddForce(_moveDir * _playerStatsController.GetCurrentSpeed());
            if (!_animator.GetBool("isMoving"))
                _animator.SetBool("isMoving", true);
            _animator.SetFloat("moveX", _moveDir.x);

            // ghost run
            if (_ghostRunAnimator.isActiveAndEnabled)
            {
                if (!_ghostRunAnimator.GetBool("isGhostRunning"))
                    _ghostRunAnimator.SetBool("isGhostRunning", true);
                var velocity = _rb.velocity.normalized;
                _ghostRunAnimator.SetFloat("moveX", velocity.x);
                _ghostRunAnimator.SetFloat("moveY", velocity.y);
            }
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
