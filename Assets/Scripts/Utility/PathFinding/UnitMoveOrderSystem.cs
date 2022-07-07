using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using Utilities;

public class UnitMoveOrderSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Common.GetMouseWorldPosition();

            float cellSize = PathfindingGridSetup.Instance.pathfindingGrid.GetCellSize();

            PathfindingGridSetup.Instance.pathfindingGrid.GetXY(mousePosition + new Vector3(1, 1) * cellSize *  + 0.5f, out int endX, out int endY);

            Debug.Log(endX + " " + endY);

            ValidateGridPosition(ref endX, ref endY);

            Entities.ForEach((Entity entity, DynamicBuffer<PathPosition> pathPositionBuffer, ref Translation translation) =>
            {
                PathfindingGridSetup.Instance.pathfindingGrid.GetXY(translation.Value + new float3(1, 1, 0) * cellSize *  + 0.5f, out int startX, out int startY);

                ValidateGridPosition(ref startX, ref startY);

                // Add Pathfinding Params
                EntityManager.AddComponentData(entity, new PathFindingParams
                {
                    startPosition = new int2(startX, startY), 
                    endPosition = new int2(endX, endY)
                });
            });
        }
    }

    private void ValidateGridPosition(ref int x, ref int y)
    {
        x = math.clamp(x, 0, PathfindingGridSetup.Instance.pathfindingGrid.GetWidth() - 1);
        y = math.clamp(y, 0, PathfindingGridSetup.Instance.pathfindingGrid.GetHeight() - 1);
    }
}
