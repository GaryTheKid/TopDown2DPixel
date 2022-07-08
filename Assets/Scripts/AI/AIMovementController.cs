/* Last Edition: 07/03/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The AI movement Controller that controls AI physical moves.
 * Last Edition:
 *   Just Created.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Utilities;

public class AIMovementController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ConvertedToEntityHolder convertedEntityHolder;

    private GridMap<GridNode> gridMap;
    private AIStatsController _StatsController;
    private AIStats _aiStats;
    //private Rigidbody2D _rb;
    //private Vector3 _moveDestination;
    private Entity entity;
    private EntityManager entityManager;

    //public List<Vector3> _wayPoints;
    [SerializeField] private int2 currentGridNodeStandingOn;

    private void Awake()
    {
        //_rb = GetComponent<Rigidbody2D>();
        _StatsController = GetComponent<AIStatsController>();
    }

    private void Start()
    {
        gridMap = PathfindingGridSetup.Instance.pathfindingGrid;
        _aiStats = _StatsController.aiStats;
        entity = convertedEntityHolder.GetEntity();
        entityManager = convertedEntityHolder.GetEntityManager();
    }

    private void Update()
    {
        UnitGridCollisionUpdate();
        FollowPath();
    }

    public void Move(Vector3 position)
    {
        MoveTo(position);
        //wayPoints.Add(position);
    }

    public void Halt()
    {
        entityManager.RemoveComponent<PathFindingParams>(entity);
        entityManager.SetComponentData(entity, new PathFollow { pathIndex = -1 });
    }

    public void Stop()
    {
        Halt();
        //ClearWayPoints();
        //GetCurrentWayPoint();
    }

    private void ClearWayPoints()
    {
        //wayPoints.Clear();
    }

    private void MoveTo(Vector3 endPosition)
    {
        // give move order
        float cellSize = gridMap.GetCellSize();

        gridMap.GetXY(endPosition + new Vector3(1, 1) * cellSize * +0.5f, out int endX, out int endY);

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
    }

    private void FollowPath()
    {
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

        // follow the path
        PathFollow pathFollow = entityManager.GetComponentData<PathFollow>(entity);
        DynamicBuffer<PathPosition> pathPositionBuffer = entityManager.GetBuffer<PathPosition>(entity);

        bool isIdle = pathFollow.pathIndex < 0;

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
            PathPosition pathPosition = pathPositionBuffer[pathFollow.pathIndex];

            float3 targetPosition = new float3(pathPosition.position.x, pathPosition.position.y, 0);
            float3 moveDir = math.normalizesafe(targetPosition - (float3)transform.position);

            transform.position += (Vector3)(moveDir * _StatsController.GetCurrentSpeed() * Time.deltaTime);

            if (!_animator.GetBool("isMoving"))
                _animator.SetBool("isMoving", true);
            _animator.SetFloat("moveX", moveDir.x);

            if (math.distance(transform.position, targetPosition) < 0.1f)
            {
                // Next Waypoint
                pathFollow.pathIndex--;
                entityManager.SetComponentData(entity, pathFollow);
            } 
        }
    }

    private void ValidateGridPosition(ref int x, ref int y)
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
    }
}
