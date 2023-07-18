using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using NetworkCalls;

public class PlayerSkillController : MonoBehaviour
{
    // all skill stats
    public const int STURDYBODY_MAXHP_BONUS_LV1 = 10;
    public const int STURDYBODY_MAXHP_BONUS_LV2 = 15;
    public const int STURDYBODY_MAXHP_BONUS_LV3 = 25;

    public const int AGILITY_SPEED_BOOST_LV1 = 2;
    public const int AGILITY_SPEED_BOOST_LV2 = 3;
    public const int AGILITY_SPEED_BOOST_LV3 = 5;

    public const int REGENERATION_HP_LV1 = 3;
    public const int REGENERATION_HP_LV2 = 5;
    public const int REGENERATION_HP_LV3 = 8;

    public const float HOLYSACRIFICE_DAMAGE_LV1 = 20f;
    public const float HOLYSACRIFICE_DAMAGE_LV2 = 30f;
    public const float HOLYSACRIFICE_DAMAGE_LV3 = 50f;

    public const float SECONDLIFE_RESPAWN_BOOST_LV1 = 0.5f;
    public const float SECONDLIFE_RESPAWN_BOOST_LV2 = 1f;
    public const float SECONDLIFE_RESPAWN_BOOST_LV3 = 2f;

    public const int PIGGYBANK_GOLD_GENERATION_LV1 = 10;
    public const int PIGGYBANK_GOLD_GENERATION_LV2 = 25;
    public const int PIGGYBANK_GOLD_GENERATION_LV3 = 45;

    public const float EAGLEEYES_VISION_BONUS_LV1 = 2;
    public const float EAGLEEYES_VISION_BONUS_LV2 = 3;
    public const float EAGLEEYES_VISION_BONUS_LV3 = 5;

    // all skills
    public enum Skills
    {
        Sturdybody,
        Agility,
        Regenaration,
        HolySacrifice,
        SecondLife,
        PiggyBank,
        EagleEyes,
        Learning
    }

    // skill ref
    public Dictionary<Skills, int> skillLvDic = new Dictionary<Skills, int>()
    {
        { Skills.Sturdybody, 0 },
        { Skills.Agility, 0 },
        { Skills.Regenaration, 0 },
        { Skills.HolySacrifice, 0 },
        { Skills.SecondLife, 0 },
        { Skills.PiggyBank, 0 },
        { Skills.EagleEyes, 0 }
    };

    // skill texts: title, description lv1, description lv2, description lv3
    public Dictionary<Skills, (string, string, string, string)> skillDescriptionDic = new Dictionary<Skills, (string, string, string, string)>()
    {
        { Skills.Sturdybody, 
            ("Sturdy Body", 
            "Increase max HP by " + STURDYBODY_MAXHP_BONUS_LV1,
            "Increase max HP by " + STURDYBODY_MAXHP_BONUS_LV2,
            "Increase max HP by " + STURDYBODY_MAXHP_BONUS_LV3)
        },
        { Skills.Agility, 
            ("Agility",
            "Increase base movement speed by " + AGILITY_SPEED_BOOST_LV1,
            "Increase base movement speed by " + AGILITY_SPEED_BOOST_LV2,
            "Increase base movement speed by " + AGILITY_SPEED_BOOST_LV3) 
        },
        { Skills.Regenaration, 
            ("Regenaration",
            "Restore " + REGENERATION_HP_LV1 + " HP every 5 sec",
            "Restore " + REGENERATION_HP_LV2 + " HP every 5 sec",
            "Restore " + REGENERATION_HP_LV2 + " HP every 5 sec") 
        },
        { Skills.HolySacrifice, 
            ("Holy Sacrifice",
            "Deal " + HOLYSACRIFICE_DAMAGE_LV1 + " damage around you upon death",
            "Deal " + HOLYSACRIFICE_DAMAGE_LV2 + " damage around you upon death",
            "Deal " + HOLYSACRIFICE_DAMAGE_LV3 + " damage around you upon death") 
        },
        { Skills.SecondLife,
            ("Second Life",
            "Respawn time reduces by " + SECONDLIFE_RESPAWN_BOOST_LV1 + " sec",
            "Respawn time reduces by " + SECONDLIFE_RESPAWN_BOOST_LV2 + " sec",
            "Respawn time reduces by " + SECONDLIFE_RESPAWN_BOOST_LV3 + " sec")
        },
        { Skills.PiggyBank,
            ("Piggy Bank",
            "Generate " + PIGGYBANK_GOLD_GENERATION_LV1 + " gold every 10 sec",
            "Generate " + PIGGYBANK_GOLD_GENERATION_LV2 + " gold every 10 sec",
            "Generate " + PIGGYBANK_GOLD_GENERATION_LV3 + " gold every 10 sec")
        },
        { Skills.EagleEyes,
            ("Eagle Eyes",
            "Increase vision sight by " + EAGLEEYES_VISION_BONUS_LV1,
            "Increase vision sight by " + EAGLEEYES_VISION_BONUS_LV2,
            "Increase vision sight by " + EAGLEEYES_VISION_BONUS_LV3)
        },
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
    [SerializeField] private int _displayPerkCount;
    [SerializeField] private int _maxPerkLearningCount;

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
                if (skillLvDic[(Skills)j] < 3)
                {
                    randPerks.Add(j);
                }
            }

