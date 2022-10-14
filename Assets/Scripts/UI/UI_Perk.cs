using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Perk : MonoBehaviour
{
    [SerializeField] private PlayerSkillController _playerSkillController;
    [SerializeField] private Image _icon;
    [SerializeField] private Text _title;
    [SerializeField] private Text _description;
    [SerializeField] private Text _tier;
    private PlayerSkillController.Skills _whichSkill;

    public void SetPerkInfo(PlayerSkillController.Skills skills, int tier)
    {
        _whichSkill = skills;

        // set icon
        switch (_whichSkill)
        {
            case PlayerSkillController.Skills.Agility:
                _icon.sprite = ItemAssets.itemAssets.skill_Agility;
                _title.text = "Agility";
                switch (tier)
                {
                    case 1:
                        _description.text = "Increase base movement speed by 3";
                        _tier.text = "I";
                        break;
                    case 2:
                        _description.text = "Increase base movement speed by 5";
                        _tier.text = "II";
                        break;
                    case 3:
                        _description.text = "Increase base movement speed by 8";
                        _tier.text = "III";
                        break;
                }
                break;

            case PlayerSkillController.Skills.Regenaration:
                _icon.sprite = ItemAssets.itemAssets.skill_Regenaration;
                _title.text = "Regenaration";
                switch (tier)
                {
                    case 1:
                        _description.text = "Restore 5 HP every 5 sec";
                        _tier.text = "I";
                        break;
                    case 2:
                        _description.text = "Restore 8 HP every 5 sec";
                        _tier.text = "II";
                        break;
                    case 3:
                        _description.text = "Restore 12 HP every 5 sec";
                        _tier.text = "III";
                        break;
                }
                break;

            case PlayerSkillController.Skills.Sturdybody:
                _icon.sprite = ItemAssets.itemAssets.skill_Sturdybody;
                _title.text = "Sturdy Body";
                switch (tier)
                {
                    case 1:
                        _description.text = "Sturdybody1";
                        _tier.text = "I";
                        break;
                    case 2:
                        _description.text = "Sturdybody2";
                        _tier.text = "II";
                        break;
                    case 3:
                        _description.text = "Sturdybody3";
                        _tier.text = "III";
                        break;
                }
                break;
        }

        
    }

    public void LearnPerkSkill()
    {
        _playerSkillController.LearnSkill(_whichSkill);
    }
}
