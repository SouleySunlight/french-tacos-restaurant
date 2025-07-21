using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class IngredientMovement : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public UnityEvent<GameObject> ClickHotplateEvent;
    [HideInInspector] public UnityEvent<GameObject> ClickFryerEvent;
    [HideInInspector] public UnityEvent<GameObject> ClickSauceGruyereEvent;
    [HideInInspector] public UnityEvent<GameObject> ClickTacosMakerEvent;



    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayzoneVisual.currentView == ViewToShowEnum.TACOS_MAKER)
        {
            ClickTacosMakerEvent.Invoke(gameObject);
        }
        if (PlayzoneVisual.currentView == ViewToShowEnum.HOTPLATE)
        {
            ClickHotplateEvent.Invoke(gameObject);
        }
        if (PlayzoneVisual.currentView == ViewToShowEnum.FRYER)
        {
            ClickFryerEvent.Invoke(gameObject);
        }
        if (PlayzoneVisual.currentView == ViewToShowEnum.SAUCE_GRUYERE)
        {
            ClickSauceGruyereEvent.Invoke(gameObject);
        }
    }
}