            // Shuffle the list randomly
            System.Random random = new System.Random();
            for (int i = 0; i < randPerks.Count - 1; i++)
            {
                int randomIndex = random.Next(i, randPerks.Count);
                int temp = randPerks[i];
                randPerks[i] = randPerks[randomIndex];
                randPerks[randomIndex] = temp;
            }

            // check if reach the max learning count, if so, can only learn perks >= lv2
            int learntPerkCount = 0;
            foreach (int perk in randPerks)
            {
                if (skillLvDic[(Skills)perk] > 1)
                {
                    learntPerkCount++;
                }
            }
            if (learntPerkCount >= _maxPerkLearningCount)
            {
                // Remove all level 1 perks from the list
                randPerks.RemoveAll(perk => skillLvDic[(Skills)perk] <= 1);
            }


            for (int i = 0; i < _displayPerkCount; i++)
            {
                // set perk info
                if (i < randPerks.Count)
                {
                    // instantiate
                    Transform perk = Instantiate(UI_PerkTemplate, UI_PerkMenu);
                    perk.gameObject.SetActive(true);

                    // get texts
                    UI_Perk ui_perk = perk.GetComponent<UI_Perk>();
                    Skills whichSkill = (Skills)randPerks[i];
                    int skillLeveltier = skillLvDic[(Skills)randPerks[i]] + 1;

                    Sprite icon = ItemAssets.itemAssets.skillIconDic[whichSkill];
                    string titleText = skillDescriptionDic[whichSkill].Item1;
                    string tierText = "";
                    string descriptionText = "";

                    switch(skillLeveltier)
                    {
                        case 1:
                            tierText = "I";
                            descriptionText = skillDescriptionDic[whichSkill].Item2;
                            break;
                        case 2:
                            tierText = "II";
                            descriptionText = skillDescriptionDic[whichSkill].Item3;
                            break;
                        case 3:
                            tierText = "III";
                            descriptionText = skillDescriptionDic[whichSkill].Item4;
                            break;
                    }

                    ui_perk.SetPerkInfo((Skills)randPerks[i], icon, titleText, tierText, descriptionText);
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
                        Skill_Network.Regeneration(_PV, REGENERATION_HP_LV1);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.Regeneration(_PV, REGENERATION_HP_LV2);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.Regeneration(_PV, REGENERATION_HP_LV3);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 3);
                        break;
                }
                break;

            case Skills.HolySacrifice:
                switch (skillLvDic[Skills.HolySacrifice])
                {
                    case 0:
                        break;
                    case 1:
                        Skill_Network.HolySacrifice(_PV, HOLYSACRIFICE_DAMAGE_LV1);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.HolySacrifice(_PV, HOLYSACRIFICE_DAMAGE_LV2);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.HolySacrifice(_PV, HOLYSACRIFICE_DAMAGE_LV3);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 3);
                        break;
                }
                break;

            case Skills.SecondLife:
                switch (skillLvDic[Skills.SecondLife])
                {
                    case 0:
                        break;
                    case 1:
                        Skill_Network.SecondLife(_PV, SECONDLIFE_RESPAWN_BOOST_LV1);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.SecondLife(_PV, SECONDLIFE_RESPAWN_BOOST_LV2);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.SecondLife(_PV, SECONDLIFE_RESPAWN_BOOST_LV3);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 3);
                        break;
                }
                break;

            case Skills.PiggyBank:
                switch (skillLvDic[Skills.PiggyBank])
                {
                    case 0:
                        break;
                    case 1:
                        Skill_Network.PiggyBank(_PV, PIGGYBANK_GOLD_GENERATION_LV1);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.PiggyBank(_PV, PIGGYBANK_GOLD_GENERATION_LV2);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.PiggyBank(_PV, PIGGYBANK_GOLD_GENERATION_LV3);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 3);
                        break;
                }
                break;

            case Skills.EagleEyes:
                switch (skillLvDic[Skills.EagleEyes])
                {
                    case 0:
                        break;
                    case 1:
                        Skill_Network.EagleEyes(_PV, EAGLEEYES_VISION_BONUS_LV1);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.EagleEyes(_PV, EAGLEEYES_VISION_BONUS_LV2);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.EagleEyes(_PV, EAGLEEYES_VISION_BONUS_LV3);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 3);
                        break;
                }
                break;
        }
    }
}
