using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGUI : MonoBehaviour
{
    public static DebugGUI debugGUI;

    private bool _isDisplaying;
    private string _debugTag;
    private float _x;
    private float _y;
    private float _w;
    private float _z;
    private GUIStyle _style;

    private void Awake()
    {
        if (debugGUI == null)
            debugGUI = this;

        _style = new GUIStyle();
        _style.fontSize = 20;
    }

    private void OnGUI()
    {
        if (_isDisplaying)
        {
            GUI.Label(new Rect(_x, _y, _w, _z), _debugTag, _style);
        }
    }

    public void ShowDebugTag(string context, float time, float x=10, float y=10, float z=20, float w=200)
    {
        _debugTag = context;
        _x = x;
        _y = y;
        _z = z;
        _w = w;
        StartCoroutine(Co_SetDisplayTimer(time));
    }

    IEnumerator Co_SetDisplayTimer(float time)
    {
        _isDisplaying = true;
        yield return new WaitForSecondsRealtime(time);
        _isDisplaying = false;
    }
}
