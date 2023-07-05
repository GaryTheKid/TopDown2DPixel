using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using TMPro;

public class CloudCommunicator : MonoBehaviour
{
    // singleton
    public static CloudCommunicator singleton;

    // fields
    [SerializeField] private GameObject loadingFeedUIScreen;
    [SerializeField] private GameObject fadeInTransitionUI;
    [SerializeField] private Text loadingStateText;
    [SerializeField] private Image loadingProgress;
    [SerializeField] private TextMeshProUGUI percentageText;

    
    public float singleRequestSyncCD = 0.5f;
    public bool hasDataSynced;

    [Header("Player Settings")]
    public float BGMVolume;
    public float FXVolume;
    public bool isFullScreen;
    public int resolutionOption;
    public int frameRate;

    [Header("Player Data")]
    public string userId;
    public string userName;
    public int selectedCharacter = -1;
    public long gold;
    public long gem;
    public List<int> equippedEmojis;
    public List<bool> unlockedEmojis;
    public List<int> unlockedCharacters;

    private FirebaseAuth auth;
    private FirebaseFirestore db;
    private Coroutine loadingCoroutine;

    private void Awake()
    {
        if (singleton == null)
        {
            loadingFeedUIScreen.SetActive(true);
        }
        else
        {
            loadingFeedUIScreen.SetActive(false);
            fadeInTransitionUI.SetActive(true);
        }

        if (singleton != null && singleton != this)
        {
            Destroy(singleton.gameObject);
        }
        singleton = this;
    }

    private void Start()
    {
        // Get the FirebaseAuth instance
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;

        // Check the login status
        CheckLoginStatus();
    }

    private void UpdateLoadingProgress(string loadingState, float targetProgress)
    {
        // Stop any previous coroutines if they are running
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
        }

