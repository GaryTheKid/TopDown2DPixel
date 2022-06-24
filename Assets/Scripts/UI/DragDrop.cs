using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<int, int> OnChangeItemUIIndex;
    public Action<int> OnDragOutSideUI;
    public Action OnUsingUI;
    public Action OnFinishUsingUI;
    public Slot currentSlot;
    public RectTransform _rectTransform;
    public CanvasGroup _canvasGroup;

    [SerializeField] private Canvas _canvas;
    private Vector2 _initialPos;
    private IEnumerator _showItemInfo_co;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnUsingUI?.Invoke();
        _canvasGroup.alpha = 0.5f;
        _canvasGroup.blocksRaycasts = false;     
        _initialPos = _rectTransform.anchoredPosition;
        DisableOthersBlockRaycast();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // move along the mouse position
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnFinishUsingUI?.Invoke();
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
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
            _rectTransform.anchoredPosition = _initialPos;
            return;
        }

        // can only be dropped into a slot
        Slot targetSlot = target.GetComponentInParent<Slot>();
        if (targetSlot == null)
        {
            _rectTransform.anchoredPosition = _initialPos;
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
        foreach (DragDrop child in transform.parent.GetComponentsInChildren<DragDrop>())
        {
            if (child == this)
                continue;
            child._canvasGroup.blocksRaycasts = false;
        }
    }

    private void EnableOthersBlockRaycast()
    {
        foreach (DragDrop child in transform.parent.GetComponentsInChildren<DragDrop>())
        {
            if (child == this)
                continue;
            child._canvasGroup.blocksRaycasts = true;
        }
    }
}
