using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerResultController : MonoBehaviour
{
    [SerializeField] private TextMeshPro nameTag;
    [SerializeField] private SpriteRenderer ring;
    [SerializeField] private GameObject indicators;

    public void SetPlayerInfo(byte actorNumber)
    {
        Player player = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);

        // set avatar
        var avatar = Instantiate(PlayerAssets.singleton.PlayerCharacterAvatarList[(int)player.CustomProperties["CharacterIndex"]], transform.position, Quaternion.identity);
        avatar.transform.parent = transform;
        avatar.transform.Find("Body").GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;

        // set name tag
        nameTag.text = player.NickName.Substring(0, player.NickName.IndexOf("#"));

        // set indicators
        indicators.SetActive(true);

        // set ring color
        if (player.IsLocal)
        {
            ring.color = Color.green;
        }
    }
}
