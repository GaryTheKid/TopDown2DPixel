using UnityEngine;
using UnityEngine.InputSystem;

public class UI_SnapshotTab : MonoBehaviour
{
    [SerializeField] private GameObject tab;

    public void ActivateTab(InputAction.CallbackContext context)
    {
        tab.SetActive(true);
    }

    public void DeactivateTab(InputAction.CallbackContext context)
    {
        tab.SetActive(false);
    }
}
