using UnityEngine;
using UnityEngine.UI;

public class UI_PerkIconList : MonoBehaviour
{
    [SerializeField] private Transform PerkIconTemplate;
    [SerializeField] private Image Icon;
    [SerializeField] private Text TierText;

    public void AddNewAbilityIcon(PlayerSkillController.Skills whichSkill, int tier)
    {
        // check if skill icon already exists
        bool isIconExist = false;
        foreach (Transform icon in transform)
        {
            if (icon.name == "PerkIconTemplate")
            {
                continue;
            }

            isIconExist = icon.Find("Icon").GetComponent<Image>().sprite == ItemAssets.itemAssets.skillIconDic[whichSkill];

            if (isIconExist)
            {
                switch (tier)
                {
                    case 1:
                        icon.Find("TierText").GetComponent<Text>().text = "I";
                        break;

                    case 2:
                        icon.Find("TierText").GetComponent<Text>().text = "II";
                        break;

                    case 3:
                        icon.Find("TierText").GetComponent<Text>().text = "III";
                        break;
                }
                return;
            }
        }

        // if skill icon is new
        Icon.sprite = ItemAssets.itemAssets.skillIconDic[whichSkill];

        switch (tier)
        {
            case 1:
                TierText.text = "I"; 
                break;

            case 2:
                TierText.text = "II"; 
                break;

            case 3:
                TierText.text = "III"; 
                break;
        }

        Instantiate(PerkIconTemplate, transform).gameObject.SetActive(true);
    }
}
