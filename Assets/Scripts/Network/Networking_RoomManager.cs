using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Networking_RoomManager : MonoBehaviourPunCallbacks
{
    // singleton
    public static Networking_RoomManager singleton;

    // lobby/room setting
    public int maxPlayer = 4;

    // fields
    [SerializeField] private GameObject _lobbyCanvas;
    [SerializeField] private GameObject _roomCanvas;
    [SerializeField] private GameObject _startGameButton;
    [SerializeField] private Transform _playerListAnchor;
    [SerializeField] private UI_PlayerInstance _playerInstance;
    [SerializeField] private List<UI_PlayerInstance> _playerList = new List<UI_PlayerInstance>();

    private void Awake()
    {
        singleton = this;
    }

    public void StartGame()
    {
        // room owner only
        Debug.Log("Starting the game...");

        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.LoadLevel(Networking_GameSettings.singleton.gameSceneIndex);
    }

    public void BackToLobby()
    {
        Debug.Log("Leaving room...");
        PhotonNetwork.LeaveRoom();
        _startGameButton.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        UpdateRoomPlayerList();

        // if not master client (host), disable the start game button
        if (!PhotonNetwork.IsMasterClient)
            _startGameButton.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Back to Lobby");
        RemovePlayerFromList(PhotonNetwork.LocalPlayer);
        _roomCanvas.SetActive(false);
        _lobbyCanvas.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerToList(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemovePlayerFromList(otherPlayer);

        // if not master client (host), disable the start game button
        if (PhotonNetwork.IsMasterClient)
            _startGameButton.SetActive(true);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
    }

    private void AddPlayerToList(Player newPlayer) 
    {
        if (!_playerList.Exists(x => x.playerNickName == newPlayer.NickName))
        {
            Debug.Log(newPlayer.NickName + " has joined room!");
            UI_PlayerInstance instance = Instantiate(_playerInstance, _playerListAnchor);
            //instance.GetComponent<Button>().onClick.AddListener(delegate { SelectRoom(instance.roomID); });
            if (instance != null)
            {
                instance.SetPlayerInfo(newPlayer);
                _playerList.Add(instance);
            }
        }
    }

    private void RemovePlayerFromList(Player otherPlayer)
    {
        // removed from room list
        int index = _playerList.FindIndex(x => x.playerNickName == otherPlayer.NickName);
        if (index != -1)
        {
            Debug.Log(otherPlayer.NickName + " has left room!");
            Destroy(_playerList[index].gameObject);
            _playerList.RemoveAt(index);
        }
    }

    private void UpdateRoomPlayerList()
    {
        // clear list 
        foreach (UI_PlayerInstance instance in _playerList)
        {
            Destroy(instance.gameObject);
        }
        _playerList.Clear();

        // add players
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddPlayerToList(player);
        }
    }
}
