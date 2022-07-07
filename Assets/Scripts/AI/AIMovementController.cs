/* Last Edition: 07/03/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The AI movement Controller that controls AI physical moves.
 * Last Edition:
 *   Just Created.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private AIStatsController _StatsController;
    private AIStats _aiStats;
    private Rigidbody2D _rb;
    private Vector3 _moveDestination;
    public List<Vector3> _wayPoints;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _StatsController = GetComponent<AIStatsController>();
    }

    private void Start()
    {
        _aiStats = _StatsController.aiStats;
        _wayPoints = new List<Vector3>();
    }

    private void FixedUpdate()
    {
        /// player dead will turn to ghost, and move even faster!

        if (_aiStats.isMovementLocked)
        {
            // Idle
            if (_animator.GetBool("isMoving"))
            {
                _animator.SetBool("isMoving", false);
                _animator.SetFloat("moveX", 0f);
            }
            return;
        }

        // set move direction
        if (_wayPoints.Count != 0)
        {
            _moveDestination = _wayPoints[0];
        }
        else
        {
            _moveDestination = Vector3.zero;
        }
        

        bool isIdle = _moveDestination == Vector3.zero;

        if (isIdle)
        {
            // Idle
            if (_animator.GetBool("isMoving"))
            {
                _animator.SetBool("isMoving", false);
                _animator.SetFloat("moveX", 0f);
            }

        }
        else
        {
            // is moving
            _rb.velocity = _moveDestination * _StatsController.GetCurrentSpeed() * Time.fixedDeltaTime;
            if (!_animator.GetBool("isMoving"))
                _animator.SetBool("isMoving", true);
            _animator.SetFloat("moveX", _moveDestination.x);
        }
    }

    public void MoveTo(Vector3 pos)
    {
        _wayPoints.Add(pos);
    }
}
