using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotplateManager : MonoBehaviour, IWorkStation
{
    private HotplateVisuals hotplateVisuals;
    private List<Ingredient> cookingIngredients = new();
    private List<float> cookingTimes = new();
    private List<float> totalCookingTimes = new();

    private Worker currentWorker = null;
    private bool isWorkerTaskDone = false;


    void Awake()
    {
        hotplateVisuals = FindFirstObjectByType<HotplateVisuals>(FindObjectsInactive.Include);
        hotplateVisuals.Setup();
        for (int i = 0; i < GlobalConstant.MAX_COOKING_INGREDIENTS; i++)
        {
            cookingIngredients.Add(null);
            cookingTimes.Add(GlobalConstant.UNUSED_TIME_VALUE);
            totalCookingTimes.Add(GlobalConstant.UNUSED_TIME_VALUE);
        }
    }
    void Update()
    {
        if (GameManager.Instance.isGamePaused) { return; }

        for (int i = 0; i < cookingTimes.Count; i++)
        {
            if (cookingTimes[i] == GlobalConstant.UNUSED_TIME_VALUE) { continue; }

            if (cookingTimes[i] >= totalCookingTimes[i])
            {
                hotplateVisuals.OnIngredientCooked(i);
            }

            if (cookingTimes[i] >= totalCookingTimes[i] + cookingIngredients[i].wastingTimeOffset)
            {
                hotplateVisuals.OnIngredientBurnt(i);
            }

            cookingTimes[i] += Time.deltaTime;
            hotplateVisuals.UpdateTimer(i, cookingTimes[i] / totalCookingTimes[i]);


        }
    }

    public void SetupIngredients()
    {
        hotplateVisuals.SetupIngredients(GetIngredientsToCook());
    }

    public void AddAvailableIngredient(Ingredient ingredient)
    {
        if (ingredient.category == IngredientCategoryEnum.MEAT)
        {
            hotplateVisuals.AddAvailableIngredient(ingredient);
        }
    }

    List<Ingredient> GetIngredientsToCook()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients.FindAll(ingredient => ingredient.category == IngredientCategoryEnum.MEAT);
    }

    public void CookIngredients(Ingredient ingredient)
    {
        for (int i = 0; i < cookingIngredients.Count; i++)
        {
            if (cookingIngredients[i] != null)
            {
                continue;
            }
            if (!GameManager.Instance.InventoryManager.IsUnprocessedIngredientAvailable(ingredient))
            {
                return;
            }
            GameManager.Instance.InventoryManager.ConsumeUnprocessedIngredient(ingredient);
            cookingIngredients[i] = ingredient;
            cookingTimes[i] = 0;
            totalCookingTimes[i] = cookingIngredients[i].processingTime * GameManager.Instance.UpgradeManager.GetEffect("HOTPLATE");
            hotplateVisuals.CookIngredients(ingredient, i);
            return;
        }
        throw new NotEnoughSpaceException();

    }
    public bool CanAddIngredientToCook()
    {
        for (int i = 0; i < cookingIngredients.Count; i++)
        {
            if (cookingIngredients[i] == null)
            {
                return true;
            }
        }
        return false;
    }

    public void OnClickOnIngredient(int position)
    {
        var cookingTime = cookingTimes[position];

        if (cookingTime < totalCookingTimes[position])
        {
            return;
        }

        if (cookingTime > totalCookingTimes[position] + cookingIngredients[position].wastingTimeOffset)
        {
            OnIngredientBurntClicked(position);
            return;
        }
        OnIngredientCookedClicked(position);


    }

    void OnIngredientCookedClicked(int position, bool? doneByWorker = false)
    {
        var ingredient = cookingIngredients[position];
        if (GameManager.Instance.InventoryManager.CanAddIngredient(ingredient))
        {
            GameManager.Instance.InventoryManager.AddIngredient(ingredient);
            RemoveIngredientFromCooking(position);
            if (doneByWorker == true)
            {
                isWorkerTaskDone = true;
            }
        }
    }

    void OnIngredientBurntClicked(int position, bool? doneByWorker = false)
    {
        RemoveIngredientFromCooking(position);
        if (doneByWorker == true)
        {
            isWorkerTaskDone = true;
        }
    }


    void RemoveIngredientFromCooking(int position)
    {
        cookingIngredients[position] = null;
        cookingTimes[position] = GlobalConstant.UNUSED_TIME_VALUE;
        totalCookingTimes[position] = GlobalConstant.UNUSED_TIME_VALUE;
        hotplateVisuals.RemoveIngredientFromGrill(position);
    }
    public void HireWorker(Worker worker)
    {
        currentWorker = worker;
        StartCoroutine(WorkerTaskCoroutine());
    }
    public void FireWorker(Worker worker)
    {
        currentWorker = null;
    }

    public IEnumerator WorkerTaskCoroutine()
    {
        if (currentWorker == null) { yield break; }

        yield return new WaitForSeconds(currentWorker.secondsBetweenTasks);
        while (!isWorkerTaskDone && currentWorker != null)
        {
            yield return new WaitUntil(() => !GameManager.Instance.isGamePaused);
            PerformWorkerTask();
            yield return new WaitForSeconds(0.5f);
        }
        isWorkerTaskDone = false;
        if (currentWorker != null)
        {
            StartCoroutine(WorkerTaskCoroutine());
        }

    }

    public void PerformWorkerTask()
    {
        WorkerRemoveDoneIngredient();
        if (isWorkerTaskDone)
        {
            return;
        }
        WorkerRemoveBurntIngredient();
        if (isWorkerTaskDone)
        {
            return;
        }
        WorkerAddIngredientToCook();

    }

    void WorkerRemoveDoneIngredient()
    {
        for (int i = 0; i < cookingIngredients.Count; i++)
        {
            if (cookingIngredients[i] == null) { continue; }

            if (cookingTimes[i] < totalCookingTimes[i]) { continue; }
            if (cookingTimes[i] > totalCookingTimes[i] + cookingIngredients[i].wastingTimeOffset)
            {
                continue;
            }
            OnIngredientCookedClicked(i, true);
            if (isWorkerTaskDone)
            {
                return;
            }
        }
    }

    void WorkerRemoveBurntIngredient()
    {
        for (int i = 0; i < cookingIngredients.Count; i++)
        {
            if (cookingIngredients[i] == null) { continue; }

            if (cookingTimes[i] < totalCookingTimes[i] + cookingIngredients[i].wastingTimeOffset)
            {
                continue;
            }
            OnIngredientBurntClicked(i, true);
            return;
        }
    }

    int GetNumberOfIngredientsCooking(Ingredient ingredientToFind)
    {
        return cookingIngredients.FindAll((ingredient) => ingredient == ingredientToFind).Count;
    }

    void WorkerAddIngredientToCook()
    {
        if (!CanAddIngredientToCook()) { return; }
        var unprocessedIngredients = GetIngredientsToCook();
        Ingredient ingredientToAdd = null;
        int minIngredientAvailable = 1000;
        foreach (var unprocessedIngredient in unprocessedIngredients)
        {
            if (!GameManager.Instance.InventoryManager.IsUnprocessedIngredientAvailable(unprocessedIngredient))
            {
                continue;
            }
            var currentIngredientAvailableQuantity = GameManager.Instance.InventoryManager.GetProcessedIngredientQuantity(unprocessedIngredient) + GetNumberOfIngredientsCooking(unprocessedIngredient);

            if (currentIngredientAvailableQuantity >= GameManager.Instance.InventoryManager.GetProcessedIngredientMaxAmount())
            {
                continue;
            }

            if (currentIngredientAvailableQuantity < minIngredientAvailable)
            {
                minIngredientAvailable = currentIngredientAvailableQuantity;
                ingredientToAdd = unprocessedIngredient;
            }
            if (currentIngredientAvailableQuantity == minIngredientAvailable && Random.Range(0, 2) == 0)
            {
                ingredientToAdd = unprocessedIngredient;
            }
        }
        if (ingredientToAdd == null)
        {
            return;
        }
        CookIngredients(ingredientToAdd);
    }

    public void UpdateButtonsVisual()
    {
        hotplateVisuals.UpdateIngredientButtons();
    }
}
