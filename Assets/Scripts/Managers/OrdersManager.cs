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

        var numberOfTacos = Random.Range(1, 4);

        for (int i = 0; i < numberOfTacos; i++)
        {
            orderList.Add(GenerateSingleTacosComposition());
        }

        return new Order(orderList);
    }

    List<Ingredient> GenerateSingleTacosComposition()
    {
        List<Ingredient> ingredients = new();
        var availableIngrdients = GameManager.Instance.InventoryManager.UnlockedIngredients;
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
            var timeToWait = Random.Range(20, 30);
            yield return new WaitForSeconds(timeToWait);
            yield return new WaitUntil(() => GameManager.Instance.isGamePaused == false);
            AddNewOrder(GenerateOrder());
        }
    }

    void LaunchOrderProvider()
    {
        StartCoroutine(OrderProviderCoroutine());
    }

    public void TryToServeTacos(Order order, Tacos tacos)
    {
        if (tacos.isBurnt)
        {
            RefuseTacos();
            return;
        }

        if (!tacos.isGrilled)
        {
            RefuseTacos();
            return;
        }

        var matchingOrderItem = FindMatchingOrderIdem(order, tacos);

        if (matchingOrderItem == null)
        {
            RefuseTacos();
            return;
        }
        ServeTacos(order, tacos, matchingOrderItem);
    }

    void ServeTacos(Order order, Tacos tacos, OrderItem orderItem)
    {
        GameManager.Instance.ServeTacos(tacos);

        orderItem.isServed = true;
        if (order.expectedOrder.Exists(orderItem => !orderItem.isServed))
        {
            ordersVisual.UpdateOrderVisual(order);
            return;
        }

        CompleteOrder(order);
    }

    void CompleteOrder(Order order)
    {
        GameManager.Instance.CompleteOrder(order);
        ordersVisual.CompleteOrder(order);
    }


    void RefuseTacos()
    {
        GameManager.Instance.RefuseTacos();
    }

    OrderItem FindMatchingOrderIdem(Order order, Tacos tacos)
    {
        var matchingOrderItem = order.expectedOrder.Find((orderItem) => orderItem.tacosIngredients.OrderBy(x => x.GetInstanceID()).SequenceEqual(tacos.ingredients.OrderBy(x => x.GetInstanceID())) && !orderItem.isServed);

        return matchingOrderItem;
    }
}
