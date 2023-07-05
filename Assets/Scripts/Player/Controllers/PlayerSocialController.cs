using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class PlayerSocialController : MonoBehaviour
{
    [SerializeField] private UI_SocialWheelMenu _wheelMenu;
    [SerializeField] private List<GameObject> _emojis;
    [SerializeField] private Animator _emojiBubble;

    private PCInputActions _inputActions;
    private int _choiceIndex;
    private Vector3 initPos;
    private Vector3 currentPos;

    private void Start()
    {
        // initialization
        _inputActions = GetComponent<PlayerInputActions>().inputActions;
        LoadSocialInputActions();
        _choiceIndex = -1;
    }

    private void Update()
    {
        // check if the wheel is active
        if (!_wheelMenu.gameObject.activeInHierarchy)
            return;

        // select choice by angle (two-points)
        currentPos = Common.GetMouseScreenPosition();
        _choiceIndex = _wheelMenu.SelectChoiceByAngle(currentPos, Common.GetEulerAngleBetweenPointsClockWise(initPos, Common.GetMouseScreenPosition()));
    }

    private void LoadSocialInputActions()
    {
        _inputActions.Player.HoldSocialWheelMenu.performed += Hold;
        _inputActions.Player.ReleaseEmojiWheelMenu.performed += Release;
    }

    private void EnableEmoji()
    {
        if (_emojiBubble.GetCurrentAnimatorStateInfo(0).IsName("ShowEmoji") || _choiceIndex == -1)
            return;

        for (int i = 0; i < _emojis.Count; i++)
        {
            if (i == _choiceIndex)
                _emojis[i].SetActive(true);
            else
                _emojis[i].SetActive(false);
        }
        _emojiBubble.SetTrigger("ShowEmoji");
    }

    private void Hold(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // set position to mouse pos
            _wheelMenu.transform.position = Common.GetMouseScreenPosition();
            initPos = _wheelMenu.transform.position;

            // enable wheel menu
            _wheelMenu.gameObject.SetActive(true);
        }
    }

    private void Release(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // send social interaction based on the index selected
            EnableEmoji();

            // reset choice index
            _choiceIndex = -1;

            // disable wheel menu
            _wheelMenu.HideAllChoices();
            _wheelMenu.gameObject.SetActive(false);
        }
    }
}