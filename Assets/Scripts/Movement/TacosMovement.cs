
using System.Collections;
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

    public void ThrowTacos(GameObject tacos, RectTransform trashPosition)
    {
        StartCoroutine(ThrowTacosCoroutine(0.25f, tacos, trashPosition));
    }
    IEnumerator ThrowTacosCoroutine(float time, GameObject tacos, RectTransform trashPosition)
    {
        var rectTransform = tacos.GetComponent<RectTransform>();
        Vector3 startingPos = rectTransform.position;
        Vector3 finalPos = trashPosition.transform.position;

        Vector3 startingScale = rectTransform.localScale;
        Vector3 finalScale = Vector3.one * 0.1f;

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            float animationCompleteRatio = elapsedTime / time;
            var position = Vector3.Lerp(startingPos, finalPos, animationCompleteRatio);
            position.y = startingPos.y + (finalPos.y - startingPos.y) * animationCompleteRatio;
            rectTransform.position = position;
            rectTransform.localScale = Vector3.Lerp(startingScale, finalScale, animationCompleteRatio);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        GameManager.Instance.GrillManager.DiscardTacos(tacos.GetComponent<TacosDisplayer>().tacosData);
        Destroy(gameObject);
    }
}