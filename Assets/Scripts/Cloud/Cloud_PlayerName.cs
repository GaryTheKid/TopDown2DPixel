using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cloud_PlayerName : MonoBehaviour
{
    private Text playerName;

    private void Start()
    {
        playerName = GetComponent<Text>();
        StartCoroutine(GetCloud_PlayerName());
    }

    private IEnumerator GetCloud_PlayerName()
    {
        yield return new WaitUntil( () => 
        { 
            return CloudCommunicator.singleton.hasDataSynced; 
        });

        var userName = CloudCommunicator.singleton.userName;
        playerName.text = userName;
        Networking_GameSettings.singleton.playerName = userName;
    } 
}
