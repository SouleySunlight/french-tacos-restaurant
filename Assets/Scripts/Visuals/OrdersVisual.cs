using UnityEngine;

public class OrdersVisual : MonoBehaviour
{
    [SerializeField] private GameObject orderPrefab;
    [SerializeField] private RectTransform orderFirstPosition;

    public void AddOrder(Order order)
    {
        var newOrder = Instantiate(orderPrefab, orderFirstPosition.position, Quaternion.identity, orderFirstPosition);
        newOrder.GetComponent<OrderDisplayer>().orderData = order;
    }



}
