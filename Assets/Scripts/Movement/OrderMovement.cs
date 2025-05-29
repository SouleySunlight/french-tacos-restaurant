using UnityEngine;
using UnityEngine.EventSystems;

public class OrderMovement : MonoBehaviour, IDropHandler
{
    private OrdersManager ordersManager;

    void Awake()
    {
        ordersManager = FindFirstObjectByType<OrdersManager>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Order order = GetComponent<OrderDisplayer>().orderData;
        if (dropped != null && dropped.GetComponent<TacosDisplayer>() != null)
        {
            var tacos = dropped.GetComponent<TacosDisplayer>().tacosData;
            ordersManager.TryToServeTacos(order, tacos);
        }
    }
}