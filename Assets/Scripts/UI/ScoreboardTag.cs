using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardTag : MonoBehaviour
{
    [SerializeField] private Text _ui_playerID;
    [SerializeField] private Text _ui_score;

    private int score;

    public string GetID()
    {
        return _ui_playerID.text;
    }

    public void SetID(string ID)
    {
        _ui_playerID.text = ID;
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
