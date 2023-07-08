using UnityEngine;

public class GlobalInputActions : MonoBehaviour
{
    [SerializeField] private UI_SnapshotTab snapshotTab;
    [SerializeField] private UI_GlobalTypeChat globalTypeChat;
    private PCInputActions inputActions;

    // bind inputs
    private void Awake()
    {
        inputActions = new PCInputActions();
    }

    private void OnEnable()
    {
        inputActions.Global.SnapshotTabActivation.started += snapshotTab.ActivateTab;
        inputActions.Global.SnapshotTabActivation.canceled += snapshotTab.DeactivateTab;
        inputActions.Global.TypeChatActivation.performed += globalTypeChat.ActivateTab;
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Global.SnapshotTabActivation.started -= snapshotTab.ActivateTab;
        inputActions.Global.SnapshotTabActivation.canceled -= snapshotTab.DeactivateTab;
        inputActions.Global.TypeChatActivation.performed -= globalTypeChat.ActivateTab;
        inputActions.Disable();
    }
}
