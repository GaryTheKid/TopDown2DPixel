using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInputActions : MonoBehaviour
{
    public PCInputActions inputActions;

    // bind inputs
    private void Awake()
    {
        inputActions = new PCInputActions();

        // SnapshotTab
        inputActions.Global.SnapshotTabActivation.Enable();
    }
}
