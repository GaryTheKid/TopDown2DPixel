/* Last Edition: 07/05/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * reference: Codemonkey
 * 
 * Description: 
 *   The pathfinding script utilizing A* algorithm to find the path between A and B.
 *   With Job system and burst complie enabled, it runs 1000 times faster!
 * Last Edition:
 *   convert from MonoBehavior to ComponentSystem.
 */

# region Legacy

/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Jobs;
using Unity.Entities;

public class PathFinding : ComponentSystem
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    protected override void OnUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
            return;

        int gridWidth = PathfindingGridSetup.Instance.pathfindingGrid.GetWidth();
        int gridHeight = PathfindingGridSetup.Instance.pathfindingGrid.GetHeight();
        int2 gridSize = new int2(gridWidth, gridHeight);

        List<FindPathJob> findPathJobList = new List<FindPathJob>();
        NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);

        Entities.ForEach((Entity entity, DynamicBuffer<PathPosition> pathPositionBuffer, ref PathFindingParams pathfindingParams) =>
        {
            FindPathJob findPathJob = new FindPathJob
            {
                gridSize = gridSize,
                pathNodeArray = GetPathNodeArray(),
                startPosition = pathfindingParams.startPosition,
                endPosition = pathfindingParams.endPosition,
                entity = entity,
                pathFollowComponentDataFromEntity = GetComponentDataFromEntity<PathFollow>(),
            };
            findPathJobList.Add(findPathJob);
            jobHandleList.Add(findPathJob.Schedule());

            PostUpdateCommands.RemoveComponent<PathFindingParams>(entity);
        });

        JobHandle.CompleteAll(jobHandleList);

        foreach (FindPathJob findPathJob in findPathJobList)
        {
            new SetBufferPathJob
            {
                entity = findPathJob.entity,
                gridSize = findPathJob.gridSize,
                pathNodeArray = findPathJob.pathNodeArray,
                pathfindingParamsComponentDataFromEntity = GetComponentDataFromEntity<PathFindingParams>(),
                pathFollowComponentDataFromEntity = GetComponentDataFromEntity<PathFollow>(),
                pathPositionBufferFromEntity = GetBufferFromEntity<PathPosition>(),
            }.Run();
        }
    }

    private NativeArray<PathNode> GetPathNodeArray()
    {
        GridMap<GridNode> grid = PathfindingGridSetup.Instance.pathfindingGrid;

        int2 gridSize = new int2(grid.GetWidth(), grid.GetHeight());
        NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.TempJob);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = new PathNode();
                pathNode.x = x;
                pathNode.y = y;
                pathNode.index = CalculateIndex(x, y, gridSize.x);

                pathNode.gCost = int.MaxValue;

                pathNode.isWalkable = grid.GetGridObject(x, y).IsWalkable();
                pathNode.cameFromNodeIndex = -1;

                pathNodeArray[pathNode.index] = pathNode;
            }
        }

        return pathNodeArray;
    }

    [BurstCompile]
    private struct SetBufferPathJob : IJob
    {
        public int2 gridSize;

        [DeallocateOnJobCompletion]
        public NativeArray<PathNode> pathNodeArray;

        public Entity entity;

        public ComponentDataFromEntity<PathFindingParams> pathfindingParamsComponentDataFromEntity;
        public ComponentDataFromEntity<PathFollow> pathFollowComponentDataFromEntity;
        public BufferFromEntity<PathPosition> pathPositionBufferFromEntity;

        public void Execute()
        {
            DynamicBuffer<PathPosition> pathPositionBuffer = pathPositionBufferFromEntity[entity];
            pathPositionBuffer.Clear();

            PathFindingParams pathFindingParams = pathfindingParamsComponentDataFromEntity[entity];
            int endNodeIndex = CalculateIndex(pathFindingParams.endPosition.x, pathFindingParams.endPosition.y, gridSize.x);
            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.cameFromNodeIndex == -1)
            {
                // Cannot find a path
                pathFollowComponentDataFromEntity[entity] = new PathFollow { pathIndex = -1 };
            }
            else
            {
                // Found a path
                CalculatePath(pathNodeArray, endNode, pathPositionBuffer);

                pathFollowComponentDataFromEntity[entity] = new PathFollow { pathIndex = pathPositionBuffer.Length - 1 };
            }
        }
    }

    [BurstCompile]
    private struct FindPathJob : IJob
    {
        public int2 gridSize;
        public NativeArray<PathNode> pathNodeArray;

        public int2 startPosition;
        public int2 endPosition;

        public Entity entity;
        [NativeDisableContainerSafetyRestriction]
        public ComponentDataFromEntity<PathFollow> pathFollowComponentDataFromEntity;

        //public BufferFromEntity<PathPosition> pathPositionBuffer;

        public void Execute()
        {
            for (int i = 0; i < pathNodeArray.Length; i++)
            {
                PathNode pathNode = pathNodeArray[i];
                pathNode.hCost = CalculateDistanceCost(new int2(pathNode.x, pathNode.y), endPosition);
                pathNode.cameFromNodeIndex = -1;

                pathNodeArray[i] = pathNode;
            }

            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
            neighbourOffsetArray[0] = new int2(-1, 0);    // Left
            neighbourOffsetArray[1] = new int2(+1, 0);    // Right
            neighbourOffsetArray[2] = new int2(0, +1);    // Up
            neighbourOffsetArray[3] = new int2(0, -1);    // Down
            neighbourOffsetArray[4] = new int2(+1, +1);   // Right Up
            neighbourOffsetArray[5] = new int2(-1, +1);   // Left Up
            neighbourOffsetArray[6] = new int2(+1, -1);   // Right Down
            neighbourOffsetArray[7] = new int2(-1, -1);   // Left Down

            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

            PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, gridSize.x)];
            startNode.gCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            openList.Add(startNode.index);

            while (openList.Length > 0)
            {
                int currentNodeIndex = GetlowestCostFNodeIndex(openList, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex];

                if (currentNodeIndex == endNodeIndex)
                {
                    // reached the destination
                    break;
                }

                // remove the current node from open list
                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentNodeIndex)
                    {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighbourOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if (!IsPositionInsideGrid(neighbourPosition, gridSize))
                    {
                        // Neighbour not valid position
                        continue;
                    }

                    int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);

                    if (closedList.Contains(neighbourNodeIndex))
                    {
                        // Already searched this node
                        continue;
                    }

                    PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                    if (!neighbourNode.isWalkable)
                    {
                        // not walkable
                        continue;
                    }

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

                    // Astar Algo
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNodeIndex = currentNodeIndex;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.CalculateFCost();
                        pathNodeArray[neighbourNodeIndex] = neighbourNode;

                        if (!openList.Contains(neighbourNode.index))
                        {
                            openList.Add(neighbourNode.index);
                        }
                    }

                }
            }

            *//*pathPositionBuffer.Clear();

            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.cameFromNodeIndex == -1)
            {
                // Didn't find a path!
                //Debug.Log("Didn't find a path");
                //pathFollowComponentDataFromEntity[entity] = new PathFollow { pathIndex = -1 };
            }
            else
            {
                // Found a path
                //CalculatePath(pathNodeArray, endNode, pathPositionBuffer);
                //pathFollowComponentDataFromEntity[entity] = new PathFollow { pathIndex = pathPositionBuffer.Length - 1 };
            }*//*

            neighbourOffsetArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
        }
    }

    private static void CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode, DynamicBuffer<PathPosition> pathPositionBuffer)
    {
        if (endNode.cameFromNodeIndex == -1)
        {
            // Could't find a path
        }
        else
        {
            // Found a path
            pathPositionBuffer.Add(new PathPosition { position = new int2(endNode.x, endNode.y) });

            PathNode currentNode = endNode;
            while (currentNode.cameFromNodeIndex != -1)
            {
                PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                pathPositionBuffer.Add(new PathPosition { position = new int2(cameFromNode.x, cameFromNode.y) });
                currentNode = cameFromNode;
            }
        }
    }

    private static NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
    {
        if (endNode.cameFromNodeIndex == -1)
        {
            // Could't find a path
            return new NativeList<int2>(Allocator.Temp);
        }
        else
        {
            // Found a path
            NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
            path.Add(new int2(endNode.x, endNode.y));

            PathNode currentNode = endNode;
            while (currentNode.cameFromNodeIndex != -1)
            {
                PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                path.Add(new int2(cameFromNode.x, cameFromNode.y));
                currentNode = cameFromNode;
            }

            return path;
        }
    }

    private static bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize)
    {
        return
            gridPosition.x >= 0 &&
            gridPosition.y >= 0 &&
            gridPosition.x < gridSize.x &&
            gridPosition.y < gridSize.y;
    }

    private static int CalculateIndex(int x, int y, int gridWidth, int offset=0)
    {
        return (x + y * gridWidth) + offset;
    }

    private static int CalculateDistanceCost(int2 aPosition, int2 bPosition)
    {
        int xDistance = math.abs(aPosition.x - bPosition.x);
        int yDistance = math.abs(aPosition.y - bPosition.y);
        int remaining = math.abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private static int GetlowestCostFNodeIndex(NativeArray<int> openList, NativeArray<PathNode> pathNodeArray)
    {
        PathNode lowestCostPathNode = pathNodeArray[openList[0]];
        for (int i = 1; i < openList.Length; i++)
        {
            PathNode testPathNode = pathNodeArray[openList[i]];
            if (testPathNode.fCost < lowestCostPathNode.fCost)
            {
                lowestCostPathNode = testPathNode;
            }
        }
        return lowestCostPathNode.index;
    }

    private struct PathNode
    {
        public int x;
        public int y;

        public int index;

        public int gCost;
        public int hCost;
        public int fCost;

        public bool isWalkable;

        public int cameFromNodeIndex;

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
        public void SetIsWalkable(bool isWalkable)
        {
            this.isWalkable = isWalkable;
        }
    }
}*/
#endregion