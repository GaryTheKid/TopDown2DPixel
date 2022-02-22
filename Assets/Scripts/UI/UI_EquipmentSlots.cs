using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipmentSlots : MonoBehaviour
{
    [SerializeField] private Transform equipmentSlotContainer;
    [SerializeField] private Transform equipmentSlotTemplate;
    private EquipmentSlots equipmentSlots;

    public void SetEquipmentSlots(EquipmentSlots equipmentSlots)
    {
        this.equipmentSlots = equipmentSlots;

        equipmentSlots.OnEquipmentListChanged += Inventory_OnEquipmentListChanged;
        UpdateEquipmentSlotsItems();
    }

    private void Inventory_OnEquipmentListChanged(object sender, System.EventArgs e)
    {
        UpdateEquipmentSlotsItems();
    }

    private void UpdateEquipmentSlotsItems()
    {
        foreach (Transform child in equipmentSlotContainer)
        {
            if (child == equipmentSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        float itemSlotCellSize = 120f;
        foreach (Item item in equipmentSlots.GetEquipmentList())
        {
            // instantiate item template
            RectTransform itemSlotRectTransform = Instantiate(equipmentSlotTemplate, equipmentSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            // set item ui image
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, 0f);
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            // set item ui text
            Text uiText = itemSlotRectTransform.Find("amountText").GetComponent<Text>();
            if (item.amount > 1)
            {
                uiText.text = item.amount.ToString();
            }
            else
            {
                uiText.text = "";
            }

            // new line
            x++;
        }
    }
}
