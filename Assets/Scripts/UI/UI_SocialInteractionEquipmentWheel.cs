using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SocialInteractionEquipmentWheel : MonoBehaviour
{
    [SerializeField] private List<UI_SocialInteractionEquipmentSlot> slots;

    public void EquipSlot(int slotIndex, int optionIndex)
    {
        // check if option already in slot
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].socialInteractionIndex == optionIndex)
            {
                UnequipSlot(i);
                break;
            }
        }

        // equip option to the new slot
        slots[slotIndex].Equip(optionIndex);

        // update PlayerSettings.singleton.PlayerSocialIndexList

    }

    public void UnequipSlot(int slotIndex)
    {
        slots[slotIndex].Unequip();
    }

    public void DisableSelectedSlotInteraction(int slotIndex)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slotIndex == i)
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    public void FreeAllSlotsInteraction()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].GetComponent<Button>().interactable = true;
        }
    }

    public bool IsSlotEmpty(int index)
    {
        if (slots[index].socialInteractionIndex == -1)
            return true;
        else
            return false;
    }

    public int[] GetSlotOptionList()
    {
        int[] retList = new int[slots.Count];

        for (int i = 0; i < slots.Count; i++)
        {
            retList[i] = slots[i].socialInteractionIndex;
        }

        return retList;
    }
}
