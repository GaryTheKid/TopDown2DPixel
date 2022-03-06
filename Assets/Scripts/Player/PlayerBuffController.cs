using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBuffController : MonoBehaviour
{
    private PlayerStats stats;
    private PlayerEffectController effectController;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        effectController = GetComponent<PlayerEffectController>();
    }

    public void ReceiveHealing(int healingAmount)
    {
        Debug.Log("HP +" + healingAmount);
        stats.hp += healingAmount;
        effectController.ReceiveHealingEffect();
    }
}
