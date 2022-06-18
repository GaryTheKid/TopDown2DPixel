using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // singleton
    public static NetworkManager networkManager;

    // fields
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _lobbyCanvas;
    [SerializeField] private GameObject _roomCanvas;
    [SerializeField] private Text _playerNameText;

    private void Awake()
    {
        networkManager = this;
    }

    public void ConnectToMaster_UseSettings()
    {
        // connect to the master server
        PhotonNetwork.GameVersion = GameSettings.gameSettings.gameVersion;
        PhotonNetwork.SendRate = GameSettings.gameSettings.sendRate;
        PhotonNetwork.SerializationRate = GameSettings.gameSettings.serializationRate;
        PhotonNetwork.AutomaticallySyncScene = true;
        if (GameSettings.gameSettings.playerName == "")
            PhotonNetwork.NickName = "Player#" + Random.Range(0, 10000);
        else
            PhotonNetwork.NickName = GameSettings.gameSettings.playerName + "#" + Random.Range(0, 10000);
        _playerNameText.text = PhotonNetwork.NickName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void DisconnectFromMaster()
    {
        // disconnect from the master server
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Successfully connected to master server!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from master server, cause: " + cause);
        _mainMenuCanvas.SetActive(true);
        _lobbyCanvas.SetActive(false);
        _roomCanvas.SetActive(false);

        // TODO: pop up the disconnect window

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        _mainMenuCanvas.SetActive(false);
        _lobbyCanvas.SetActive(true);
    }
}
