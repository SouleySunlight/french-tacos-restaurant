
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TacosMovemement : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector] public UnityEvent<GameObject> ClickEventGrill;
    [SerializeField] private CanvasGroup canvasGroup;
    public bool isAboveTrash = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        if (PlayzoneVisual.currentView == ViewToShowEnum.GRILL)
        {
            FindFirstObjectByType<GrillVisual>().PutTacosAbove(eventData.pointerDrag);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (PlayzoneVisual.currentView == ViewToShowEnum.CHECKOUT)
        {
            FindFirstObjectByType<CheckoutVisual>().UpdateVisuals();
        }
        if (PlayzoneVisual.currentView == ViewToShowEnum.GRILL)
        {
            GameManager.Instance.GrillManager.OnEndDrag(eventData.pointerDrag);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (PlayzoneVisual.currentView == ViewToShowEnum.CHECKOUT)
        {
            GetComponent<RectTransform>().anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
        }
        if (PlayzoneVisual.currentView == ViewToShowEnum.GRILL)
        {
            FindFirstObjectByType<GrillVisual>().DragTacos(eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayzoneVisual.currentView == ViewToShowEnum.GRILL)
        {
            ClickEventGrill.Invoke(gameObject);
        }
    }
}