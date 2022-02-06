using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // singleton
    public static LobbyManager lobbyManager;

    // create room settings
    public int maxPlayer = 4;
    private string roomName = "Room";
    private int randomRoomID = 0;

    // join room settings
    public string selectedRoomID;

    // fields
    [SerializeField] private GameObject _lobbyCanvas;
    [SerializeField] private GameObject _roomCanvas;
    [SerializeField] private Text _roomTitle;
    [SerializeField] private Transform _roomListAnchor;
    [SerializeField] private RoomInstance _roomInstance;
    [SerializeField] private List<RoomInstance> _roomList = new List<RoomInstance>();

    private void Awake()
    {
        lobbyManager = this;
    }

    /// <summary>
    /// Create room and its callback functions
    /// </summary>
    public void CreateRoom() 
    {
        Debug.Log("Creating Room: " + roomName);
        randomRoomID = Random.Range(0, 10000);// create a new room with random name
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxPlayer };
        PhotonNetwork.CreateRoom(roomName + "#"+ randomRoomID.ToString(), roomOptions);
        Debug.Log("Creating Room with ID "  + "#" + randomRoomID);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Sucessfully create room " + roomName + "#" + randomRoomID);
        _roomCanvas.SetActive(true);
        _lobbyCanvas.SetActive(false);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Fail to create room, return Code: " + returnCode + "   msg: " + message);
    }

    /// <summary>
    /// Join room and its callback functions
    /// </summary>
    public void SelectRoom(string ID)
    {
        Debug.Log("Selecting Room: " + ID);
        selectedRoomID = ID;
    }

    public void JoinRoom()
    {
        if (selectedRoomID == null || selectedRoomID == "") 
        {
            Debug.Log("Please select a room first!");
            return;
        }

        Debug.Log("Joining Room " + selectedRoomID + " ...");
        PhotonNetwork.JoinRoom(selectedRoomID.ToString());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Sucessfully join room " + selectedRoomID);
        _roomTitle.text = roomName;
        _roomCanvas.SetActive(true);
        _lobbyCanvas.SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Fail to join room, return Code: " + returnCode + "   msg: " + message);
    }

    /// <summary>
    /// Update the room list and its callback functions
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // removed from room list
            if (info.RemovedFromList)
            {
                int index = _roomList.FindIndex(x => x.roomID == info.Name);
                if (index != -1)
                {
                    Destroy(_roomList[index].gameObject);
                    _roomList.RemoveAt(index);
                }
            }
            // added to room list
            else if (!_roomList.Exists(x => x.roomID == info.Name))
            {
                RoomInstance instance = Instantiate(_roomInstance, _roomListAnchor);
                instance.GetComponent<Button>().onClick.AddListener(delegate { SelectRoom(instance.roomID); });
                if (instance != null)
                {
                    instance.SetRoomInfo(info);
                    _roomList.Add(instance);
                }
            }
        }
    }
}
