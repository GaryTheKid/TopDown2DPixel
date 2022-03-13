using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    public void ReceiveMeleeWeaponDamageEffect()
    {
        
    }

    public void ReceiveDamageEffect()
    {
        Debug.Log("Blink Red, Show -10 pop text...");
    }

    public void ReceiveHealingEffect()
    {
        Debug.Log("Blink Green, Show +10 pop text...");
    }
}
