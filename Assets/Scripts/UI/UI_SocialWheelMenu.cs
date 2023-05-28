using System.Collections.Generic;
using UnityEngine;

public class UI_SocialWheelMenu : MonoBehaviour
{
    private const float SCALER_CANCELZONE = 0.06f;

    [SerializeField] private List<UI_SocialWheelChoice> _choiceList;
    [SerializeField] private GameObject _cancelIndicator;

    public int SelectChoiceByAngle(Vector3 pointerPos, float angle)
    {
        int choiceIndex = -1;

        // check if in the cancel range
        float dynamicCancelRange = Screen.height * SCALER_CANCELZONE;

        if (pointerPos.x < transform.position.x + dynamicCancelRange 
            && pointerPos.x > transform.position.x - dynamicCancelRange
            && pointerPos.y < transform.position.y + dynamicCancelRange
            && pointerPos.y > transform.position.y - dynamicCancelRange)
        {
            _cancelIndicator.SetActive(true);
            HideAllChoices();
            return choiceIndex;
        }
        else
        {
            _cancelIndicator.SetActive(false);
        }

        // set angle step
        float angleStep = 360f / _choiceList.Count;

        // Convert to negative angles
        if (angle > 180f)
        {
            angle -= 360f;
        }

        // Adjust angle to shift the index range by half of angleStep
        angle += angleStep / 2f;

        // Convert negative angles to positive equivalent
        if (angle < 0f)
        {
            angle = 360f + angle;
        }

        // Calculate the index based on the adjusted angle
        choiceIndex = Mathf.FloorToInt(angle / angleStep);

        // Show selection visual
        ShowSelectionChoice(choiceIndex);

        return choiceIndex;
    }

    public void ShowSelectionChoice(int choiceIndex)
    {
        if (choiceIndex < 0 || choiceIndex >= _choiceList.Count)
            return;

        for (int i = 0; i < _choiceList.Count; i++)
        {
            if (i == choiceIndex)
                _choiceList[i].ShowHighlight();
            else
                _choiceList[i].HideHighlight();
        }
    }

    public void HideAllChoices()
    {
        for (int i = 0; i < _choiceList.Count; i++)
        {
            _choiceList[i].HideHighlight();
        }
    }
}
