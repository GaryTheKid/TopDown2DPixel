using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Utilities;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerSocialController : MonoBehaviour, IOnEventCallback
{
    [Header("Emoji")]
    [SerializeField] private UI_SocialWheelMenu _wheelMenu;
    [SerializeField] private List<GameObject> _emojis;
    [SerializeField] private Animator _emojiBubble;

    private PhotonView _PV;
    private PCInputActions _inputActions;
    private int _choiceIndex;
    private Vector3 initPos;
    private Vector3 currentPos;

    [Header("Chat")]
    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private TextMeshProUGUI chatTextTemplate;
    [SerializeField] private Transform chatContent;

    private void Start()
    {
        // initialization
        _PV = GetComponent<PhotonView>();
        _inputActions = GetComponent<PlayerInputActions>().inputActions;
        LoadSocialInputActions();
        _choiceIndex = -1;
        chatInput.onSubmit.AddListener(SendChatMessage);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
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
    public void ShowEmojiByIndex(byte index)
    {
        for (int i = 0; i < _emojis.Count; i++)
        {
            if (i == index)
                _emojis[i].SetActive(true);
            else
                _emojis[i].SetActive(false);
        }
        _emojiBubble.SetTrigger("ShowEmoji");
    }

    private void EnableEmoji()
    {
        if (_emojiBubble.GetCurrentAnimatorStateInfo(0).IsName("ShowEmoji") || _choiceIndex == -1)
            return;

        NetworkCalls.Player_NetWork.Emote(_PV, (byte)_choiceIndex);
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

    private void SendChatMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        // Create a custom event to send the chat message
        byte eventCode = 1; // You can choose any value for the event code
        object[] eventData = new object[] { message };
        RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(eventCode, eventData, eventOptions, SendOptions.SendReliable);

        chatInput.text = ""; // Clear the chat input field
    }

    // Called when a custom event is received
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 1) // Match the event code used for chat messages
        {
            object[] data = (object[])photonEvent.CustomData;
            string message = (string)data[0];

            DisplayChatMessage(message);
        }
    }

    private void DisplayChatMessage(string message)
    {
        // Instantiate a new chat text template and set its content
        TextMeshProUGUI newChatText = Instantiate(chatTextTemplate, chatContent);
        newChatText.text = message;
    }
}