using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public short uiIndex;
    public KeyCode keyCode = KeyCode.None;

    public virtual void OnDrop(PointerEventData eventData)
    {
        // TODO: On Drop problem
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
