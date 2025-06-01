using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class IngredientMovement : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public UnityEvent<GameObject> ClickHotplateEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayzoneVisual.currentView == ViewToShowEnum.HOTPLATE)
        {
            ClickHotplateEvent.Invoke(gameObject);
        }
    }
}