using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameResultManager : MonoBehaviour
{
    [SerializeField] private PlayerResultController[] _results;
    [SerializeField] private Text _ping;
    [SerializeField] private GameObject _fadeOut;
    [SerializeField] private GameObject _transitionCurtain;
    private ScoreResults scoreResults;

    private void Start()
    {
        // get the results from the game scene
        scoreResults = GameObject.Find("ScoreResults").GetComponent<ScoreResults>();

        // update the results in characters
        List<int> top3 = scoreResults.GetTop3Players();
        for (int i = 0; i < top3.Count; i++)
        {
            _results[i].SetPlayerInfo((byte)top3[i]);
        }
    }

    private void Update()
    {
        // show pin
        _ping.text = PhotonNetwork.GetPing().ToString() + "ms";
    }

    public void ReturnToMenuScene()
    {
        PhotonNetwork.Disconnect();
        StartCoroutine(LoadGameSceneSmoothly(Networking_GameSettings.singleton.menuSceneIndex));
    }

    private IEnumerator LoadGameSceneSmoothly(int sceneIndex)
    {
        _fadeOut.SetActive(true);
        yield return new WaitUntil(() => { return !_fadeOut.activeInHierarchy; });
        _transitionCurtain.SetActive(true);
        PhotonNetwork.LoadLevel(sceneIndex);
    }
}
