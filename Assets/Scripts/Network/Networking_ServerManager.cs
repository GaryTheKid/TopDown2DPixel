/* Last Edition: 12/20/2022
 * Editor: Chongyang Wang
 * Collaborators: 
 * References: 
 * Description: 
 *     The singleton manages player's connection/disconnection to the Photon master server.
 * Lastest Update:
 *     Migirated from Chongyang's multiplayer project.
 */

using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Networking_ServerManager : MonoBehaviourPunCallbacks
{
    #region Fields
    // singleton
    public static Networking_ServerManager singleton;

    // private fields
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _lobbyCanvas;
    [SerializeField] private GameObject _roomCanvas;
    [SerializeField] private Text _playerNameText;
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
    /// function for connecting to the master server using info from GameSettings singleton
    /// </summary>
    public void ConnectToMaster_UseSettings()
    {
        // set up info
        PhotonNetwork.GameVersion = Networking_GameSettings.singleton.gameVersion;
        PhotonNetwork.SendRate = Networking_GameSettings.singleton.sendRate;
        PhotonNetwork.SerializationRate = Networking_GameSettings.singleton.serializationRate;
        PhotonNetwork.AutomaticallySyncScene = true;
        if (Networking_GameSettings.singleton.playerName == "")
            PhotonNetwork.NickName = "Player#" + Random.Range(0, 10000);
        else
            PhotonNetwork.NickName = Networking_GameSettings.singleton.playerName + "#" + Random.Range(0, 10000);
        _playerNameText.text = PhotonNetwork.NickName;

        // connect
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// function for disconnection
    /// </summary>
    public void DisconnectFromMaster()
    {
        PhotonNetwork.Disconnect();
    }
    #endregion


    #region Callback Functions
    /// <summary>
    /// Callback for successfully connecting to the master server
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Successfully connected to master server!");
        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// Callback for disconnection
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from the server, cause: " + cause);
        _mainMenuCanvas.SetActive(true);
        _lobbyCanvas.SetActive(false);
        _roomCanvas.SetActive(false);

        // TODO: pop up the disconnect window

    }

    /// <summary>
    /// Callback for successfully joining the lobby
    /// </summary>
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        _mainMenuCanvas.SetActive(false);
        _lobbyCanvas.SetActive(true);
    }
    #endregion
}
