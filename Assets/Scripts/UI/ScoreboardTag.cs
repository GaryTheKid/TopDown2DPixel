using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardTag : MonoBehaviour
{
    // UI
    [SerializeField] private Text _ui_playerID;
    [SerializeField] private Text _ui_score;
    [SerializeField] private Text _ui_KDA;
    [SerializeField] private Image _ui_tagColor;
    [SerializeField] private UI_PingTier _ui_pingTier;

    [SerializeField] private int _actorNumber;
    
    private int score;

    public int GetActorNumber()
    {
        return _actorNumber;
    }

    public void SetActorNumber(int actorNumber)
    {
        _actorNumber = actorNumber;
    }

    public string GetID()
    {
        return _ui_playerID.text;
    }

    public void SetID(string ID)
    {
        _ui_playerID.text = ID;
    }

    public void SetTagColor(byte index) // 0: me, 1: ally, 2: enemy
    {
        switch (index)
        {
            case 0:
                _ui_tagColor.color = GameManager.singleton.myColor;
                break;
            case 1:
                _ui_tagColor.color = GameManager.singleton.allyColor;
                break;
            case 2:
                _ui_tagColor.color = GameManager.singleton.enemyColor;
                break;
        }
    }

    public void SetPingTier(byte tier)
    {
        _ui_pingTier.SetPingTier(tier);
    }

    public void SetKDA(byte kill, byte death)
    {
        _ui_KDA.text = string.Format("{0}/{1}", kill, death);
    }

    public void AddScore(int score)
    {
        this.score += score;
        _ui_score.text = this.score.ToString();
    }

    public void LoseScore(int score)
    {
        this.score -= score;
        _ui_score.text = this.score.ToString();
    }

    public void SetScore(int score)
    {
        this.score = score;
        _ui_score.text = this.score.ToString();
    }
}
