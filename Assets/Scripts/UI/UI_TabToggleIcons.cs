using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_TabToggleIcons : MonoBehaviour
{
    [SerializeField] private List<Image> icons;

    public void SetToggleIcon(int index)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            if (i == index)
            {
                var newCol = Color.gray;
                newCol.a = 0.5f;
                icons[i].color = newCol;
            }
            else
                icons[i].color = Color.white;
        }
    }
}
