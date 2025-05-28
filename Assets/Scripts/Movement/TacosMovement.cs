
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TacosMovemement : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector] public UnityEvent<GameObject> ClickEventGrill;
    [SerializeField] private CanvasGroup canvasGroup;

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (PlayzoneVisual.currentView == ViewToShowEnum.CHECKOUT)
        {
            GetComponent<RectTransform>().anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
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