        // Start a new coroutine to smoothly transition the progress
        loadingCoroutine = StartCoroutine(TransitionProgress(loadingState, targetProgress));
    }

    private IEnumerator TransitionProgress(string loadingState, float targetProgress)
    {
        // Store the initial values
        float initialProgress = loadingProgress.fillAmount;
        int initialPercentage = Mathf.FloorToInt(initialProgress * 100f);

        // Set the loading state text immediately
        loadingStateText.text = loadingState;

        // Gradually update the progress and percentage text
        float elapsedTime = 0f;
        float duration = 1f; // The duration for the transition (in seconds)

        while (elapsedTime < duration)
        {
            // Calculate the current progress and percentage
            float currentProgress = Mathf.Lerp(initialProgress, targetProgress, elapsedTime / duration);
            int currentPercentage = Mathf.FloorToInt(currentProgress * 100f);

            // Update the loading progress and percentage text
            loadingProgress.fillAmount = currentProgress;
            percentageText.text = currentPercentage.ToString() + "%";

            // Wait for the next frame
            yield return null;

            // Update the elapsed time
            elapsedTime += Time.deltaTime;
        }

        // Ensure the final values are set
        loadingProgress.fillAmount = targetProgress;
        percentageText.text = Mathf.FloorToInt(targetProgress * 100f).ToString() + "%";
    }

    private void CheckLoginStatus()
    {
        UpdateLoadingProgress("Connecting...", 0);

        if (auth.CurrentUser != null)
        {
            // Access Firestore data
            userId = auth.CurrentUser.UserId;
            userName = auth.CurrentUser.DisplayName;

            // User is logged in
            Debug.Log("User is logged in: " + userId);

            // Call the function to check if the document exists
            CheckIfDocumentExists("PlayersCustomData", userId);
        }
        else
        {
            // User is not logged in
            Debug.Log("User is not logged in");
        }
    }

    private async void CheckIfDocumentExists(string collectionName, string documentName)
    {
        DocumentReference docRef = db.Collection(collectionName).Document(documentName);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            Debug.Log("Document exists!");

            TryGetDataFromCloud(snapshot, (bool isSuccessful) =>
            {
                if (isSuccessful)
                {
                    // TODO: disable the feed UI screen (loading screen => 100%)
                    loadingFeedUIScreen.SetActive(false);
                    fadeInTransitionUI.SetActive(true);
                    Debug.Log("Data sync from cloud successful!");

                    UpdateLoadingProgress("Retrieving Player Data...", 100);
                }
                else
                {
                    // pop the fail UI
                    PopCloudConnectionFailUI();
                    Debug.Log("Data sync from cloud failed!");
                }
            });
        }
        else
        {
            Debug.Log("Document does not exist. Creating a new one...");

            CreateNewPlayerDocument(docRef, snapshot, (bool isSyncSuccessful) => { Debug.Log("Create new player document on cloud:" + isSyncSuccessful); });

            Debug.Log("New document created!");


            // TODO: enable tutorial


        }
    }

    private async void CreateNewPlayerDocument(DocumentReference docRef, DocumentSnapshot snapshot, Action<bool> callback)
    {
        UpdateLoadingProgress("Creating Profile...", 20);

        List<bool> unlockedEmojis = new List<bool>();
        for (int i = 0; i < PlayerAssets.singleton.SocialInteractionList.Count; i++)
        {
            unlockedEmojis.Add(true);
        }

        // Create a new document
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            // player settings
            { "BGMVolume", 1f },
            { "FXVolume", 1f },
            { "isFullScreen", true },
            { "resolutionOption", 0 }, // mobile device differ 
            { "frameRate", 144 }, // mobile device differ

            // player data
            { "Currency_Gold", 0 },
            { "Currency_Gem", 0 },
            { "EquippedEmojis", new List<int>{ -1, -1, -1, -1 } },
            { "PlayerSettings", new List<string>() },
            { "SelectedCharacter", 1 },
            { "UnlockedCharacters", new List<int>(){ 0, 1 } },
            { "UnlockedEmojis", unlockedEmojis },
            // TODO: Add more fields as needed
        };

        UpdateLoadingProgress("Creating Profile...", 30);

        // Set the data for the new document
        // Update the specific field in the document
        try
        {
            await docRef.SetAsync(data);
            Debug.Log("Update operation completed successfully");

            // Invoke the callback with a true value to indicate success
            callback?.Invoke(true);

            UpdateLoadingProgress("Creating Profile...", 50);
        }
        catch (Exception e)
        {
            Debug.LogError("Update operation failed: " + e.Message);

            // Invoke the callback with a false value to indicate failure
            callback?.Invoke(false);
        }

        // sync data to local
        TryGetDataFromCloud(snapshot, (bool isSuccessful) => 
        {
            if (isSuccessful)
            {
                // TODO: disable the feed UI screen (loading screen => 100%)
                loadingFeedUIScreen.SetActive(false);
                fadeInTransitionUI.SetActive(true);
                Debug.Log("Data sync from cloud successful!");

                UpdateLoadingProgress("Retrieving Player Data...", 100);
            }
            else
            {
                // pop the fail UI
                PopCloudConnectionFailUI();
                Debug.Log("Data sync from cloud failed!");
            }
        });
    }

    public void TryGetDataFromCloud(DocumentSnapshot snapshot, Action<bool> OnGetDataFromCloudCallback)
    {
        UpdateLoadingProgress("Retrieving Player Data...", 70);

        // sync player's custom settings from the database
        Dictionary<string, object> data = snapshot.ToDictionary();

        // Access the player setting values from the data dictionary
        if (data.TryGetValue("BGMVolume", out object value_BGMVolume))
        {
            BGMVolume = Convert.ToSingle(value_BGMVolume);
        }
        else
        {
            Debug.LogError("Try get BGMVolume failed!");
            PopCloudConnectionFailUI();
            OnGetDataFromCloudCallback?.Invoke(false);
            return;
        }

        if (data.TryGetValue("FXVolume", out object value_FXVolume))
        {
            FXVolume = Convert.ToInt64(value_FXVolume);
        }
        else
        {
            Debug.LogError("Try get FXVolume failed!");
            PopCloudConnectionFailUI();
            OnGetDataFromCloudCallback?.Invoke(false);
            return;
        }

        if (data.TryGetValue("isFullScreen", out object value_IsFullScreen))
        {
            isFullScreen = Convert.ToBoolean(value_IsFullScreen);
        }
        else
        {
            Debug.LogError("Try get IsFullScreen failed!");
            PopCloudConnectionFailUI();
            OnGetDataFromCloudCallback?.Invoke(false);
            return;
        }

        if (data.TryGetValue("resolutionOption", out object value_ResolutionOption))
        {
            resolutionOption = Convert.ToInt32(value_ResolutionOption);
        }
        else
        {
            Debug.LogError("Try get resolutionOption failed!");
            PopCloudConnectionFailUI();
            OnGetDataFromCloudCallback?.Invoke(false);
            return;
        }

        if (data.TryGetValue("frameRate", out object value_FrameRate))
        {
            frameRate = Convert.ToInt32(value_FrameRate);
        }
        else
        {
            Debug.LogError("Try get frameRate failed!");
            PopCloudConnectionFailUI();
            OnGetDataFromCloudCallback?.Invoke(false);
            return;
        }

        // Access the player data values from the data dictionary
        if (data.TryGetValue("Currency_Gold", out object value_Gold))
        {
            gold = Convert.ToInt64(value_Gold);
        }
        else
        {
            Debug.LogError("Try get UnlockedCharacters failed!");
            PopCloudConnectionFailUI();
            OnGetDataFromCloudCallback?.Invoke(false);
            return;
        }

        if (data.TryGetValue("Currency_Gem", out object value_Gem))
        {
            gem = Convert.ToInt64(value_Gem);
        }
        else
        {
            Debug.LogError("Try get UnlockedCharacters failed!");
            PopCloudConnectionFailUI();
            OnGetDataFromCloudCallback?.Invoke(false);
            return;
        }

        if (data.TryGetValue("SelectedCharacter", out object value_SelectedCharacter))
        {
            selectedCharacter = Convert.ToInt32(value_SelectedCharacter);
        }
        else
        {
            Debug.LogError("Try get UnlockedCharacters failed!");
            PopCloudConnectionFailUI();
            OnGetDataFromCloudCallback?.Invoke(false);
            return;
        }

        if (data.TryGetValue("EquippedEmojis", out object value_EquippedEmojis))
        {
            List<object> list = (List<object>)value_EquippedEmojis;

            if (list != null)
            {
                List<int> vals = new List<int>();
                foreach (object obj in list)
                {
                    vals.Add(Convert.ToInt32(obj));
                }
                equippedEmojis = vals;
            }
        }
        else
        {
            Debug.LogError("Try get UnlockedCharacters failed!");
            PopCloudConnectionFailUI();
            OnGetDataFromCloudCallback?.Invoke(false);
            return;
        }

        if (data.TryGetValue("UnlockedEmojis", out object value_UnlockedEmojis))
        {
            List<object> list = (List<object>)value_UnlockedEmojis;

            if (list != null)
            {
                List<bool> vals = new List<bool>();
                foreach (object obj in list)
                {
                    vals.Add(Convert.ToBoolean(obj));
                }
                unlockedEmojis = vals;
            }
        }
        else
        {
            Debug.LogError("Try get UnlockedEmojis failed!");
            PopCloudConnectionFailUI();
            OnGetDataFromCloudCallback?.Invoke(false);
            return;
        }

        if (data.TryGetValue("UnlockedCharacters", out object value_UnlockedCharacters))
        {
            List<object> list = (List<object>)value_UnlockedCharacters;

            if (list != null)
            {
                List<int> vals = new List<int>();
                foreach (object obj in list)
                {
                    vals.Add(Convert.ToInt32(obj));
                }
                unlockedCharacters = vals;
            }
        }
        else
        {
            Debug.LogError("Try get UnlockedCharacters failed!");
            PopCloudConnectionFailUI();
            OnGetDataFromCloudCallback?.Invoke(false);
            return;
        }

        hasDataSynced = true;

        SyncDataToLocal();

        OnGetDataFromCloudCallback?.Invoke(true);
    }

    public void SyncPlayerCustomDataToCloud(string fieldName, object value, Action<bool> callback)
    {
        SetDataPointInDocument("PlayersCustomData", userId, fieldName, value, callback);
    }

    private async void SetDataPointInDocument(string collectionName, string documentName, string fieldName, object value, Action<bool> callback)
    {
        DocumentReference docRef = db.Collection(collectionName).Document(documentName);

        // Create a dictionary with the field to update
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { fieldName, value }
        };

        // Update the specific field in the document
        try
        {
            await docRef.UpdateAsync(data);
            Debug.Log("Update operation completed successfully");

            // Invoke the callback with a true value to indicate success
            callback?.Invoke(true);
        }
        catch (Exception e)
        {
            Debug.LogError("Update operation failed: " + e.Message);

            // Invoke the callback with a false value to indicate failure
            callback?.Invoke(false);
        }

        Debug.Log("Data point updated in the document!");
    }

    private void SyncDataToCloud()
    {
        // player settings
        SyncPlayerCustomDataToCloud("BGMVolume", PlayerSettings.singleton.BGMVolume, (bool isSyncSuccessful) => { Debug.Log("sync BGMVolume to cloud:" + isSyncSuccessful); });
        SyncPlayerCustomDataToCloud("FXVolume", PlayerSettings.singleton.FXVolume, (bool isSyncSuccessful) => { Debug.Log("sync FXVolume to cloud:" + isSyncSuccessful); });
        SyncPlayerCustomDataToCloud("isFullScreen", PlayerSettings.singleton.FullScreen, (bool isSyncSuccessful) => { Debug.Log("sync FullScreen to cloud:" + isSyncSuccessful); });
        SyncPlayerCustomDataToCloud("resolutionOption", PlayerSettings.singleton.ResolutionOption, (bool isSyncSuccessful) => { Debug.Log("sync resolutionOption to cloud:" + isSyncSuccessful); });
        SyncPlayerCustomDataToCloud("frameRate", PlayerSettings.singleton.FrameRate, (bool isSyncSuccessful) => { Debug.Log("sync frameRate to cloud:" + isSyncSuccessful); });

        // player data
        SyncPlayerCustomDataToCloud("Currency_Gold", PlayerSettings.singleton.Gold, (bool isSyncSuccessful) => { Debug.Log("sync gold to cloud:" + isSyncSuccessful); });
        SyncPlayerCustomDataToCloud("Currency_Gem", PlayerSettings.singleton.Gem, (bool isSyncSuccessful) => { Debug.Log("sync gem to cloud:" + isSyncSuccessful); });
        SyncPlayerCustomDataToCloud("SelectedCharacter", PlayerSettings.singleton.PlayerCharacterIndex, (bool isSyncSuccessful) => { Debug.Log("sync character index to cloud:" + isSyncSuccessful); });
        SyncPlayerCustomDataToCloud("EquippedEmojis", PlayerSettings.singleton.PlayerSocialIndexList, (bool isSyncSuccessful) => { Debug.Log("sync emoji list to cloud:" + isSyncSuccessful); });
    }

    private void SyncDataToLocal()
    {
        // player settings
        PlayerSettings.singleton.BGMVolume = BGMVolume;
        PlayerSettings.singleton.FXVolume = FXVolume;
        PlayerSettings.singleton.FullScreen = isFullScreen;
        PlayerSettings.singleton.ResolutionOption = resolutionOption;
        PlayerSettings.singleton.FrameRate = frameRate;
        PlayerSettings.singleton.UpdateAudioVolumes();

        // player data
        PlayerSettings.singleton.Gold = gold;
        PlayerSettings.singleton.Gem = gem;
        PlayerSettings.singleton.PlayerCharacterIndex = selectedCharacter;
        int[] socialList = new int[equippedEmojis.Count];
        for (int i = 0; i < equippedEmojis.Count; i++)
        {
            socialList[i] = equippedEmojis[i];
        }
        PlayerSettings.singleton.PlayerSocialIndexList = socialList;
    }

    public void SyncDataAndSignOut()
    {
        // Perform the necessary data synchronization before signing out
        SyncDataToCloud();

        // Sign out the user from Firebase
        FirebaseAuth.DefaultInstance.SignOut();

        // Perform any additional cleanup or tasks
        // ...
    }

    public void SignOutAndBackToLoginScene()
    {
        // Perform the necessary data synchronization before signing out
        SyncDataToCloud();

        // Sign out the user from Firebase
        FirebaseAuth.DefaultInstance.SignOut();

        // back to login scene
        SceneManager.LoadScene(Networking_GameSettings.singleton.loginSceneIndex);
    }

    public void PopCloudConnectionFailUI()
    {
        Debug.LogError("Transaction fails...Check internet!");

        // TODO: prompt re-connection
    }
}