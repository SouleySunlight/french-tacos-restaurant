using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    private OrdersVisual ordersVisual;
    private List<Order> orders = new();
    private readonly int DEFAULT_DELAY_BETWEEN_ORDERS = 60;
    private readonly float popularityFactor = 0.9f;

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

        ingredients.Add(GetMeatOfTheTacos());
        ingredients.AddRange(GetVegetablesOfTheTacos());
        ingredients.AddRange(GetSauceOfTheTacos());
        ingredients.AddRange(GetInEveryTacosIngredients());

        return ingredients;
    }

    private IEnumerator OrderProviderCoroutine()
    {
        yield return new WaitForSeconds(2);
        AddNewOrder(GenerateOrder());

        while (true)
        {
            var timeToWait = DEFAULT_DELAY_BETWEEN_ORDERS * Mathf.Pow(popularityFactor, GameManager.Instance.InventoryManager.Popularity);
            Debug.Log(GameManager.Instance.InventoryManager.Popularity);
            Debug.Log(timeToWait);
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

    public void WorkerTryToServeTacos(Tacos tacos)
    {
        if (tacos.isBurnt)
        {
            return;
        }

        if (!tacos.isGrilled)
        {
            return;
        }

        foreach (var order in orders)
        {
            var matchingOrderItem = FindMatchingOrderIdem(order, tacos);

            if (matchingOrderItem == null)
            {
                continue;
            }
            ServeTacos(order, tacos, matchingOrderItem);
            GameManager.Instance.CheckoutManager.MarkWorkerTaskAsDone();
            return;

        }
    }

    public void ServeTacos(Order order, Tacos tacos, OrderItem orderItem)
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

    public Order GetOrderWithTacos(Tacos tacos)
    {
        return orders.Find(order => order.expectedOrder.Exists(orderItem => orderItem.tacosIngredients.OrderBy(x => x.GetInstanceID()).SequenceEqual(tacos.ingredients.OrderBy(x => x.GetInstanceID()))));
    }

    List<Ingredient> GetVegetablesOfTheTacos()
    {
        var availableVegetables = GameManager.Instance.InventoryManager.UnlockedIngredients
            .FindAll(ingredient => ingredient.category == IngredientCategoryEnum.VEGETABLE);
        var numberOfVegetables = Random.Range(0, availableVegetables.Count + 1);
        var selectedVegetables = new List<Ingredient>();
        for (int i = 0; i < numberOfVegetables; i++)
        {
            var vegetableToAdd = availableVegetables[Random.Range(0, availableVegetables.Count)];
            selectedVegetables.Add(vegetableToAdd);
            availableVegetables.Remove(vegetableToAdd);
        }
        return selectedVegetables;
    }

    List<Ingredient> GetSauceOfTheTacos()
    {
        var availableSauces = GameManager.Instance.InventoryManager.UnlockedIngredients
            .FindAll(ingredient => ingredient.category == IngredientCategoryEnum.SAUCE);
        if (availableSauces.Count == 0) { return new List<Ingredient>(); }
        var numberOfSauces = Random.Range(0, availableSauces.Count >= 2 ? 3 : 2);
        var selectedSauces = new List<Ingredient>();
        for (int i = 0; i < numberOfSauces; i++)
        {
            var vegetableToAdd = availableSauces[Random.Range(0, availableSauces.Count)];
            selectedSauces.Add(vegetableToAdd);
            availableSauces.Remove(vegetableToAdd);
        }
        return selectedSauces;
    }

    Ingredient GetMeatOfTheTacos()
    {
        var availableMeat = GameManager.Instance.InventoryManager.UnlockedIngredients
            .FindAll(ingredient => ingredient.category == IngredientCategoryEnum.MEAT);
        return availableMeat[Random.Range(0, availableMeat.Count)];
    }

    List<Ingredient> GetInEveryTacosIngredients()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients
            .FindAll(ingredient => ingredient.inEveryTacos);
    }
}
