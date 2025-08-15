using System.Collections.Generic;
using UnityEngine;

public class OrdersVisual : MonoBehaviour
{
    [SerializeField] private GameObject orderPrefab;
    private List<GameObject> orderPrefabs = new();

    public void AddOrder(Order order)
    {
        var newOrder = Instantiate(orderPrefab, this.transform);
        newOrder.GetComponent<OrderDisplayer>().orderData = order;
        orderPrefabs.Add(newOrder);
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        var orderWidth = orderPrefab.GetComponent<RectTransform>().rect.width;
        var orderHeight = orderPrefab.GetComponent<RectTransform>().rect.height;

        var index = 0;
        foreach (var prefab in orderPrefabs)
        {
            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(0, 0.5f);
            rectTransform.anchoredPosition = new Vector2(orderWidth / 2 + 20 + (index % 3) * (orderWidth + 20), (index / 3) * -(orderHeight + 20));
            prefab.GetComponent<RectTransform>().SetAsFirstSibling();
            index++;
        }
    }

    public void CompleteOrder(Order order)
    {
        var orderToUpdate = orderPrefabs.Find(orderPrefab => orderPrefab.GetComponent<OrderDisplayer>().orderData.guid == order.guid);
        if (orderToUpdate == null)
        {
            return;
        }

        Destroy(orderToUpdate);
        orderPrefabs.Remove(orderToUpdate);
        UpdateVisuals();
    }



}
