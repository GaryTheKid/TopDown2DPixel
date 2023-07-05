using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PingTier : MonoBehaviour
{
    [SerializeField] private Image tier1;
    [SerializeField] private Image tier2;
    [SerializeField] private Image tier3;
    [SerializeField] private Image tier4;

    [SerializeField] private Color tier1Col;
    [SerializeField] private Color tier2Col;
    [SerializeField] private Color tier3Col;
    [SerializeField] private Color tier4Col;

    /// <summary>
    /// Ping Tier:
    /// 4 -> (0~50ms)
    /// 3 -> (50~100ms)
    /// 2 -> (100~150ms)
    /// 1 -> (>=151ms)
    /// </summary>
    /// <param name="tier"></param>
    public void SetPingTier(byte tier)
    {
        switch (tier)
        {
            case 1:
                {
                    tier1.gameObject.SetActive(true);
                    tier2.gameObject.SetActive(false);
                    tier3.gameObject.SetActive(false);
                    tier4.gameObject.SetActive(false);

                    tier1.color = tier1Col;
                    break;
                }
            case 2:
                {
                    tier1.gameObject.SetActive(true);
                    tier2.gameObject.SetActive(true);
                    tier3.gameObject.SetActive(false);
                    tier4.gameObject.SetActive(false);

                    tier1.color = tier2Col;
                    tier2.color = tier2Col;
                    break;
                }
            case 3:
                {
                    tier1.gameObject.SetActive(true);
                    tier2.gameObject.SetActive(true);
                    tier3.gameObject.SetActive(true);
                    tier4.gameObject.SetActive(false);

                    tier1.color = tier3Col;
                    tier2.color = tier3Col;
                    tier3.color = tier3Col;
                    break;
                }
            case 4:
                {
                    tier1.gameObject.SetActive(true);
                    tier2.gameObject.SetActive(true);
                    tier3.gameObject.SetActive(true);
                    tier4.gameObject.SetActive(true);

                    tier1.color = tier4Col;
                    tier2.color = tier4Col;
                    tier3.color = tier4Col;
                    tier4.color = tier4Col;
                    break;
                }
        }
    }
}
