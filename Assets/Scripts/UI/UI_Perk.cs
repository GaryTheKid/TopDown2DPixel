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

    public void SetPerkInfo(PlayerSkillController.Skills skills, Sprite icon, string titleText, string tierText, string descriptionText)
    {
        _whichSkill = skills;
        _title.text = titleText;
        _tier.text = tierText;
        _description.text = descriptionText;
        _icon.sprite = icon;
    }

    public void LearnPerkSkill()
    {
        _playerSkillController.LearnSkill(_whichSkill);
    }
}
