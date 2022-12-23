/* Last Edition: 12/20/2022
 * Editor: Chongyang Wang
 * Collaborators: 
 * References:
 * Description: 
 *     The singleton manages functions and callbacks in the Photon lobby.
 * Lastest Update:
 *     Migirated from Chongyang's multiplayer project.
 */

using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Networking_LobbyManager : MonoBehaviourPunCallbacks
{
    #region Fields
    // singleton
    public static Networking_LobbyManager singleton;

    // public fields
    public string selectedRoomID;
    public int maxPlayer = 4;

    // private fields
    [SerializeField] private GameObject _lobbyCanvas;
    [SerializeField] private GameObject _roomCanvas;
    [SerializeField] private Text _roomTitle;
    [SerializeField] private Transform _roomListAnchor;
    [SerializeField] private UI_RoomInstance _roomInstance;
    [SerializeField] private List<UI_RoomInstance> _roomList = new List<UI_RoomInstance>();

    private string _roomName = "Room";
    private int _randomRoomID = 0;
    #endregion


    #region Unity Functions
    /// <summary>
    /// Set singleton
    /// </summary>
    private void Awake()
    {
        if (singleton != this)
        {
            singleton = this;
        }
    }
    #endregion


    #region Custom Functions


    /// <summary>
    /// Function for creating a room
    /// </summary>
    public void CreateRoom()
    {
        Debug.Log("Creating Room: " + _roomName);
        _randomRoomID = Random.Range(0, 10000);// create a new room with random name
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxPlayer };
        PhotonNetwork.CreateRoom(_roomName + "#" + _randomRoomID.ToString(), roomOptions);
        Debug.Log("Creating Room with ID " + "#" + _randomRoomID);
    }

    /// <summary>
    /// Function for selecting a room
    /// </summary>
    public void SelectRoom(string ID)
    {
        Debug.Log("Selecting Room: " + ID);
        selectedRoomID = ID;
    }

    /// <summary>
    /// Function for Joining a room
    /// </summary>
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
    #endregion


    #region Callback Functions
    /// <summary>
    /// Callback for successful creation of a Photon room
    /// </summary>
    public override void OnCreatedRoom()
    {
        Debug.Log("Sucessfully create room " + _roomName + "#" + _randomRoomID);
        _roomCanvas.SetActive(true);
        _lobbyCanvas.SetActive(false);
    }

    /// <summary>
    /// Callback for failing to create a Photon room
    /// </summary>
    /// <param name="returnCode"></param> The error code (fail reason)
    /// <param name="message"></param> The error message
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Fail to create room, return Code: " + returnCode + "   msg: " + message);
    }

    /// <summary>
    /// Callback for successfully joining a Photon room
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("Sucessfully join room " + selectedRoomID);
        _roomTitle.text = _roomName;
        _roomCanvas.SetActive(true);
        _lobbyCanvas.SetActive(false);
    }

    /// <summary>
    /// Callback for failing to join a Photon room
    /// </summary>
    /// <param name="returnCode"></param> The error code (fail reason)
    /// <param name="message"></param> The error message
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Fail to join room, return Code: " + returnCode + "   msg: " + message);
    }

    /// <summary>
    /// Callback for whenever the lobby's room list has been updated
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
                UI_RoomInstance instance = Instantiate(_roomInstance, _roomListAnchor);
                instance.GetComponent<Button>().onClick.AddListener(delegate { SelectRoom(instance.roomID); });
                if (instance != null)
                {
                    instance.SetRoomInfo(info);
                    _roomList.Add(instance);
                }
            }
        }
    }
    #endregion
}

