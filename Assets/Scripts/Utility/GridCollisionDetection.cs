using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCollisionDetection : MonoBehaviour
{
    [SerializeField] private TestGrid testGrid;

    private void OnTriggerStay2D(Collider2D collision)
    {
        testGrid.grid.SetGridObject(transform.position, true);
    }
}
