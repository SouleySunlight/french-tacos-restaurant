using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BasketMovement : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public UnityEvent<GameObject> ClickBasket;
    public void OnPointerDown(PointerEventData eventData)
    {
        ClickBasket.Invoke(gameObject);
    }
}