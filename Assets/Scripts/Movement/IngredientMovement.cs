using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class IngredientMovement : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public UnityEvent<GameObject> ClickHotplateEvent;
    [HideInInspector] public UnityEvent<GameObject> ClickFryerEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayzoneVisual.currentView == ViewToShowEnum.HOTPLATE)
        {
            ClickHotplateEvent.Invoke(gameObject);
        }
        if (PlayzoneVisual.currentView == ViewToShowEnum.FRYER)
        {
            ClickFryerEvent.Invoke(gameObject);
        }
    }
}