/* Last Edition: 07/09/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * Reference: CodeMonkey
 * 
 * Description: 
 *  A grid system for pathfinding, building, etc.
 *   
 * Last Edition:
 *  Grid collision detection from single box 2d overlay to area all.
 */

using System;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using CodeMonkey.Utils;
using Unity.Collections;
using Unity.Entities;

public class GridMap<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridArray;

    public GridMap(int width, int height, float cellSize, Func<GridMap<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = false;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 5, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
            };
        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }

    public void SetGridObject(int x, int y, TGridObject value) 
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public void GenerateCollisionMap()
    {
        int detectionLayer = LayerMask.NameToLayer("Everything");
        detectionLayer = ~LayerMask.GetMask("EnemyAI", "AIChaseRadius", "AIAttackRadius", "Enemy", "Character");

        var offset = new Vector3(cellSize * 0.5f, cellSize * 0.5f);
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Collider2D hitCollider = Physics2D.OverlapBox(GetWorldPosition(x, y) + offset, new Vector2(cellSize * 0.8f, cellSize * 0.8f), 0f, detectionLayer);
                if (hitCollider != null)
                {
                    PathfindingGridSetup.Instance.pathfindingGrid.GetGridObject(x, y).SetIsWalkable(false);
                }
            }
        }

        /*// calculate all nodes' collision
        NativeArray<bool> walkableResults = new NativeArray<bool>(width * height, Allocator.TempJob);
        GridMapCollisionDetectionJob collisionDetectionJob = new GridMapCollisionDetectionJob
        {
            width = width,
            height = height,
            cellSize = cellSize,
            originPosition = originPosition,
            layerMask = LayerMask.NameToLayer("Everything"),
            result = walkableResults

        };
        JobHandle jobHandle = collisionDetectionJob.Schedule();

        // wait job complete
        jobHandle.Complete();

        // inject calculated results to the gridmap
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                PathfindingGridSetup.Instance.pathfindingGrid.GetGridObject(x, y).SetIsWalkable(walkableResults[CalculateIndex(x, y, width)]);
            }
        }

        // dispose
        walkableResults.Dispose();*/
    }

    private struct GridMapCollisionDetectionJob : IJob
    {
        public int width;
        public int height;
        public float cellSize;
        public Vector3 originPosition;
        public LayerMask layerMask;
        public NativeArray<bool> result;

        public void Execute()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Collider2D hitCollider = Physics2D.OverlapBox(new Vector3(x, y) * cellSize + originPosition, new Vector2(cellSize / 1.5f, cellSize / 1.5f), 0f, layerMask);
                    if (hitCollider != null)
                    {
                        result[CalculateIndex(x, y, width)] = false;
                    }
                }
            }
        }
    }

    private static int CalculateIndex(int x, int y, int gridWidth)
    {
        return x + y * gridWidth;
    }
}
