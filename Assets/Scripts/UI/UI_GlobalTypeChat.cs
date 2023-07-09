using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_GlobalTypeChat : MonoBehaviourPunCallbacks
{
    private const float FREEZE_DURATION = 5f;
    private const float FADE_DURATION = 2f;
    private const int MESSAGE_POOL_SIZE = 5;
    private const float RESET_TIMER_DELAY = 2f;

    [SerializeField] private GameObject _chatTab;
    [SerializeField] private TMP_InputField _chatInput;

    private PlayerStats _playerStats;
    private CanvasGroup _chatMessageCanvasGroup;
    private Coroutine _chatFadeCoroutine;
    private int availableMessageCount = MESSAGE_POOL_SIZE;
    private Coroutine resetTimerCoroutine;

    private void Start()
    {
        _chatInput.onSubmit.AddListener(SendChatMessage);

        _chatMessageCanvasGroup = GameManager.singleton.chatContent.GetComponent<CanvasGroup>();

        // Start with all chat messages fully transparent
        SetChatMessagesAlpha(0f);
    }

    public void BindLocalPlayer(PlayerStats playerStats)
    {
        _playerStats = playerStats;
    }

    public void ActivateTab(InputAction.CallbackContext context)
    {
        if (!_chatTab.activeInHierarchy)
        {
            _chatTab.SetActive(true);
            _chatInput.Select();
            if (_playerStats != null)
                _playerStats.isTyping = true;

            // Restore transparency of chat messages immediately
            SetChatMessagesAlpha(1f);
        }
        else if (string.IsNullOrWhiteSpace(_chatInput.text))
        {
            _chatTab.SetActive(false);
            if (_playerStats != null)
                _playerStats.isTyping = false;

            StartChatFadeCoroutine();
        }
    }

    private void SendChatMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        if (availableMessageCount <= 0)
        {
            // Message pool is empty, set cooldown timer and block message sending
            StartResetTimer();

            // Display a local message in the chat content
            DisplayLocalMessage("Please type slower.");

            return;
        }

        // Truncate the message if it exceeds the character limit
        if (message.Length > _chatInput.characterLimit)
        {
            message = message.Substring(0, _chatInput.characterLimit);
        }

        NetworkCalls.Game_Network.ReceiveChatMessage(GameManager.singleton.PV, message, (byte)PhotonNetwork.LocalPlayer.ActorNumber);

        _chatInput.text = ""; // Clear the chat input field

        if (_playerStats != null)
            _playerStats.isTyping = false; 
        _chatTab.SetActive(false);

        // Reduce the available message count
        availableMessageCount--;

        // Restart the reset timer coroutine if it's already running
        if (resetTimerCoroutine != null)
        {
            StopCoroutine(resetTimerCoroutine);
        }
        resetTimerCoroutine = StartCoroutine(ResetTimerCoroutine());
    }

    public void StartChatFadeCoroutine()
    {
        if (_chatFadeCoroutine != null)
            StopCoroutine(_chatFadeCoroutine);
        _chatFadeCoroutine = StartCoroutine(ChatFadeCoroutine());
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
            yield break;

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

        _chatFadeCoroutine = null;
    }

    private void DisplayLocalMessage(string message)
    {
        // create text obj
        TextMeshProUGUI newChatText = Instantiate(GameManager.singleton.chatTextTemplate, GameManager.singleton.chatContent);
        newChatText.richText = true;
        newChatText.text = message;
        newChatText.color = Color.yellow;
        newChatText.gameObject.SetActive(true);

        // Add the new chat text to the list
        GameManager.singleton.chatTexts.Add(newChatText);

        // Check if the maximum text count is exceeded
        if (GameManager.singleton.chatTexts.Count > GameManager.singleton.maxTextCount)
        {
            // Remove the earliest chat text from the list
            TextMeshProUGUI oldestChatText = GameManager.singleton.chatTexts[0];
            GameManager.singleton.chatTexts.RemoveAt(0);
            Destroy(oldestChatText.gameObject);
        }

        // Scroll the chat content to the bottom;
        GameManager.singleton.chatContent.GetComponentInParent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }

    private void SetChatMessagesAlpha(float alpha)
    {
        _chatMessageCanvasGroup.alpha = alpha;
    }

    private void StartResetTimer()
    {
        if (resetTimerCoroutine != null)
            StopCoroutine(resetTimerCoroutine);
        resetTimerCoroutine = StartCoroutine(ResetTimerCoroutine());
    }

    private IEnumerator ResetTimerCoroutine()
    {
        yield return new WaitForSeconds(RESET_TIMER_DELAY);

        // Refill the message pool
        availableMessageCount = MESSAGE_POOL_SIZE;

        resetTimerCoroutine = null;
    }
}