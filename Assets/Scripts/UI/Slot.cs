using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public short uiIndex;
    public KeyCode keyCode = KeyCode.None;

    [SerializeField] private Text shortCutText;

    private void Start()
    {
        if (keyCode != KeyCode.None)
            shortCutText.text = keyCode.ToString().Replace("Alpha", "");
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        // TODO: On Drop problem
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }

    public void UpdateKeyCode(KeyCode newKeyCode)
    {
        keyCode = newKeyCode;
        shortCutText.text = keyCode.ToString();
    }
}
