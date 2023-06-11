using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SocialView : MonoBehaviour
{
    [SerializeField] private UI_SocialInteractionEquipmentWheel _socialInteractionEquipmentWheel;
    [SerializeField] private Transform _emojiParent;
    [SerializeField] private Transform _wordsParent;
    [SerializeField] private Transform _emojiOptionTemplate;
    [SerializeField] private Transform _wordsOptionTemplate;
    [SerializeField] private GameObject _deleteButton;
    [SerializeField] private List<UI_SocialInteractionOption> _emojiList;
    [SerializeField] private List<UI_SocialInteractionOption> _wordsList;

    [SerializeField] private int _selectedSlotIndex;
    [SerializeField] private int _selectedOptionIndex;
    [SerializeField] private bool _isClearingSlot;

    private IEnumerator Co_SyncPlayerCustomProperty;

    private void OnDisable()
    {
        SetPlayerData();
    }

    private void Start()
    {
        // init emoji list
        _emojiList = new List<UI_SocialInteractionOption>();
        for (int i = 0; i < PlayerAssets.singleton.emojiCount; i++)
        {
            var emojiObj = Instantiate(_emojiOptionTemplate, _emojiParent);
            var emojiOption = emojiObj.GetComponent<UI_SocialInteractionOption>();
            emojiOption.SetOptionIndex(i);
            emojiOption.DisableCheckMark();
            var socialInteractionOption = PlayerAssets.singleton.SocialInteractionList[i];
            if (socialInteractionOption)
            {
                Instantiate(socialInteractionOption.staticObj, emojiOption.anchorTransform);
            }
            _emojiList.Add(emojiOption);
            emojiObj.gameObject.SetActive(true);
        }

        // init words list
        _wordsList = new List<UI_SocialInteractionOption>();
        for (int i = 0; i < PlayerAssets.singleton.SocialInteractionList.Count - PlayerAssets.singleton.emojiCount; i++)
        {
            var wordsObj = Instantiate(_wordsOptionTemplate, _wordsParent);
            var wordsOption = wordsObj.GetComponent<UI_SocialInteractionOption>();
            wordsOption.SetOptionIndex(i + PlayerAssets.singleton.emojiCount);
            wordsOption.DisableCheckMark();
            var socialInteractionOption = PlayerAssets.singleton.SocialInteractionList[i + PlayerAssets.singleton.emojiCount];
            if (socialInteractionOption)
            {
                Instantiate(socialInteractionOption.staticObj, wordsOption.anchorTransform);
            }
            _wordsList.Add(wordsOption);
            wordsObj.gameObject.SetActive(true);
        }

        // init selected indices
        _selectedSlotIndex = -1;
        _selectedOptionIndex = -1;
    }

    public void ClearSlotIndex()
    {
        _isClearingSlot = true;
        _deleteButton.GetComponent<Button>().interactable = false;

        if (_selectedSlotIndex != -1)
        {
            ClearSelectedSlot(_selectedSlotIndex);
            _isClearingSlot = false;
            _deleteButton.SetActive(false);
        }
    }

    public void SelectSlotIndex(int index)
    {
        if (!_socialInteractionEquipmentWheel.IsSlotEmpty(index) && _selectedOptionIndex == -1)
        {
            // enable delete button
            _deleteButton.SetActive(true);
            _deleteButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            // enable delete button
            _deleteButton.SetActive(false);
            _deleteButton.GetComponent<Button>().interactable = false;
        }

        // clear the current 
        if (_isClearingSlot)
        {
            ClearSelectedSlot(index);
            _isClearingSlot = false;
            _deleteButton.SetActive(false);
        }

        // disable the current selected slot button interable
        _socialInteractionEquipmentWheel.DisableSelectedSlotInteraction(index);

        _selectedSlotIndex = index;
        CheckSlotOptionMatching();
    }
    
    public void SelectOptionIndex(int index)
    {
        // disable delete button
        _isClearingSlot = false;
        _deleteButton.GetComponent<Button>().interactable = false;
        _deleteButton.SetActive(false);

        // disable the current selected option button interable
        DisableSelectedOptionInteraction(index);

        _selectedOptionIndex = index;
        CheckSlotOptionMatching();
    }

    public void ShowSelectedHightlight(int index)
    {
        if (index == -1)
        {
            print("Show selected highlight index invalid");
            return;
        }

        if (index <= _emojiList.Count - 1)
        {
            _emojiList[index].SelecteOption();
        }
        else
        {
            _wordsList[index - _emojiList.Count].SelecteOption();
        }
    }

    public void HideSelectedHightlight(int index)
    {
        if (index == -1)
        {
            print("Show selected highlight index invalid");
            return;
        }

        if (index <= _emojiList.Count - 1)
        {
            _emojiList[index].DeselectOption();
        }
        else
        {
            _wordsList[index - _emojiList.Count].DeselectOption();
        }
    }

    private void ClearSelectedSlot(int index)
    {
        _socialInteractionEquipmentWheel.UnequipSlot(index);
        _socialInteractionEquipmentWheel.FreeAllSlotsInteraction();
        FreeAllOptionsInteraction();

        // update the player customized properties
        SyncPlayerCustomProperty();

        // reset
        _selectedSlotIndex = -1;
    }

    private void CheckSlotOptionMatching()
    {
        if (_selectedSlotIndex != -1 && _selectedOptionIndex != -1)
        {
            _socialInteractionEquipmentWheel.UnequipSlot(_selectedSlotIndex);
            _socialInteractionEquipmentWheel.EquipSlot(_selectedSlotIndex, _selectedOptionIndex);
            _socialInteractionEquipmentWheel.FreeAllSlotsInteraction();
            FreeAllOptionsInteraction();

            // sync the player customized properties
            SyncPlayerCustomProperty();

            // reset
            _selectedSlotIndex = -1;
            _selectedOptionIndex = -1;
        }
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
        PlayerSettings.singleton.PlayerSocialIndexList = _socialInteractionEquipmentWheel.GetSlotOptionList();
        CloudCommunicator.singleton.SyncPlayerCustomDataToCloud("EquippedEmojis", PlayerSettings.singleton.PlayerSocialIndexList, (bool isSuccessful) =>
        {
            if (isSuccessful)
            {
                Debug.Log("Emoji list sync to cloud successfully");
            }
            else
            {
                CloudCommunicator.singleton.PopCloudConnectionFailUI();
            }
        });
    }

    private void DisableSelectedOptionInteraction(int index)
    {
        for (int i = 0; i < _emojiList.Count; i++)
        {
            if (i == index)
            {
                _emojiList[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                _emojiList[i].GetComponent<Button>().interactable = true;
            }
        }

        for (int i = 0; i < _wordsList.Count; i++)
        {
            if (i == index - _emojiList.Count)
            {
                _wordsList[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                _wordsList[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    private void FreeAllOptionsInteraction()
    {
        for (int i = 0; i < _emojiList.Count; i++)
        {
            _emojiList[i].GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < _wordsList.Count; i++)
        {
            _wordsList[i].GetComponent<Button>().interactable = true;
        }
    }
}
