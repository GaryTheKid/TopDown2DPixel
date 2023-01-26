/* Last Edition: 12/20/2022
 * Editor: Chongyang Wang
 * Collaborators: 
 * References:
 * Description: 
 *     The scripts controls the info of a player UI tag instance.
 * Lastest Update:
 *     Updated Comments.
 */

using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class UI_PlayerInstance : MonoBehaviour
{
    [SerializeField] private Text displayedPlayerName;
    [SerializeField] private Text playerCharacterName;
    [SerializeField] private Image playerCharacterIcon;
    public string playerNickName;

    public Player PlayerInfo { get; private set; }

    public void SetPlayerInfo(Player playerInfo)
    {
        PlayerInfo = playerInfo;
        displayedPlayerName.text = playerInfo.NickName; //.Substring(0, playerInfo.UserId.IndexOf("#"))
        playerNickName = playerInfo.NickName;
        playerCharacterIcon.sprite = PlayerAssets.singleton.PlayerCharacterIconList[(int)playerInfo.CustomProperties["CharacterIndex"]];
        playerCharacterName.text = PlayerAssets.singleton.PlayerCharacterNameList[(int)playerInfo.CustomProperties["CharacterIndex"]];
    }
}
