using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Utilities;
using MoreMountains.Feedbacks;

public class WeaponCursor : MonoBehaviour
{
    [SerializeField] protected Image[] _visuals;
    [SerializeField] protected Transform _mouseCollider;
    [SerializeField] protected MMF_Player _cursorAttackFeedBack;
    [SerializeField] protected MMF_Player _cursorChargeFeedBack;

    protected bool hasShownDetectedEnemy;

    protected void OnEnable()
    {
        Cursor.visible = false;
    }

    protected void OnDisable()
    {
        Cursor.visible = true;
    }

    protected void Update()
    {
        // update cursor position
        transform.position = Common.GetMouseScreenPosition();
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

    public void FireAttackFeedbacks()
    {
        if (_cursorChargeFeedBack != null)
        {
            _cursorChargeFeedBack.StopFeedbacks();
        }
        
        _cursorAttackFeedBack.PlayFeedbacks();
    }

    public void FireChargeFeedbacks()
    {
        _cursorChargeFeedBack.PlayFeedbacks();
    }
}
