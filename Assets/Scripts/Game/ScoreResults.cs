using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreResults : MonoBehaviour
{
    [SerializeField] private Dictionary<int, int> scoreResultDic;

    private void Awake()
    {
        scoreResultDic = new Dictionary<int, int>();
        DontDestroyOnLoad(gameObject);
    }

    public void AddNewPlayer(int actorNumber)
    {
        scoreResultDic.Add(actorNumber, 0);
    }

    public void AddScoreToPlayer(int actorNumber)
    {
        if (scoreResultDic.ContainsKey(actorNumber))
        {
            scoreResultDic[actorNumber]++;
        }
    }

    public void AddScoreToPlayer(int actorNumber, int deltaScore)
    {
        if (scoreResultDic.ContainsKey(actorNumber))
        {
            scoreResultDic[actorNumber] += deltaScore;
        }
    }

    public List<int> GetTop3Players()
    {
        List<int> topPlayers = new List<int>();

        // Sort the dictionary by score in descending order
        var sortedPlayers = scoreResultDic.OrderByDescending(x => x.Value);

        // Retrieve the top 3 players' actorNumbers
        int count = 0;
        foreach (var player in sortedPlayers)
        {
            topPlayers.Add(player.Key);
            count++;

            // Break the loop after getting the top 3 players
            if (count >= 3)
                break;
        }

        return topPlayers;
    }
}
