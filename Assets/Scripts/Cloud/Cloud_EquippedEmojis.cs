using System.Collections;
using UnityEngine;

public class Cloud_EquippedEmojis : MonoBehaviour
{
    private UI_SocialInteractionEquipmentWheel socialinteractionEquipmentWheel;

    private void Start()
    {
        socialinteractionEquipmentWheel = GetComponent<UI_SocialInteractionEquipmentWheel>();
        StartCoroutine(GetCloud_EquippedEmojis());
    }

    private IEnumerator GetCloud_EquippedEmojis()
    {
        yield return new WaitUntil(() =>
        {
            return CloudCommunicator.singleton.hasDataSynced;
        });

        var equippedEmojis_cloud = CloudCommunicator.singleton.equippedEmojis;
        PlayerSettings.singleton.PlayerSocialIndexList = new int[4];
        for (int i = 0; i < equippedEmojis_cloud.Count; i++)
        {
            socialinteractionEquipmentWheel.EquipSlot(i, equippedEmojis_cloud[i]);
            PlayerSettings.singleton.PlayerSocialIndexList[i] = equippedEmojis_cloud[i];
        }
    }
}