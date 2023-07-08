using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerMovementController : MonoBehaviour
{
    private const float MIN_MOVE_THRESHOLD = 0.8f;

    [SerializeField] private float _dashAmount;
    [SerializeField] private LayerMask _dashLayerMask;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _ghostRunAnimator;

    private PhotonView _PV;
    private PlayerStatsController _playerStatsController;
    private PlayerStats _playerStats;
    private Rigidbody2D _rb;
    private Vector3 _moveDir;

    private PCInputActions _inputActions;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody2D>();
        _playerStatsController = GetComponent<PlayerStatsController>();
        _playerStats = _playerStatsController.playerStats;

        // bind and enable input
        _inputActions = GetComponent<PlayerInputActions>().inputActions;
    }

    private void FixedUpdate()
    {
        // check if self
        if (_PV.IsMine)
        {
            /// player dead will turn to ghost, and move even faster!

            if (_playerStats.isMovementLocked || _playerStats.isTyping)
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
            if (_moveDir == Vector3.zero)
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
        else
        {
            var speed = _rb.velocity.x;
            if (Mathf.Abs(speed) < MIN_MOVE_THRESHOLD && Mathf.Abs(speed) >= 0)
            {
                _animator.SetBool("isMoving", false);
                _animator.SetFloat("moveX", 0f);
            }
            else
            {
                _animator.SetBool("isMoving", true);
                _animator.SetFloat("moveX", speed);
            }
        }
    }

    public void SetAvatarAnimation(Animator animator, Animator ghostRunAnimator)
    {
        _animator = animator;
        _ghostRunAnimator = ghostRunAnimator;
    }
}
