using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class WeaponCursor : MonoBehaviour
{
    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();
    }
}
