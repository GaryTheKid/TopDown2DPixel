/* Last Edition: 1/25/2023
 * Editor: Chongyang Wang
 * Collaborators: 
 * References: 
 * Description: 
 *    The UI for character selection.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class UI_CharacterSelectionList : MonoBehaviour
{
    public List<Transform> characterSelectionList;
    public List<string> characterNameList;
    public int currentCharacterIndex;

    [SerializeField] private MMF_Player _mmFeedBack;
    [SerializeField] private GameObject _selectionButton_Left;
    [SerializeField] private GameObject _selectionButton_Right;
    [SerializeField] private Text _selectedCharacterNameText;

    private void Start()
    {
        // initialize character list for selection
        characterSelectionList = new List<Transform>();
        foreach (Transform child in transform)
        {
            characterSelectionList.Add(child);
        }
        UpdateCharacterSelectionList();
    }

    public void SwitchToCharacter(int newIndex)
    {
        if (newIndex > characterSelectionList.Count - 1 || newIndex < 0)
        {
            return;
        }

        currentCharacterIndex = newIndex;
        _mmFeedBack.GetFeedbackOfType<MMF_Position>().DestinationPositionTransform = characterSelectionList[characterSelectionList.Count - newIndex - 1];
        _mmFeedBack.PlayFeedbacks();
        UpdateCharacterSelectionList();
    }

    public void SwitchToNextCharacter()
    {
        if (currentCharacterIndex + 1 > characterSelectionList.Count - 1)
        {
            return;
        }

        currentCharacterIndex++;

        _mmFeedBack.GetFeedbackOfType<MMF_Position>().DestinationPositionTransform = characterSelectionList[characterSelectionList.Count - currentCharacterIndex - 1];
        _mmFeedBack.PlayFeedbacks();
        UpdateCharacterSelectionList();
    }

    public void SwitchToPrevCharacter()
    {
        if (currentCharacterIndex - 1 < 0)
        {
            return;
        }

        currentCharacterIndex--;

        _mmFeedBack.GetFeedbackOfType<MMF_Position>().DestinationPositionTransform = characterSelectionList[characterSelectionList.Count - currentCharacterIndex - 1];
        _mmFeedBack.PlayFeedbacks();
        UpdateCharacterSelectionList();
    }

    public void UpdateCharacterSelectionList()
    {
        // edge detection
        if (currentCharacterIndex == characterSelectionList.Count - 1)
        {
            _selectionButton_Right.SetActive(false);
        }
        else if (currentCharacterIndex == 0)
        {
            _selectionButton_Left.SetActive(false);
        }
        else
        {
            _selectionButton_Right.SetActive(true);
            _selectionButton_Left.SetActive(true);
        }

        // update selected character
        _selectedCharacterNameText.text = characterNameList[currentCharacterIndex];
        for (int i = 0; i < characterSelectionList.Count; i++)
        {
            if (i == currentCharacterIndex)
            {
                characterSelectionList[i].GetComponent<Animator>().SetBool("isSelected", true);
            }
            else
            {
                characterSelectionList[i].GetComponent<Animator>().SetBool("isSelected", false);
            }
        }
        PlayerSettings.singleton.PlayerCharacterIndex = currentCharacterIndex;
    }
}
