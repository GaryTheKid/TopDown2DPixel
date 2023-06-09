using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_CharacterSelection : MonoBehaviour
{
    private UI_CharacterSelectionList characterSelectionList;

    private void Start()
    {
        characterSelectionList = GetComponent<UI_CharacterSelectionList>();
        StartCoroutine(GetCloud_SelectCharacter());
    }

    private IEnumerator GetCloud_SelectCharacter()
    {
        yield return new WaitUntil(() =>
        {
            return CloudCommunicator.singleton.hasDataSynced;
        });

        var selectedCharacter_cloud = CloudCommunicator.singleton.selectedCharacter;
        characterSelectionList.ImmediateSwitchToCharacter(selectedCharacter_cloud);
        PlayerSettings.singleton.PlayerCharacterIndex = selectedCharacter_cloud;
    }
}
