using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SocialInteractionOption : MonoBehaviour
{
    public Transform anchorTransform;

    [SerializeField] private UI_SocialView _socialView;
    [SerializeField] private GameObject _checkMark;
    [SerializeField] private int _optionIndex;

    public void EnableCheckMark()
    {
        _checkMark.SetActive(true);
    }

    public void DisableCheckMark()
    {
        _checkMark.SetActive(false);
    }

    public void SetOptionIndex(int index)
    {
        _optionIndex = index;
    }

    public int GetOptionIndex()
    {
        return _optionIndex;
    }

    public void SetSocialViewSlotIndex()
    {
        _socialView.SelectSlotIndex(_optionIndex);
    }

    public void SetSocialViewOptionIndex()
    {
        _socialView.SelectOptionIndex(_optionIndex);
    }

    public void SelecteOption()
    {
        _checkMark.SetActive(true);
    }

    public void DeselectOption()
    {
        _checkMark.SetActive(false);
    }
}
