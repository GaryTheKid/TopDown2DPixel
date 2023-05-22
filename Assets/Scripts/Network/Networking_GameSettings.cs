/* Last Edition: 12/20/2022
 * Editor: Chongyang Wang
 * Collaborators: 
 * References: 
 * Description: 
 *    The singleton holds player's custom info for PhotonNetworking.
 * Lastest Update:
 *     Migirated from Chongyang's multiplayer project.
 */

using UnityEngine;
using UnityEngine.UI;

public class Networking_GameSettings : MonoBehaviour
{
    #region Fields
    // singleton
    public static Networking_GameSettings singleton;

    // public fields
    public string gameVersion;
    public string playerName;
    public int menuSceneIndex;
    public int gameSceneIndex;
    public int resultSceneIndex;
    public int sendRate;
    public int serializationRate;

    // private fields
    [SerializeField] private Text inputNameTextUI;
    #endregion


    #region Unity Functions
    /// <summary>
    /// Set singleton
    /// </summary>
    private void Awake()
    {
        singleton = this;
    }
    #endregion


    #region Custom Functions
    /// <summary>
    /// Function for setting the player name
    /// </summary>
    public void SetPlayerName()
    {
        playerName = inputNameTextUI.text;
    }
    #endregion
}
