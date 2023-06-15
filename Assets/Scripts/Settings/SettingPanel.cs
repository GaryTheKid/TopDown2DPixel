using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    public static SettingPanel singleton;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }


        if (singleton != null && singleton != this)
        {
            Destroy(singleton.gameObject);
        }
    }
}
