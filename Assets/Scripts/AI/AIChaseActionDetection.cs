using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseActionDetection : MonoBehaviour
{
    [SerializeField] private AIBrain _aiBrain;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _aiBrain.SetChaseTarget(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _aiBrain.ResetChaseTarget();
    }
}
