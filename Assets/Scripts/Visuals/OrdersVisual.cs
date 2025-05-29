using System.Collections.Generic;
using UnityEngine;

public class OrdersVisual : MonoBehaviour
{
    [SerializeField] private GameObject orderPrefab;
    [SerializeField] private RectTransform orderFirstPosition;
    private List<GameObject> orderPrefabs = new();
    private readonly int HORIZONTAL_PADDING = 80;

    public void AddOrder(Order order)
    {
        var newOrder = Instantiate(orderPrefab, orderFirstPosition.position, Quaternion.identity, orderFirstPosition);
        newOrder.GetComponent<OrderDisplayer>().orderData = order;
        orderPrefabs.Add(newOrder);
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        var orderWidth = orderPrefab.GetComponent<RectTransform>().rect.width;
        var availableWidth = GlobalConstant.PLAYZONE_WIDTH - 2 * HORIZONTAL_PADDING;
        var gap = (orderPrefabs.Count - 1) * orderWidth < availableWidth ?
             availableWidth - (orderPrefabs.Count - 1) * orderWidth :
             orderWidth / 2;
        var index = 0;
        foreach (var prefab in orderPrefabs)
        {
            var newPosition = new Vector3(orderFirstPosition.position.x - gap * index, orderFirstPosition.position.y, orderFirstPosition.position.z);
            prefab.GetComponent<RectTransform>().position = newPosition;
            prefab.GetComponent<RectTransform>().SetAsFirstSibling();
            index++;
        }
    }

    public void UpdateOrderVisual(Order order)
    {
        var orderToUpdate = orderPrefabs.Find(orderPrefab => orderPrefab.GetComponent<OrderDisplayer>().orderData.guid == order.guid);
        if (orderToUpdate == null)
        {
            return;
        }
        orderToUpdate.GetComponent<OrderDisplayer>().UpdateOrder();
    }



}
