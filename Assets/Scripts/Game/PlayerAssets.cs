/* Last Edition: 1/26/2023
 * Editor: Chongyang Wang
 * Collaborators: 
 * References: 
 * Description: 
 *    The singleton holds all player assets.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssets : MonoBehaviour
{
    #region Fields
    // singleton
    public static PlayerAssets singleton;

    // public fields
    [Header("Character")]
    public List<Sprite> PlayerCharacterIconList;
    public List<string> PlayerCharacterNameList;
    public List<GameObject> PlayerCharacterAvatarList;

    [Header("Social")]
    public List<SocialInteractionAsset> SocialInteractionList;
    public int emojiCount;
    #endregion


    #region Unity Functions
    /// <summary>
    /// Set singleton
    /// </summary>
    private void Awake()
    {
        singleton = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
}
