using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;
using System;

public class CloudCommunicator : MonoBehaviour
{
    // singleton
    public static CloudCommunicator singleton;

    // public fields
    public float singleRequestSyncCD = 0.5f;
    public bool hasDataSynced;
    public string userId;
    public string userName;
    public int selectedCharacter = -1;
    public List<int> equippedEmojis;
    public List<bool> unlockedEmojis;
    public List<int> unlockedCharacters;

    // private fields
    private FirebaseAuth auth;
    private FirebaseFirestore db;

    private void Awake()
    {
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

    private void CheckLoginStatus()
    {
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

            TryGetDataFromCloud(snapshot);
        }
        else
        {
            Debug.Log("Document does not exist. Creating a new one...");

            CreateNewPlayerDocument(docRef, snapshot);

            Debug.Log("New document created!");
        }
    }

    private async void CreateNewPlayerDocument(DocumentReference docRef, DocumentSnapshot snapshot)
    {
        List<bool> unlockedEmojis = new List<bool>();
        for (int i = 0; i < PlayerAssets.singleton.SocialInteractionList.Count; i++)
        {
            unlockedEmojis.Add(true);
        }

        // Create a new document
        Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "EquippedEmojis", new List<int>{ -1, -1, -1, -1 } },
                { "PlayerSettings", new List<string>() },
                { "SelectedCharacter", 1 },
                { "UnlockedCharacters", new List<int>(){ 0, 1 } },
                { "UnlockedEmojis", unlockedEmojis },
                // TODO: Add more fields as needed
            };

        // Set the data for the new document
        await docRef.SetAsync(data);

        // sync data to local
        TryGetDataFromCloud(snapshot);
    }

    public void TryGetDataFromCloud(DocumentSnapshot snapshot)
    {
        // TODO: sync player's custom settings from the database
        Dictionary<string, object> data = snapshot.ToDictionary();

        // Access the values from the data dictionary
        if (data.TryGetValue("SelectedCharacter", out object value1))
        {
            selectedCharacter = Convert.ToInt32(value1);
        }

        if (data.TryGetValue("EquippedEmojis", out object value2))
        {
            List<object> list = (List<object>)value2;

            if (list != null)
            {
                List<int> vals = new List<int>();
                foreach (object obj in list)
                {
                    vals.Add(Convert.ToInt32(obj));
                }
                equippedEmojis = vals;
            }
            else
            {
                Debug.LogError("Try get EquippedEmojis failed!");
            }
        }

        if (data.TryGetValue("UnlockedEmojis", out object value3))
        {
            List<object> list = (List<object>)value3;

            if (list != null)
            {
                List<bool> vals = new List<bool>();
                foreach (object obj in list)
                {
                    vals.Add(Convert.ToBoolean(obj));
                }
                unlockedEmojis = vals;
            }
            else
            {
                Debug.LogError("Try get UnlockedEmojis failed!");
            }
        }

        if (data.TryGetValue("UnlockedEmojis", out object value4))
        {
            List<object> list = (List<object>)value4;

            if (list != null)
            {
                List<int> vals = new List<int>();
                foreach (object obj in list)
                {
                    vals.Add(Convert.ToInt32(obj));
                }
                unlockedCharacters = vals;
            }
            else
            {
                Debug.LogError("Try get UnlockedCharacters failed!");
            }
        }
        
        hasDataSynced = true;
    }

    public void SyncPlayerCustomDataToCloud(string fieldName, object value)
    {
        SetDataPointInDocument("PlayersCustomData", userId, fieldName, value);
    }

    private async void SetDataPointInDocument(string collectionName, string documentName, string fieldName, object value)
    {
        DocumentReference docRef = db.Collection(collectionName).Document(documentName);

        // Create a dictionary with the field to update
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { fieldName, value }
        };

        // Update the specific field in the document
        await docRef.UpdateAsync(data);

        Debug.Log("Data point updated in the document!");
    }
}