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

        var numberOfTacos = Random.Range(1, 3);

        for (int i = 0; i < numberOfTacos; i++)
        {
            orderList.Add(GenerateSingleTacosComposition());
        }

        return new Order(orderList);
    }

    List<Ingredient> GenerateSingleTacosComposition()
    {
        List<Ingredient> ingredients = new();
        var availableIngrdients = GameManager.Instance.AvailableIngredients;
        var numberOfIngredients = Random.Range(1, availableIngrdients.Count);

        for (int i = 0; i < numberOfIngredients; i++)
        {
            ingredients.Add(availableIngrdients[Random.Range(0, availableIngrdients.Count)]);
        }

        return ingredients;
    }

}
