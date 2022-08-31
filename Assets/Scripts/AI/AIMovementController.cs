/* Last Edition: 07/23/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The AI movement Controller that controls AI physical moves.
 * Last Edition:
 *   Add move animation handling
 */
using UnityEngine;
using Photon.Pun;
using Pathfinding;

public class AIMovementController : MonoBehaviour
{
    private const float CONSIDER_IDLE_SPEED_THRESHOLD = 0.01f;

    [SerializeField] private Animator _animator;
    public Transform _moveTarget;

    private PhotonView _PV;
    private AIStatsController _StatsController;
    private AIStats _aiStats;
    private AIDestinationSetter _aiDestinationSetter;
    private Vector3 _prevPos;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _StatsController = GetComponent<AIStatsController>();
        _aiDestinationSetter = GetComponent<AIDestinationSetter>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _moveTarget.parent = transform.root.parent;
        _aiStats = _StatsController.aiStats;
        _aiDestinationSetter.target = _moveTarget;
        _prevPos = transform.position;
    }

    private void Update()
    {
        if (_aiStats.isDead)
            return;

        HandleMoveAnimation();
    }

    public void MoveTo(Vector2 pos)
    {
        _moveTarget.position = pos;
        //AI_NetWork.Move(_PV, pos);
    }

    public void Chase(Transform target)
    {
        _aiDestinationSetter.target = target;
    }

    public void StopChasing()
    {
        _moveTarget.position = transform.position;
        _aiDestinationSetter.target = _moveTarget;
        _animator.SetBool("isMoving", false);
        //AI_NetWork.Halt(_PV);
    }

    public void Halt()
    {
        _moveTarget.position = transform.position;
        _aiDestinationSetter.target = _moveTarget;
        _animator.SetBool("isMoving", false);
        //AI_NetWork.Halt(_PV);
    }
        
    public void Stop()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        Halt();
        //ClearWayPoints();
        //GetCurrentWayPoint();
    }

    public void Respawn()
    {
        _aiDestinationSetter.target = _moveTarget;
        _aiDestinationSetter.target.position = transform.position;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void HandleMoveAnimation()
    {
        var velocity = transform.position - _prevPos;

        if (velocity.magnitude <= CONSIDER_IDLE_SPEED_THRESHOLD && _animator.GetBool("isMoving"))
        {
            _animator.SetBool("isMoving", false);
            _animator.SetFloat("moveX", 0f);
        }

        if (velocity.magnitude > CONSIDER_IDLE_SPEED_THRESHOLD)
        {
            if (!_animator.GetBool("isMoving"))
            {
                _animator.SetBool("isMoving", true);
            }

            // check move direction
            if (velocity.x >= 0 && _animator.GetFloat("moveX") != 1f)
            {
                _animator.SetFloat("moveX", 1f);
            }
            else if(velocity.x < 0 && _animator.GetFloat("moveX") != -1f)
            {
                _animator.SetFloat("moveX", -1f);
            }
        }

        _prevPos = transform.position;
    }

    #region Legacy
    /*//private const float TARGET_REACH_MIN_DISTANCE = 0.4f;
    //private const float CONSIDER_IDLE_SPEED_THRESHOLD = 0.5f;

    //private Rigidbody2D _rb;
    //private GridMap<GridNode> gridMap;
    //public Entity entity;
    //public EntityManager entityManager;

    //public List<Vector3> _wayPoints;
    //[SerializeField] private ConvertedToEntityHolder convertedEntityHolder;
    //[SerializeField] private int2 currentGridNodeStandingOn;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _PV = GetComponent<PhotonView>();
        _StatsController = GetComponent<AIStatsController>();
    }

    private void Start()
    {
        gridMap = PathfindingGridSetup.Instance.pathfindingGrid;
        _aiStats = _StatsController.aiStats;
    }

    *//*private void FixedUpdate()
    {
        if (_aiStats.isDead)
            return;

        //UnitGridCollisionUpdate();
        //FollowPath();
    }*//*

    private void ClearWayPoints()
    {
        //wayPoints.Clear();
    }

    *//*public void MoveTo(Vector2 endPosition)
    {
        float cellSize = gridMap.GetCellSize();

        gridMap.GetXY(endPosition + new Vector2(1, 1) * cellSize * +0.5f, out int endX, out int endY);

        ValidateGridPosition(ref endX, ref endY);

        gridMap.GetXY(transform.position + new Vector3(1, 1, 0) * cellSize * +0.5f, out int startX, out int startY);

        ValidateGridPosition(ref startX, ref startY);

        // Add Pathfinding Params
        try
        {
            entityManager.AddComponentData(entity, new PathFindingParams
            {
                startPosition = new int2(startX, startY),
                endPosition = new int2(endX, endY)
            });
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e);
        }
    }*/

    /*private void FollowPath()
    {
        try
        {   
            // follow the path
            PathFollow pathFollow = entityManager.GetComponentData<PathFollow>(entity);
            DynamicBuffer<PathPosition> pathPositionBuffer = entityManager.GetBuffer<PathPosition>(entity);

            if (pathFollow.pathIndex >= 0)
            {
                // is moving
                PathPosition pathPosition = pathPositionBuffer[pathFollow.pathIndex];

                float3 targetPosition = new float3(pathPosition.position.x, pathPosition.position.y, 0);
                float3 moveDir = math.normalizesafe(targetPosition - (float3)transform.position);

                _rb.AddForce((Vector3)moveDir * _StatsController.GetCurrentSpeed());

                if (math.distance(transform.position, targetPosition) < TARGET_REACH_MIN_DISTANCE)
                {
                    // Next Waypoint
                    pathFollow.pathIndex--;
                    entityManager.SetComponentData(entity, pathFollow);
                }
            }
        }
        catch//(NullReferenceException e)
        {
            //Debug.Log(e);
        }
    }*/

    /*private void HandleMoveAnimation()
    {
        if (_rb.velocity.magnitude < CONSIDER_IDLE_SPEED_THRESHOLD && _animator.GetBool("isMoving"))
        {
            _animator.SetBool("isMoving", false);
            _animator.SetFloat("moveX", 0f);
        }

        if (_rb.velocity.magnitude >= CONSIDER_IDLE_SPEED_THRESHOLD)
        {
            if (!_animator.GetBool("isMoving"))
            {
                _animator.SetBool("isMoving", true);
            }

            // check move direction
            if (_rb.velocity.x >= 0 && _animator.GetFloat("moveX") != 1f)
            {
                _animator.SetFloat("moveX", 1f);
            }
            else if(_rb.velocity.x < 0 && _animator.GetFloat("moveX") != -1f)
            {
                _animator.SetFloat("moveX", -1f);
            }
        }
    }*/

    /*private void ValidateGridPosition(ref int x, ref int y)
    {
        x = math.clamp(x, 0, gridMap.GetWidth() - 1);
        y = math.clamp(y, 0, gridMap.GetHeight() - 1);
    }

    private void UnitGridCollisionUpdate()
    {
        gridMap.GetXY(transform.position, out int x, out int y);
        if (x != currentGridNodeStandingOn.x && y != currentGridNodeStandingOn.y)
        {
            gridMap.GetGridObject(currentGridNodeStandingOn.x, currentGridNodeStandingOn.y).SetIsWalkable(true);
            gridMap.GetGridObject(x, y).SetIsWalkable(false);
            currentGridNodeStandingOn = new int2(x, y);
        }
    }

    private void GetCurrentWayPoint()
    {
        gridMap.GetXY(transform.position + new Vector3(1, 1, 0) * PathfindingGridSetup.Instance.pathfindingGrid.GetCellSize() * +0.5f, out int startX, out int startY);
        ValidateGridPosition(ref startX, ref startY);
        //currentWayPoint = new Vector3(startX, startY);
    }*/
    #endregion

}
