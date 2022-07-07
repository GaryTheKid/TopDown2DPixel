/* Last Edition: 07/05/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   To test the gridmap system.
 * Last Edition:
 *   Just Created.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class TestGrid : MonoBehaviour
{
    [SerializeField] private Transform collisionDetector;
    public GridMap<bool> grid;

    private void Start()
    {
        grid = new GridMap<bool>(30, 20, 1f, (GridMap<bool> grid, int x, int y) => true);
        //StartCoroutine(Co_TraverseGridCollision());
    }

    IEnumerator Co_TraverseGridCollision()
    {
        yield return new WaitForSeconds(0.5f);

        // move the detector to traverse the entire grid
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.ReadValue() != 0f)
        {
            grid.SetGridObject(Common.GetMouseWorldPosition(), true);
        }

        if (Mouse.current.rightButton.ReadValue() != 0f)
        {
            print(grid.GetGridObject(Common.GetMouseWorldPosition()));
        }
    }

    /*public void TraverseGridCollision(Transform collisionDetector, int x, int y)
    {
        if (collisionDetector.gameObject.layer != LayerMask.NameToLayer("GridCollisionDetection"))
        {
            Debug.LogError("Collision Detector must be in the 'GridCollisionDetection' layer !");
            return;
        }

        BoxCollider2D collider = collisionDetector.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            Debug.LogError("Collision Detector must contain a 'BoxCollider2D' !");
            return;
        }

        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            collider.size = new Vector2(cellSize, cellSize);
            collisionDetector.position = GetWorldPosition(x, y) + new Vector3(cellSize / 2f, cellSize / 2f);
        }
    }*/
}
