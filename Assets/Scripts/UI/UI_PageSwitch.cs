using UnityEngine;

public class UI_PageSwitch : MonoBehaviour
{
    [SerializeField] private GameObject fadeInOutTransition;
    [SerializeField] private Canvas[] pageCanvases;

    public void SwitchToStartPage(int whichPage) 
    {
        fadeInOutTransition.SetActive(true);
        for (int i = 0; i < pageCanvases.Length; i++)
        {
            if (i == whichPage)
            {
                pageCanvases[i].sortingOrder = 0;
                foreach (Transform child in pageCanvases[i].transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
            else
            {
                pageCanvases[i].sortingOrder = -1;
                foreach (Transform child in pageCanvases[i].transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
