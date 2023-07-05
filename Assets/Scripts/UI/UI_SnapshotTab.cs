using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_SnapshotTab : MonoBehaviour
{
    [SerializeField] private GlobalInputActions _globalInputActions;
    [SerializeField] private GameObject tab;
    private PCInputActions _inputActions;

    private void Awake()
    {
        _inputActions = _globalInputActions.inputActions;
    }

    private void OnEnable()
    {
        _inputActions.Global.SnapshotTabActivation.started += ActivateTab;
        _inputActions.Global.SnapshotTabActivation.canceled += DeactivateTab;
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Global.SnapshotTabActivation.started -= ActivateTab;
        _inputActions.Global.SnapshotTabActivation.canceled -= DeactivateTab;
        _inputActions.Disable();
    }

    private void ActivateTab(InputAction.CallbackContext context)
    {
        tab.SetActive(true);
    }

    private void DeactivateTab(InputAction.CallbackContext context)
    {
        tab.SetActive(false);
    }
}
