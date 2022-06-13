using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utilities;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] private Transform _v1;
    [SerializeField] private Transform _v2;
    [Tooltip("By default, if this value is 0. If not, it will override the game manager spawn density for this spawn area.")]
    public int overrideSpawnDensity;

    // turn this off when build, only works on Editor
    /*private void OnDrawGizmos()
    {
        // draw area
        (Vector3 center, Vector3 size) = ConvertTwoVertsToCenterSize();
        Color col = Color.blue;
        col.a = 0.5f;
        Gizmos.color = col;
        Gizmos.DrawCube(center, size);

        // draw text
        Vector3 textCenter = new Vector3(center.x - 1.5f, center.y + 0.5f);
        Handles.Label(textCenter, "Spawn Area");
    }*/

    public float GetAreaSize()
    {
        return Math.GetSizeFromTwoVerts(_v1.position, _v2.position);
    }

    public Vector3 GetRandomPointFromArea()
    {
        var randX = Random.Range(_v1.position.x, _v2.position.x);
        var randY = Random.Range(_v1.position.y, _v2.position.y);
        return new Vector3(randX, randY, 0f);
    }

    private (Vector3, Vector3) ConvertTwoVertsToCenterSize()
    {
        Vector3 center = (_v1.position + _v2.position) / 2f;
        Vector3 size = (_v1.position - _v2.position);
        return (center, size);
    }
}
