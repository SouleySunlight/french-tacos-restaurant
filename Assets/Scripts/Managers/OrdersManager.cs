using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        LaunchOrderProvider();
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

    private IEnumerator OrderProviderCoroutine()
    {
        yield return new WaitForSeconds(2);
        AddNewOrder(GenerateOrder());

        while (true)
        {
            var timeToWait = Random.Range(30, 60);
            yield return new WaitForSeconds(timeToWait);
            AddNewOrder(GenerateOrder());
        }
    }

    void LaunchOrderProvider()
    {
        StartCoroutine(OrderProviderCoroutine());
    }

    public void TryToServeTacos(Order order, Tacos tacos)
    {
        if (IsTacosPartOfTheOrder(order, tacos))
        {
            ServeTacos(order, tacos);
            return;
        }
        RefuseTacos();
    }

    void ServeTacos(Order order, Tacos tacos)
    {
        var orderItem = FindMatchingOrderIdem(order, tacos);
        orderItem.isServed = true;
        ordersVisual.UpdateOrderVisual(order);
        GameManager.Instance.ServeTacos(tacos);
    }

    void RefuseTacos()
    {
        GameManager.Instance.RefuseTacos();
    }

    bool IsTacosPartOfTheOrder(Order order, Tacos tacos)
    {
        foreach (var orderItem in order.expectedOrder)
        {
            if (orderItem.tacosIngredients.Count != tacos.ingredients.Count)
            {
                continue;
            }
            if (orderItem.tacosIngredients.OrderBy(x => x.GetInstanceID()).SequenceEqual(tacos.ingredients.OrderBy(x => x.GetInstanceID())))
            {
                return true;
            }
        }
        return false;
    }

    OrderItem FindMatchingOrderIdem(Order order, Tacos tacos)
    {
        var matchingOrderItem = order.expectedOrder.Find((orderItem) => orderItem.tacosIngredients.OrderBy(x => x.GetInstanceID()).SequenceEqual(tacos.ingredients.OrderBy(x => x.GetInstanceID())));

        return matchingOrderItem;
    }
}
