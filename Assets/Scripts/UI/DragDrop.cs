using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Action<int> OnChangeItemUIIndex;
    public Slot currentSlot;
    public RectTransform _rectTransform;

    [SerializeField] private Canvas _canvas;
    private Vector2 _initialPos;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0.5f;
        _canvasGroup.blocksRaycasts = false;     
        _initialPos = _rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // move along the mouse position
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        GameObject target = eventData.pointerEnter;
        // cannot be dropped outside the inventory bound
        if(target == null || !target.CompareTag("slot"))
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
            currentSlot.SlotItem = null;
            currentSlot = targetSlot;
            OnChangeItemUIIndex?.Invoke(targetSlot.uiIndex);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // start dragging
    }
}
