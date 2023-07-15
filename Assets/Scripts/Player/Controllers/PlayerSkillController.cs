using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using NetworkCalls;

public class PlayerSkillController : MonoBehaviour
{
    // all skill stats
    public const int HOLYSACRIFICE_DAMAGE_LV1 = 20;
    public const int HOLYSACRIFICE_DAMAGE_LV2 = 30;
    public const int HOLYSACRIFICE_DAMAGE_LV3 = 50;

    public const int STURDYBODY_MAXHP_BONUS_LV1 = 10;
    public const int STURDYBODY_MAXHP_BONUS_LV2 = 25;
    public const int STURDYBODY_MAXHP_BONUS_LV3 = 45;

    public const int AGILITY_SPEED_BOOST_LV1 = 2;
    public const int AGILITY_SPEED_BOOST_LV2 = 3;
    public const int AGILITY_SPEED_BOOST_LV3 = 5;

    public const int REGENERATION_HP_PER_SEC_LV1 = 3;
    public const int REGENERATION_HP_PER_SEC_LV2 = 5;
    public const int REGENERATION_HP_PER_SEC_LV3 = 8;

    // all skills
    public enum Skills
    {
        Sturdybody,
        Agility,
        Regenaration,
        HolySacrifice,
    }

    // skill ref
    public Dictionary<Skills, int> skillLvDic = new Dictionary<Skills, int>()
    {
        { Skills.Sturdybody, 0 },
        { Skills.Agility, 0 },
        { Skills.Regenaration, 0 },
        { Skills.HolySacrifice, 0 }
    };

    private PhotonView _PV;
    private PlayerStatsController _playerStatsController;
    private int TotalAvailablePerks;
    private int currPerks;
    private bool isPerkListGenerated;
    private List<UI_Perk> _perkList = new List<UI_Perk>();

    [SerializeField] private UI_PerkIconList UI_PerkIconList;
    [SerializeField] private Transform UI_PerkMenuButton;
    [SerializeField] private Transform UI_PerkMenu;
    [SerializeField] private Transform UI_PerkTemplate;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _playerStatsController = GetComponent<PlayerStatsController>();
        TotalAvailablePerks = skillLvDic.Count * 3;
    }

    public void AddSkillPerk()
    {
        if (currPerks < TotalAvailablePerks)
            currPerks++;
    }

    public void ApplySkillPerk()
    {
        if (currPerks > 0)
        {
            currPerks--;
            TotalAvailablePerks--;
        }
    }

    // display the UI of the skills which can be learned
    public void HideOrShowSkillUpgradeOptions()
    {
        if (UI_PerkMenu.gameObject.activeInHierarchy)
        {
            UI_PerkMenu.gameObject.SetActive(false);
        }
        else
        {
            UI_PerkMenu.gameObject.SetActive(true);

            // check if perk menu is previously generated
            if (isPerkListGenerated)
            {
                return;
            }

            // prepare random perks
            List<int> randPerks = new List<int>();
            for (int j = 0; j < skillLvDic.Count; j++)
            {
                randPerks.Add(j);
            }
            for (int j = 0; j < skillLvDic.Count; j++)
            {
                int randIndex = Random.Range(0, skillLvDic.Count);
                int randEl = randPerks[randIndex];
                randPerks.RemoveAt(randIndex);
                randPerks.Add(randEl);
            }

            for (int i = 0; i < 4; i++)
            {
                // set perk info
                if (i < randPerks.Count && skillLvDic[(Skills)randPerks[i]] < 3)
                {
                    // instantiate
                    Transform perk = Instantiate(UI_PerkTemplate, UI_PerkMenu);
                    perk.gameObject.SetActive(true);

                    UI_Perk ui_perk = perk.GetComponent<UI_Perk>();
                    ui_perk.SetPerkInfo((Skills)randPerks[i], skillLvDic[(Skills)randPerks[i]] + 1);
                    _perkList.Add(ui_perk);
                }
            }

            isPerkListGenerated = true;
        }
    }

    public void LearnSkill(Skills whichSkill)
    {
        // max level is 3
        if (skillLvDic[whichSkill] >= 3)
        {
            return;
        }

        skillLvDic[whichSkill]++;
        ApplySkillPerk();
        UpdateSkills(whichSkill);

        // clear previously generated perk list
        foreach (UI_Perk ui_perk in _perkList)
        {
            Destroy(ui_perk.gameObject);
        }
        _perkList.Clear();
        UI_PerkMenu.gameObject.SetActive(false);
        isPerkListGenerated = false;
    }

    public void SetSkill(Skills whichSkill, int level)
    {
        skillLvDic[whichSkill] = level;
        UpdateSkills(whichSkill);
    }

    private void UpdateSkills(Skills whichSkill)
    {
        switch (whichSkill)
        {
            case Skills.Sturdybody: 
                switch (skillLvDic[Skills.Sturdybody])
                {
                    case 0:
                        break;
                    case 1:
                        Skill_Network.SturdyBody(_PV, STURDYBODY_MAXHP_BONUS_LV1);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.SturdyBody(_PV, STURDYBODY_MAXHP_BONUS_LV2);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.SturdyBody(_PV, STURDYBODY_MAXHP_BONUS_LV3);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 3);
                        break;
                } break;

            case Skills.Agility:
                switch (skillLvDic[Skills.Agility])
                {
                    case 0:
                        break;
                    case 1:
                        _playerStatsController.playerStats.baseSpeed += AGILITY_SPEED_BOOST_LV1;
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        _playerStatsController.playerStats.baseSpeed += AGILITY_SPEED_BOOST_LV2;
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        _playerStatsController.playerStats.baseSpeed += AGILITY_SPEED_BOOST_LV3;
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 3);
                        break;
                }
                break;

            case Skills.Regenaration:
                switch (skillLvDic[Skills.Regenaration])
                {
                    case 0:
                        break;
                    case 1:
                        Skill_Network.Regeneration(_PV, 5);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, REGENERATION_HP_PER_SEC_LV1);
                        break;
                    case 2:
                        Skill_Network.Regeneration(_PV, 8);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, REGENERATION_HP_PER_SEC_LV2);
                        break;
                    case 3:
                        Skill_Network.Regeneration(_PV, 12);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, REGENERATION_HP_PER_SEC_LV3);
                        break;
                }
                break;

            case Skills.HolySacrifice:
                switch (skillLvDic[Skills.HolySacrifice])
                {
                    case 0:
                        break;
                    case 1:
                        Skill_Network.Regeneration(_PV, 5);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.Regeneration(_PV, 8);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.Regeneration(_PV, 12);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 3);
                        break;
                }
                break;
        }
    }
}
