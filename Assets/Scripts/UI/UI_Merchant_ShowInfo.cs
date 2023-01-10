using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Merchant_ShowInfo : MonoBehaviour
{
    private const float INFO_EXIST_TIME = 1.5f;

    private void OnEnable()
    {
        StartCoroutine(Co_InfoFadeAway());
    }

    private IEnumerator Co_InfoFadeAway()
    {
        yield return new WaitForSecondsRealtime(INFO_EXIST_TIME);
        gameObject.SetActive(false);
    }
}
