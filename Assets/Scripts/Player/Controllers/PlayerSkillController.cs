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
    public const float STURDYBODY_MAXHP_BONUS_MAX_BONUS = 0.1f;

    public const int AGILITY_SPEED_BOOST_LV1 = 2;
    public const int AGILITY_SPEED_BOOST_LV2 = 3;
    public const int AGILITY_SPEED_BOOST_LV3 = 5;
    public const float AGILITY_SPEED_BOOST_MAX_BONUS = 0.25f;

    public const int REGENERATION_HP_LV1 = 3;
    public const int REGENERATION_HP_LV2 = 5;
    public const int REGENERATION_HP_LV3 = 8;
    public const float REGENERATION_HP_MISSING_HEALTH_BONUS = 0.05f;

    public const float HOLYSACRIFICE_DAMAGE_LV1 = 20f;
    public const float HOLYSACRIFICE_DAMAGE_LV2 = 30f;
    public const float HOLYSACRIFICE_DAMAGE_LV3 = 50f;
    public const float HOLYSACRIFICE_SIZE_BONUS = 0.4f;

    public const float SECONDLIFE_RESPAWN_BOOST_LV1 = 0.5f;
    public const float SECONDLIFE_RESPAWN_BOOST_LV2 = 1f;
    public const float SECONDLIFE_RESPAWN_BOOST_LV3 = 2f;
    public const float SECONDLIFE_RESPAWN_SPEEDBOOST_AMOUNT = 10f;
    public const float SECONDLIFE_RESPAWN_SPEEDBOOST_TIME = 3f;

    public const int PIGGYBANK_GOLD_GENERATION_LV1 = 10;
    public const int PIGGYBANK_GOLD_GENERATION_LV2 = 25;
    public const int PIGGYBANK_GOLD_GENERATION_LV3 = 45;
    public const float PIGGYBANK_GOLD_INTEREST_BONUS = 0.05f;

    public const float EAGLEEYES_VISION_BONUS_LV1 = 2;
    public const float EAGLEEYES_VISION_BONUS_LV2 = 3;
    public const float EAGLEEYES_VISION_BONUS_LV3 = 4;

    public const float LEARNING_EXP_GAIN_MODIFIER_LV1 = 1.1f;
    public const float LEARNING_EXP_GAIN_MODIFIER_LV2 = 1.25f;
    public const float LEARNING_EXP_GAIN_MODIFIER_LV3 = 1.5f;

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
        { Skills.EagleEyes, 0 },
        { Skills.Learning, 0 }
    };

    // skill texts: title, description lv1, description lv2, description lv3
    public Dictionary<Skills, (string, string, string, string)> skillDescriptionDic = new Dictionary<Skills, (string, string, string, string)>()
    {
        { Skills.Sturdybody, 
            ("Sturdy Body",
            "Increase max HP by additional " + STURDYBODY_MAXHP_BONUS_LV1,
            "Increase max HP by additional " + STURDYBODY_MAXHP_BONUS_LV2,
            "Increase max HP by additional " + STURDYBODY_MAXHP_BONUS_LV3 + " + " + (STURDYBODY_MAXHP_BONUS_MAX_BONUS * 100f) + "% of your max HP")
        },
        { Skills.Agility, 
            ("Agility",
            "Increase base movement speed by " + AGILITY_SPEED_BOOST_LV1,
            "Increase base movement speed by " + AGILITY_SPEED_BOOST_LV2,
            "Increase base movement speed by " + AGILITY_SPEED_BOOST_LV3 + " and receive " + (AGILITY_SPEED_BOOST_MAX_BONUS * 100f) + "% more speed boost effect") 
        },
        { Skills.Regenaration, 
            ("Regenaration",
            "Restore " + REGENERATION_HP_LV1 + " HP every 5 sec",
            "Restore " + REGENERATION_HP_LV2 + " HP every 5 sec",
            "Restore " + REGENERATION_HP_LV2 + " HP every 5 sec, if out of combat, restore "+ (REGENERATION_HP_MISSING_HEALTH_BONUS * 100f) + "% missing health as well") 
        },
        { Skills.HolySacrifice, 
            ("Holy Sacrifice",
            "Deal " + HOLYSACRIFICE_DAMAGE_LV1 + " damage around you upon death",
            "Deal " + HOLYSACRIFICE_DAMAGE_LV2 + " damage around you upon death",
            "Deal " + HOLYSACRIFICE_DAMAGE_LV3 + " damage around you upon death, damage radius increase by " + (HOLYSACRIFICE_SIZE_BONUS * 100f) + "%") 
        },
        { Skills.SecondLife,
            ("Second Life",
            "Respawn time reduces by " + SECONDLIFE_RESPAWN_BOOST_LV1 + " sec",
            "Respawn time reduces by " + SECONDLIFE_RESPAWN_BOOST_LV2 + " sec",
            "Respawn time reduces by " + SECONDLIFE_RESPAWN_BOOST_LV3 + " sec, on respawn, gain a temporary speed boost")
        },
        { Skills.PiggyBank,
            ("Piggy Bank",
            "Generate " + PIGGYBANK_GOLD_GENERATION_LV1 + " gold every 10 sec",
            "Generate " + PIGGYBANK_GOLD_GENERATION_LV2 + " gold every 10 sec",
            "Generate " + PIGGYBANK_GOLD_GENERATION_LV3 + " gold plus " + (PIGGYBANK_GOLD_INTEREST_BONUS * 100f) + "% of interest every 10 sec")
        },
        { Skills.EagleEyes,
            ("Eagle Eyes",
            "Increase vision sight by additional " + EAGLEEYES_VISION_BONUS_LV1,
            "Increase vision sight by additional " + EAGLEEYES_VISION_BONUS_LV2,
            "Increase vision sight by additional " + EAGLEEYES_VISION_BONUS_LV3 + ", vision will not be affected by night")
        },
        { Skills.Learning,
            ("Learning",
            "Gain " + ((LEARNING_EXP_GAIN_MODIFIER_LV1 - 1f) * 100) + "% extra xp",
            "Gain " + ((LEARNING_EXP_GAIN_MODIFIER_LV2 - 1f) * 100) + "% extra xp",
            "Gain " + ((LEARNING_EXP_GAIN_MODIFIER_LV3 - 1f) * 100) + "% extra xp, and can learn 1 additional talent")
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

    public void IncrementMaxSkillSlot()
    {
        _maxPerkLearningCount++;
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
                        Skill_Network.SturdyBody(_PV, false, STURDYBODY_MAXHP_BONUS_LV1);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.SturdyBody(_PV, false, STURDYBODY_MAXHP_BONUS_LV2);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.SturdyBody(_PV, true, STURDYBODY_MAXHP_BONUS_LV3 + (int)((_playerStatsController.playerStats.maxHp + STURDYBODY_MAXHP_BONUS_LV3) * STURDYBODY_MAXHP_BONUS_MAX_BONUS));
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
                        _playerStatsController.playerStats.speedBoostModifier += 0.25f;
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
                        Skill_Network.Regeneration(_PV, false, REGENERATION_HP_LV1, REGENERATION_HP_MISSING_HEALTH_BONUS);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.Regeneration(_PV, false, REGENERATION_HP_LV2, REGENERATION_HP_MISSING_HEALTH_BONUS);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.Regeneration(_PV, true, REGENERATION_HP_LV3, REGENERATION_HP_MISSING_HEALTH_BONUS);
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
                        Skill_Network.HolySacrifice(_PV, false, HOLYSACRIFICE_DAMAGE_LV1, HOLYSACRIFICE_SIZE_BONUS);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.HolySacrifice(_PV, false, HOLYSACRIFICE_DAMAGE_LV2, HOLYSACRIFICE_SIZE_BONUS);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.HolySacrifice(_PV, true, HOLYSACRIFICE_DAMAGE_LV3, HOLYSACRIFICE_SIZE_BONUS);
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
                        Skill_Network.SecondLife(_PV, false, SECONDLIFE_RESPAWN_BOOST_LV1, SECONDLIFE_RESPAWN_SPEEDBOOST_AMOUNT, SECONDLIFE_RESPAWN_SPEEDBOOST_TIME);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.SecondLife(_PV, false, SECONDLIFE_RESPAWN_BOOST_LV2, SECONDLIFE_RESPAWN_SPEEDBOOST_AMOUNT, SECONDLIFE_RESPAWN_SPEEDBOOST_TIME);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.SecondLife(_PV, true, SECONDLIFE_RESPAWN_BOOST_LV3, SECONDLIFE_RESPAWN_SPEEDBOOST_AMOUNT, SECONDLIFE_RESPAWN_SPEEDBOOST_TIME);
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
                        Skill_Network.PiggyBank(_PV, false, PIGGYBANK_GOLD_GENERATION_LV1, PIGGYBANK_GOLD_INTEREST_BONUS);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.PiggyBank(_PV, false, PIGGYBANK_GOLD_GENERATION_LV2, PIGGYBANK_GOLD_INTEREST_BONUS);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.PiggyBank(_PV, true, PIGGYBANK_GOLD_GENERATION_LV3, PIGGYBANK_GOLD_INTEREST_BONUS);
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
                        Skill_Network.EagleEyes(_PV, false, EAGLEEYES_VISION_BONUS_LV1);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.EagleEyes(_PV, false, EAGLEEYES_VISION_BONUS_LV2);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.EagleEyes(_PV, true, EAGLEEYES_VISION_BONUS_LV3);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 3);
                        break;
                }
                break;

            case Skills.Learning:
                switch (skillLvDic[Skills.Learning])
                {
                    case 0:
                        break;
                    case 1:
                        Skill_Network.Learning(_PV, false, LEARNING_EXP_GAIN_MODIFIER_LV1);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 1);
                        break;
                    case 2:
                        Skill_Network.Learning(_PV, false, LEARNING_EXP_GAIN_MODIFIER_LV2);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 2);
                        break;
                    case 3:
                        Skill_Network.Learning(_PV, true, LEARNING_EXP_GAIN_MODIFIER_LV3);
                        UI_PerkIconList.AddNewAbilityIcon(whichSkill, 3);
                        break;
                }
                break;
        }
    }
}
