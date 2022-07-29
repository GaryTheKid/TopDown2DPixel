using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastIndicatorController : MonoBehaviour
{
    public Weapon.CastIndicatorType indicatorType;
    [SerializeField] private Transform indicator_point;
    [SerializeField] private Transform indicator_circle;
    [SerializeField] private Transform indicator_line;

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
                Vector2 origin = transform.position;
                Vector2 aimDir = (pos - origin).normalized;
                var angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
                var eular = new Vector3(0f, 0f, angle);
                indicator_line.eulerAngles = eular;
                break;
        }
    }
}
