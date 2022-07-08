/* Last Edition: 07/07/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * reference: Codemonkey
 * 
 * Description: 
 *   Blend ECS with Mono, giving the unit gameobject an order to move by adding a
 *   pathFindingParams component.
 *   step by step.
 * Last Edition:
 *   Just created.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Utilities;

public class UnitGameObject : MonoBehaviour
{
    [SerializeField] private ConvertedToEntityHolder convertedEntityHolder;

    //public List<Vector3> wayPoints;
    //public Vector3 currentWayPoint;

    private Entity entity;
    private EntityManager entityManager;

    private void Start()
    {
        entity = convertedEntityHolder.GetEntity();
        entityManager = convertedEntityHolder.GetEntityManager();
    }

    private void Update()
    {
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
        float cellSize = PathfindingGridSetup.Instance.pathfindingGrid.GetCellSize();

        PathfindingGridSetup.Instance.pathfindingGrid.GetXY(endPosition + new Vector3(1, 1) * cellSize * +0.5f, out int endX, out int endY);

        ValidateGridPosition(ref endX, ref endY);

        PathfindingGridSetup.Instance.pathfindingGrid.GetXY(transform.position + new Vector3(1, 1, 0) * cellSize * +0.5f, out int startX, out int startY);

        ValidateGridPosition(ref startX, ref startY);

        // Add Pathfinding Params
        entityManager.AddComponentData(entity, new PathFindingParams
        {
            startPosition = new int2(startX, startY),
            endPosition = new int2(endX, endY)
        });
    }

    private void FollowPath()
    {
        // follow the path
        PathFollow pathFollow = entityManager.GetComponentData<PathFollow>(entity);
        DynamicBuffer<PathPosition> pathPositionBuffer = entityManager.GetBuffer<PathPosition>(entity);

        if (pathFollow.pathIndex >= 0)
        {
            PathPosition pathPosition = pathPositionBuffer[pathFollow.pathIndex];

            float3 targetPosition = new float3(pathPosition.position.x, pathPosition.position.y, 0);
            float3 moveDir = math.normalizesafe(targetPosition - (float3)transform.position);
            float moveSpeed = 3f;

            transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);

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
        x = math.clamp(x, 0, PathfindingGridSetup.Instance.pathfindingGrid.GetWidth() - 1);
        y = math.clamp(y, 0, PathfindingGridSetup.Instance.pathfindingGrid.GetHeight() - 1);
    }

    private void GetCurrentWayPoint()
    {
        PathfindingGridSetup.Instance.pathfindingGrid.GetXY(transform.position + new Vector3(1, 1, 0) * PathfindingGridSetup.Instance.pathfindingGrid.GetCellSize() * +0.5f, out int startX, out int startY);
        ValidateGridPosition(ref startX, ref startY);
        //currentWayPoint = new Vector3(startX, startY);
    }
}
