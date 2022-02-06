using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerInstance : MonoBehaviour
{
    [SerializeField] private Text displayedPlayerName;
    public string playerNickName;

    public Player PlayerInfo { get; private set; }

    public void SetPlayerInfo(Player playerInfo)
    {
        PlayerInfo = playerInfo;
        displayedPlayerName.text = playerInfo.NickName; //.Substring(0, playerInfo.UserId.IndexOf("#"))
        playerNickName = playerInfo.NickName;
    }
}
