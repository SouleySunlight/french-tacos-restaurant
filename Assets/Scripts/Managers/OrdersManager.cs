using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    private OrdersVisual ordersVisual;
    private List<Order> orders = new();

    void Awake()
    {
        ordersVisual = FindFirstObjectByType<OrdersVisual>(FindObjectsInactive.Include);
    }

    void Start()
    {
        AddNewOrder(GenerateOrder());
    }

    void AddNewOrder(Order order)
    {
        orders.Add(order);
        ordersVisual.AddOrder(order);
    }

    Order GenerateOrder()
    {
        List<List<Ingredient>> orderList = new();
        List<Ingredient> ingredients = new()
        {
            GameManager.Instance.AvailableIngredients[0]
        };
        orderList.Add(ingredients);

        return new Order(orderList);
    }


}
