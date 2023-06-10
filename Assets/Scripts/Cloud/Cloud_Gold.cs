using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cloud_Gold : MonoBehaviour
{
    [SerializeField] private Text goldText;

    private void Start()
    {
        StartCoroutine(GetCloud_SelectCharacter());
    }

    private IEnumerator GetCloud_SelectCharacter()
    {
        yield return new WaitUntil(() =>
        {
            return CloudCommunicator.singleton.hasDataSynced;
        });

        var gold_Cloud = CloudCommunicator.singleton.gold;
        goldText.text = gold_Cloud.ToString();
        PlayerSettings.singleton.Gold = gold_Cloud;
    }
}
