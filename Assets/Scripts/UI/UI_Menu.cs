using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuBackground;

    public void SwitchMenu()
    {
        if (menuBackground.activeInHierarchy)
            menuBackground.SetActive(false);
        else
            menuBackground.SetActive(true);
    }

    public void QuitGame()
    {
        GameManager.singleton.LeaveRoom();
        Application.Quit();
    }
}
