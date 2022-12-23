/* Last Edition: 12/20/2022
 * Editor: Chongyang Wang
 * Collaborators: 
 * References:
 * Description: 
 *     The scripts controls the info of a room UI tag instance.
 * Lastest Update:
 *     Updated Comments.
 */
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class UI_RoomInstance : MonoBehaviour
{
    #region Fields
    [SerializeField] private Text displayedRoomID;
    public string roomID;

    public RoomInfo RoomInfo { get; private set; }
    #endregion


    #region Custom Functions
    /// <summary>
    /// Function for setting the info for this room ui instance
    /// </summary>
    /// <param name="roomInfo"></param>
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        displayedRoomID.text = roomInfo.Name.Substring(0, roomInfo.Name.IndexOf("#")) + " " + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
        roomID = roomInfo.Name;
    }
    #endregion
}