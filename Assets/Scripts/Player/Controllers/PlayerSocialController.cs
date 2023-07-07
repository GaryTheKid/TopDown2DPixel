using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Utilities;
using Photon.Pun;
using TMPro;

public class PlayerSocialController : MonoBehaviour//, IOnEventCallback
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

    private Color myColor;
    private Color allyColor;
    private Color enemyColor;

    private void Start()
    {
        // initialization
        _PV = GetComponent<PhotonView>();
        _inputActions = GetComponent<PlayerInputActions>().inputActions;
        LoadSocialInputActions();
        _choiceIndex = -1;
        chatInput.onSubmit.AddListener(SendChatMessage);

        // Get the color values from GameManager.singleton
        myColor = GameManager.singleton.myColor;
        allyColor = GameManager.singleton.allyColor;
        enemyColor = GameManager.singleton.enemyColor;
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

        byte senderActorNumber = (byte)_PV.OwnerActorNr;

        _PV.RPC("ReceiveChatMessage", RpcTarget.All, message, senderActorNumber);

        chatInput.text = ""; // Clear the chat input field
    }

    [PunRPC]
    private void ReceiveChatMessage(string message, byte senderActorNumber)
    {
        DisplayChatMessage(message, senderActorNumber);
    }

    private void DisplayChatMessage(string message, byte senderActorNumber)
    {
        // get player name
        string playerName = "";
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == senderActorNumber)
            {
                playerName = player.NickName.Split("#")[0];
            }
        }

        // determine sender group

        // TODO: add ally for future mode

        Color senderColor = Color.white;
        if (senderActorNumber == _PV.OwnerActorNr)
        {
            // me 
            senderColor = myColor;
        }
        else 
        {
            // enemy
            senderColor = enemyColor;
        }

        TextMeshProUGUI newChatText = Instantiate(chatTextTemplate, chatContent);
        newChatText.richText = true;
        newChatText.text = string.Format("<color=#{0}>{1}:</color> {2}", ColorUtility.ToHtmlStringRGB(senderColor), playerName, message);
    }
}