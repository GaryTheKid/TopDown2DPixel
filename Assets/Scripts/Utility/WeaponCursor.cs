using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Utilities;

public class WeaponCursor : MonoBehaviour
{
    [SerializeField] private Image[] _visuals;
    [SerializeField] private Transform _mouseCollider;

    private bool hasShownDetectedEnemy;

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        // update cursor position
        transform.position = Mouse.current.position.ReadValue();
        _mouseCollider.position = Common.GetMouseWorldPosition();
    }

    public void ShowCursorDetectedEnemy()
    {
        if (!hasShownDetectedEnemy)
        {
            foreach (var visual in _visuals)
            {
                visual.color = Color.red;
            }
            hasShownDetectedEnemy = true;
        }
    }

    public void HideCursorDetectedEnemy()
    {
        if (hasShownDetectedEnemy)
        {
            foreach (var visual in _visuals)
            {
                visual.color = Color.white;
            }
            hasShownDetectedEnemy = false;
        }
    }
}
