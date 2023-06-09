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
    public int currentCharacterIndex;

    [SerializeField] private MMF_Player _mmFeedBack;
    [SerializeField] private GameObject _selectionButton_Left;
    [SerializeField] private GameObject _selectionButton_Right;
    [SerializeField] private Text _selectedCharacterNameText;
    [SerializeField] private Transform _moveTarget;

    private IEnumerator Co_SyncPlayerCustomProperty;

    private void Start()
    {
        // initialize character list for selection
        characterSelectionList = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.name == "moveTarget")
                continue;
            characterSelectionList.Add(child);
        }
        UpdateCharacterSelectionList();
    }

    private void OnEnable()
    {
        try
        {
            UpdateCharacterSelectionList();
        }
        catch
        {}
    }

    private void OnDisable()
    {
        try
        {
            SetPlayerData();
        }
        catch
        { }
    }

    public void ImmediateSwitchToCharacter(int newIndex)
    {
        if (newIndex > characterSelectionList.Count - 1 || newIndex < 0)
        {
            return;
        }

        currentCharacterIndex = newIndex;
        GetComponent<RectTransform>().localPosition = new Vector3(-characterSelectionList[newIndex].localPosition.x, 0f, 0f);
        UpdateCharacterSelectionList();
    }

    public void SwitchToCharacter(int newIndex)
    {
        if (newIndex > characterSelectionList.Count - 1 || newIndex < 0)
        {
            return;
        }

        currentCharacterIndex = newIndex;

        _moveTarget.GetComponent<RectTransform>().localPosition = new Vector3(-characterSelectionList[newIndex].localPosition.x, 0f, 0f);
        _mmFeedBack.GetFeedbackOfType<MMF_Position>().DestinationPositionTransform = _moveTarget;
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

        _moveTarget.GetComponent<RectTransform>().localPosition = new Vector3(-characterSelectionList[currentCharacterIndex].localPosition.x, 0f, 0f);
        _mmFeedBack.GetFeedbackOfType<MMF_Position>().DestinationPositionTransform = _moveTarget;
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

        _moveTarget.GetComponent<RectTransform>().localPosition = new Vector3(-characterSelectionList[currentCharacterIndex].localPosition.x, 0f, 0f);
        _mmFeedBack.GetFeedbackOfType<MMF_Position>().DestinationPositionTransform = _moveTarget;
        _mmFeedBack.GetFeedbackOfType<MMF_Position>().DestinationPositionTransform = _moveTarget;
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
        _selectedCharacterNameText.text = PlayerAssets.singleton.PlayerCharacterNameList[currentCharacterIndex];
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

        SyncPlayerCustomProperty();
    }

    private void SyncPlayerCustomProperty()
    {
        if (Co_SyncPlayerCustomProperty != null)
        {
            StopCoroutine(Co_SyncPlayerCustomProperty);
            Co_SyncPlayerCustomProperty = null;
        }
        Co_SyncPlayerCustomProperty = SyncPlayerCustomProperty_Co();
        StartCoroutine(Co_SyncPlayerCustomProperty);
    }

    private IEnumerator SyncPlayerCustomProperty_Co()
    {
        yield return new WaitForSeconds(CloudCommunicator.singleton.singleRequestSyncCD);

        SetPlayerData();
    }

    private void SetPlayerData()
    {
        PlayerSettings.singleton.PlayerCharacterIndex = currentCharacterIndex;
        CloudCommunicator.singleton.SyncPlayerCustomDataToCloud("SelectedCharacter", PlayerSettings.singleton.PlayerCharacterIndex);
    }
}
