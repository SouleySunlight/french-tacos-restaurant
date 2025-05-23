
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TacosMovemement : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public UnityEvent<GameObject> ClickEventGrill;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayzoneVisual.currentView == ViewToShowEnum.GRILL)
        {
            ClickEventGrill.Invoke(gameObject);
        }
    }
}