using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cloud_Gem : MonoBehaviour
{
    [SerializeField] private Text gemText;

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

        var gem_Cloud = CloudCommunicator.singleton.gem;
        gemText.text = gem_Cloud.ToString();
        PlayerSettings.singleton.Gem = gem_Cloud;
    }
}
