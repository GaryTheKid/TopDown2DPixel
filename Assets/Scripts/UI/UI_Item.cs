using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Item : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<int, int> OnChangeItemUIIndex;
    public Action<int> OnDragOutSideUI;
    public Action OnUsingUI;
    public Action OnFinishUsingUI;
    public Slot currentSlot;
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;

    [SerializeField] private Canvas _canvas;
    private Vector2 _initialPos;
    private IEnumerator _showItemInfo_co;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnUsingUI?.Invoke();
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;     
        _initialPos = rectTransform.anchoredPosition;
        DisableOthersBlockRaycast();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // move along the mouse position
        rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnFinishUsingUI?.Invoke();
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        GameObject target = eventData.pointerEnter;
        EnableOthersBlockRaycast();

        // drag outside the UI
        if (target == null)
        {
            OnDragOutSideUI?.Invoke(currentSlot.uiIndex);
            return;
        }

        // cannot be dropped outside the inventory bound
        if(!target.CompareTag("slot"))
        {
            rectTransform.anchoredPosition = _initialPos;
            return;
        }

        // can only be dropped into a slot
        Slot targetSlot = target.GetComponentInParent<Slot>();
        if (targetSlot == null)
        {
            rectTransform.anchoredPosition = _initialPos;
            return;
        }
        // if drop into a new slot
        else if (targetSlot != currentSlot)
        {
            int currIndex = currentSlot.uiIndex;
            int targetIndex = targetSlot.uiIndex;
            OnChangeItemUIIndex?.Invoke(currIndex, targetIndex);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // start dragging
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_showItemInfo_co == null)
        {
            _showItemInfo_co = Co_ShowItemInfo();
            StartCoroutine(_showItemInfo_co);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_showItemInfo_co != null)
        {
            StopCoroutine(_showItemInfo_co);
            _showItemInfo_co = null;
        }
    }
    IEnumerator Co_ShowItemInfo()
    {
        yield return new WaitForSecondsRealtime(2f);
        print(123);
    }

    private void DisableOthersBlockRaycast()
    {
        foreach (UI_Item child in transform.parent.GetComponentsInChildren<UI_Item>())
        {
            if (child == this)
                continue;
            child.canvasGroup.blocksRaycasts = false;
        }
    }

    private void EnableOthersBlockRaycast()
    {
        foreach (UI_Item child in transform.parent.GetComponentsInChildren<UI_Item>())
        {
            if (child == this)
                continue;
            child.canvasGroup.blocksRaycasts = true;
        }
    }
}
