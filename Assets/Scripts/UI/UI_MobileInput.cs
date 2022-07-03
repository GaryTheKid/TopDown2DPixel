using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_MobileInput : MonoBehaviour
{
    public RectTransform Pad_Left;
    public RectTransform Pad_Right;
    public RectTransform Joystick_Left;
    public RectTransform Joystick_Right;
    public RectTransform Button_Interaction;

    private Text DEBUG_TEXT;

    private PCInputActions _inputActions;

    private void OnValidate()
    {
#if UNITY_EDITOR_WIN || PLATFORM_STANDALONE_WIN
        gameObject.SetActive(false);
#endif
    }

    private void Awake()
    {
        _inputActions = GetComponentInParent<PlayerInputActions>().inputActions;
        _inputActions.Player.TouchStart.performed += LockLeftPadPos;
        //DEBUG_TEXT = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        //DEBUG_TEXT.text = _inputActions.Player.FireOrChargeWeapon.ReadValue<float>().ToString();
    }

    private void LockLeftPadPos(InputAction.CallbackContext context)
    {
        var touchPos = _inputActions.Player.TouchPos.ReadValue<Vector2>();
        if (touchPos.x <= Screen.width / 2.5f)
        {
            Pad_Left.position = touchPos;
        }
    }

    public void ActivateInteractionButton()
    {
        Pad_Right.gameObject.SetActive(false);
        Button_Interaction.gameObject.SetActive(true);
    }

    public void DeactivateInteractionButton()
    {
        Pad_Right.gameObject.SetActive(true);
        Button_Interaction.gameObject.SetActive(false);
    }
}