using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomInstance : MonoBehaviour
{
    [SerializeField] private Text displayedRoomID;
    public string roomID;

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        displayedRoomID.text = roomInfo.Name.Substring(0, roomInfo.Name.IndexOf("#")) + " " + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
        roomID = roomInfo.Name;
    }
}