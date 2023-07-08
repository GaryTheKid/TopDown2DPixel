using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_GlobalTypeChat : MonoBehaviour
{
    private const float FREEZE_DURATION = 5f;
    private const float FADE_DURATION = 2f;

    [SerializeField] private GameObject _chatTab;
    [SerializeField] private TMP_InputField _chatInput;

    private CanvasGroup chatMessageCanvasGroup;
    private Coroutine chatFadeCoroutine;

    private void Start()
    {
        _chatInput.onSubmit.AddListener(SendChatMessage);

        chatMessageCanvasGroup = GameManager.singleton.chatContent.GetComponent<CanvasGroup>();

        // Start with all chat messages fully transparent
        SetChatMessagesAlpha(0f);
    }

    public void ActivateTab(InputAction.CallbackContext context)
    {
        if (!_chatTab.activeInHierarchy)
        {
            _chatTab.SetActive(true);
            _chatInput.Select();

            // Restore transparency of chat messages immediately
            SetChatMessagesAlpha(1f);
        }
        else if (string.IsNullOrWhiteSpace(_chatInput.text))
        {
            _chatTab.SetActive(false);
            StartChatFadeCoroutine();
        }
    }

    private void SendChatMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        // Truncate the message if it exceeds the character limit
        if (message.Length > _chatInput.characterLimit)
        {
            message = message.Substring(0, _chatInput.characterLimit);
        }

        NetworkCalls.Game_Network.ReceiveChatMessage(GameManager.singleton.PV, message, (byte)PhotonNetwork.LocalPlayer.ActorNumber);

        _chatInput.text = ""; // Clear the chat input field

        _chatTab.SetActive(false);
    }

    public void StartChatFadeCoroutine()
    {
        if (chatFadeCoroutine != null)
        {
            StopCoroutine(chatFadeCoroutine);
        }

        chatFadeCoroutine = StartCoroutine(ChatFadeCoroutine());
    }

    private IEnumerator ChatFadeCoroutine()
    {
        float currentAlpha = 1f;
        float targetAlpha = 0f;

        SetChatMessagesAlpha(currentAlpha);

        // freeze for a while
        yield return new WaitForSeconds(FREEZE_DURATION);

        // check if the tab is active
        if (_chatTab.activeInHierarchy)
        {
            yield break;
        }

        float elapsedTime = 0f;
        while (elapsedTime < FADE_DURATION)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / FADE_DURATION);

            float alpha = Mathf.Lerp(currentAlpha, targetAlpha, t);
            SetChatMessagesAlpha(alpha);

            yield return null;
        }

        // Ensure the alpha is set to the target value after the coroutine ends
        SetChatMessagesAlpha(targetAlpha);

        chatFadeCoroutine = null;
    }

    private void SetChatMessagesAlpha(float alpha)
    {
        chatMessageCanvasGroup.alpha = alpha;
    }
}