using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastIndicatorController : MonoBehaviour
{
    private const float INVALID_CAST_TEXT_DISPLAY_TIME = 3f;

    public Weapon.CastIndicatorType indicatorType;
    public LayerMask invalidCastLayerMask;
    public bool isCastValid;
    [SerializeField] private GameObject invalidCastText;
    [SerializeField] private Transform indicator_point;
    [SerializeField] private Transform indicator_circle;
    [SerializeField] private Transform indicator_line;

    private IEnumerator _co_showInvalidCastText;

    public void ActivateIndicator()
    {
        switch (indicatorType)
        {
            case Weapon.CastIndicatorType.Point:
                indicator_point.gameObject.SetActive(true);
                break;
            case Weapon.CastIndicatorType.Circle:
                indicator_circle.gameObject.SetActive(true);
                break;
            case Weapon.CastIndicatorType.Line:
                indicator_line.gameObject.SetActive(true);
                break;
        }
    }

    public void DeactivateIndicator()
    {
        indicator_point.position = transform.position;
        indicator_circle.position = transform.position;
        indicator_line.position = transform.position;
        indicator_point.gameObject.SetActive(false);
        indicator_circle.gameObject.SetActive(false);
        indicator_line.gameObject.SetActive(false);
    }

    public void SetIndicator_Point()
    {

    }

    public void SetIndicator_Line(float width, float range)
    {
        indicator_line.localScale = new Vector3(width, range, 1f);
    }

    public void SetIndicator_Circle()
    {
        
    }

    public void PositionIndicator(Vector2 pos)
    {
        switch (indicatorType)
        {
            case Weapon.CastIndicatorType.Point:
                indicator_point.position = pos;
                break;
            case Weapon.CastIndicatorType.Circle:
                indicator_circle.position = pos;
                break;
            case Weapon.CastIndicatorType.Line:
                isCastValid = true;
                Vector2 origin = transform.position;
                Vector2 aimDir = (pos - origin).normalized;
                var angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
                var eular = new Vector3(0f, 0f, angle);
                indicator_line.eulerAngles = eular;
                break;
        }
    }

    public void DisableInvalidCastText()
    {
        if (_co_showInvalidCastText != null)
        {
            StopCoroutine(_co_showInvalidCastText);
            _co_showInvalidCastText = null;
        }

        invalidCastText.SetActive(false);
    }

    public void ShowInvalidCastText()
    {
        if (_co_showInvalidCastText != null)
        {
            StopCoroutine(_co_showInvalidCastText);
            _co_showInvalidCastText = null;
        }
        _co_showInvalidCastText = Co_ShowInvalidCastText();
        StartCoroutine(_co_showInvalidCastText);
    }
    IEnumerator Co_ShowInvalidCastText()
    {
        invalidCastText.SetActive(true);
        yield return new WaitForSecondsRealtime(INVALID_CAST_TEXT_DISPLAY_TIME);
        invalidCastText.SetActive(false);

        _co_showInvalidCastText = null;
    }
}
