using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackActionDetection : MonoBehaviour
{
    [SerializeField] private AIBrain _aiBrain;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _aiBrain.SetAttackTarget(collision.transform.parent);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _aiBrain.ResetAttackTarget();
    }
}
