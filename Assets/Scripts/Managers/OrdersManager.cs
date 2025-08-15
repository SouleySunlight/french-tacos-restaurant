using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    private OrdersVisual ordersVisual;
    private List<Order> orders = new();
    private readonly int DEFAULT_DELAY_BETWEEN_ORDERS = 40;
    private readonly float popularityFactor = 0.9f;
    private readonly int MAX_NUMBER_OF_ORDERS = 3;
    private float timeSinceLastOrder = 0f;

    void Awake()
    {
        ordersVisual = FindFirstObjectByType<OrdersVisual>(FindObjectsInactive.Include);
    }

    void Update()
    {
        timeSinceLastOrder += Time.deltaTime;

        if (GameManager.Instance.isGamePaused)
        {
            return;
        }
        if (orders.Count > MAX_NUMBER_OF_ORDERS)
        {
            return;
        }
        if (GameManager.Instance.DayCycleManager.isDayOver)
        {
            return;
        }

        if (timeSinceLastOrder >= DEFAULT_DELAY_BETWEEN_ORDERS * Mathf.Pow(popularityFactor, GameManager.Instance.InventoryManager.Popularity))
        {
            if (orders.Count < MAX_NUMBER_OF_ORDERS)
            {
                AddNewOrder(GenerateOrder());
                timeSinceLastOrder = 0f;
            }
        }
    }

    void AddNewOrder(Order order)
    {
        orders.Add(order);
        ordersVisual.AddOrder(order);
    }

    Order GenerateOrder()
    {
        return new Order(GenerateSingleTacosComposition());
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

    IEnumerator AddFirstOrderOfTheDayCoroutine()
    {
        yield return new WaitForSeconds(2);
        AddNewOrder(GenerateOrder());
        timeSinceLastOrder = 0f;
    }

    public void AddFirstOrderOfTheDay()
    {
        StartCoroutine(AddFirstOrderOfTheDayCoroutine());
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

        if (!IsTacosCompositionCorrect(order, tacos))
        {
            RefuseTacos();
            return;
        }
        ServeTacos(order, tacos);
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
            if (!IsTacosCompositionCorrect(order, tacos))
            {
                continue;
            }
            ServeTacos(order, tacos);
            GameManager.Instance.CheckoutManager.MarkWorkerTaskAsDone();
            return;

        }
    }

    public void ServeTacos(Order order, Tacos tacos)
    {
        GameManager.Instance.ServeTacos(tacos);
        CompleteOrder(order);
    }

    void CompleteOrder(Order order)
    {
        orders.Remove(order);
        GameManager.Instance.WalletManager.ReceiveMoney(order.price);
        ordersVisual.CompleteOrder(order);
        if (GameManager.Instance.DayCycleManager.isDayOver && GameManager.Instance.OrdersManager.GetCurrentOrdersCount() == 0)
        {
            GameManager.Instance.DayCycleManager.TryToFinishDay();
        }
    }


    void RefuseTacos()
    {
        GameManager.Instance.RefuseTacos();
    }

    bool IsTacosCompositionCorrect(Order order, Tacos tacos)
    {
        return order.expectedOrder.OrderBy(x => x.GetInstanceID()).SequenceEqual(tacos.ingredients.OrderBy(x => x.GetInstanceID()));
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

    public int GetCurrentOrdersCount()
    {
        return orders.Count;
    }
}
