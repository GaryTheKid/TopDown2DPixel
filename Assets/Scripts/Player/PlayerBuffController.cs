using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBuffController : MonoBehaviour
{
    public UnityEvent OnReceiveHealingEffect;

    public void ReceiveHealingEffect()
    {
        Debug.Log("HP +10");
    }
}
