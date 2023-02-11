using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCollider : MonoBehaviour
{
    [SerializeField] private WeaponCursor _weaponCursor;
    [SerializeField] private List<Collider2D> _enemyColliders;
    [SerializeField] private List<Collider2D> _bushColliders;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null 
            && collision.gameObject.name == "HitBox" 
            && (collision.gameObject.CompareTag("EnemyPlayer")
            || collision.gameObject.CompareTag("EnemyAI")))
        {
            _enemyColliders.Add(collision);
        }
        else if (collision.gameObject.CompareTag("Bush"))
        {
            _bushColliders.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null
            && collision.gameObject.name == "HitBox"
            && (collision.gameObject.CompareTag("EnemyPlayer")
            || collision.gameObject.CompareTag("EnemyAI")))
        {
            _enemyColliders.Remove(collision);
        }
        else if (collision.gameObject.CompareTag("Bush"))
        {
            _bushColliders.Remove(collision);
        }
    }

    private void Update()
    {
        if (_enemyColliders.Count > 0 && _bushColliders.Count <= 0)
        {
            _weaponCursor.ShowCursorDetectedEnemy();
        }
        else
        {
            _weaponCursor.HideCursorDetectedEnemy();
        }
    }
}
