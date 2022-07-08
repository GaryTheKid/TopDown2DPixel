/* Last Edition: 07/07/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * reference: Codemonkey
 * 
 * Description: 
 *   Setup for the global grid system.
 *   step by step.
 * Last Edition:
 *   Just created.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Utilities;

public class PathfindingGridSetup : MonoBehaviour
{
    public static PathfindingGridSetup Instance { private set; get; }

    [SerializeField] private Tilemap tilemap;

    public GridMap<GridNode> pathfindingGrid;
    
    private void Awake()
    {
        Instance = this;
        var gridSize = tilemap.cellBounds.size;
        pathfindingGrid = new GridMap<GridNode>(gridSize.x, gridSize.y, 1f, (GridMap<GridNode> grid, int x, int y) => new GridNode(grid, x, y));
    }

    // Start is called before the first frame update
    void Start()
    {
        pathfindingGrid.GenerateCollisionMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.rightButton.ReadValue() != 0f)
        {
            Vector3 mousePosition = Common.GetMouseWorldPosition() + (new Vector3(+1, +1) * pathfindingGrid.GetCellSize() * 0.5f);
            GridNode gridNode = pathfindingGrid.GetGridObject(mousePosition);
            if(gridNode != null)
            {
                gridNode.SetIsWalkable(!gridNode.IsWalkable());
            }
        }
    }
}

public class GridNode
{
    private GridMap<GridNode> grid;
    private int x;
    private int y;

    private bool isWalkable;

    public GridNode(GridMap<GridNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString()
    {
        return isWalkable.ToString();
    }
}
