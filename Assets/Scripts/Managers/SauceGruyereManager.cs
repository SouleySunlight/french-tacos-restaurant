using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SauceGruyereManager : MonoBehaviour, IWorkStation
{
    private SauceGruyereVisual sauceGruyereVisual;
    private List<Ingredient> sauceGruyereIngredients = new();
    [SerializeField] private Ingredient sauceGruyere;


    private float cookingTime = GlobalConstant.UNUSED_TIME_VALUE;
    private float currentCookingTime = GlobalConstant.UNUSED_TIME_VALUE;

    private bool isSauceGruyereCooked = false;
    private bool isSauceGruyereBurnt = false;
    private static readonly int SAUCE_GRUYERE_CREATED_QUANTITY = 5;

    private Worker currentWorker = null;
    private bool isWorkerTaskDone = false;


    void Awake()
    {
        sauceGruyereVisual = FindFirstObjectByType<SauceGruyereVisual>(FindObjectsInactive.Include);
    }

    void Update()
    {
        if (GameManager.Instance.isGamePaused) { return; }


        if (cookingTime == GlobalConstant.UNUSED_TIME_VALUE) { return; }

        if (cookingTime >= currentCookingTime + sauceGruyere.wastingTimeOffset)
        {
            if (!isSauceGruyereBurnt)
            {
                isSauceGruyereBurnt = true;
                sauceGruyereVisual.OnSauceGruyereBurnt();
            }
        }

        if (cookingTime >= currentCookingTime && cookingTime < currentCookingTime + sauceGruyere.wastingTimeOffset)
        {
            if (!isSauceGruyereCooked)
            {
                isSauceGruyereCooked = true;
                sauceGruyereVisual.OnSauceGruyereCooked();
            }
        }

        cookingTime += Time.deltaTime;
        sauceGruyereVisual.UpdateTimer(cookingTime / currentCookingTime);

    }

    public void SetupIngredients()
    {
        sauceGruyereVisual.SetupIngredients(GetSauceGruyereComponent());
        sauceGruyereVisual.SetupIngredientIndicators(GetSauceGruyere());
    }

    List<Ingredient> GetSauceGruyereComponent()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients.FindAll((ingredient) => ingredient.category == IngredientCategoryEnum.SAUCE_GRUYERE_INGREDIENT);
    }

    List<Ingredient> GetSauceGruyere()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients.FindAll((ingredient) => ingredient.category == IngredientCategoryEnum.SAUCE_GRUYERE);
    }

    public void AddIngredientToSauceGruyere(Ingredient ingredient, bool? isDoneByWorker = false)
    {
        if (!GameManager.Instance.InventoryManager.IsUnprocessedIngredientAvailable(ingredient))
        {
            return;
        }
        if (sauceGruyereIngredients.Contains(ingredient))
        {
            return;
        }
        GameManager.Instance.InventoryManager.ConsumeUnprocessedIngredient(ingredient);
        sauceGruyereIngredients.Add(ingredient);
        sauceGruyereVisual.AddIngredientToSauceGruyere(ingredient);
        sauceGruyereVisual.UpdateIngredientButtons();
        isDoneByWorker = true;
        if (sauceGruyereIngredients.Count == GetSauceGruyereComponent().Count)
        {
            CookSauceGruyere();
        }
    }

    void CookSauceGruyere()
    {
        cookingTime = 0;
    }

    public void UpdateCookingTime()
    {
        currentCookingTime = sauceGruyere.processingTime * GameManager.Instance.UpgradeManager.GetSpeedfactor("GRUYERE_POT");
    }

    public void SetupCookingTime()
    {
        UpdateCookingTime();
    }

    public void OnClickOnPot()
    {
        if (isSauceGruyereBurnt)
        {
            RemoveSauceGruyere();
            return;
        }
        if (isSauceGruyereCooked)
        {
            RemoveCookedSauceGruyere();
        }
    }

    public void RemoveSauceGruyere()
    {
        sauceGruyereIngredients.Clear();
        sauceGruyereVisual.RemoveSauceGruyere();
        cookingTime = GlobalConstant.UNUSED_TIME_VALUE;
        isSauceGruyereCooked = false;
        isSauceGruyereBurnt = false;
        sauceGruyereVisual.UpdateTimer(0);

    }

    void RemoveCookedSauceGruyere()
    {
        GameManager.Instance.InventoryManager.AddProcessedIngredients(sauceGruyere, SAUCE_GRUYERE_CREATED_QUANTITY);
        RemoveSauceGruyere();
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

        yield return new WaitForSeconds(GlobalConstant.DELAY_BETWEEN_WORKER_TASKS);
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
        WorkerAddIngredientToSauceGruyere();
    }

    void WorkerRemoveDoneIngredient()
    {
        if (isSauceGruyereBurnt) { return; }
        if (!isSauceGruyereCooked)
        {
            return;
        }
        isWorkerTaskDone = true;
        RemoveCookedSauceGruyere();
    }

    void WorkerRemoveBurntIngredient()
    {
        if (!isSauceGruyereBurnt)
        {
            return;
        }
        isWorkerTaskDone = true;
        RemoveSauceGruyere();
    }

    void WorkerAddIngredientToSauceGruyere()
    {
        if (GameManager.Instance.isGamePaused) { return; }
        if (sauceGruyereIngredients.Count >= GetSauceGruyereComponent().Count)
        {
            return;
        }
        if (GameManager.Instance.InventoryManager.GetProcessedIngredientQuantity(sauceGruyere) + SAUCE_GRUYERE_CREATED_QUANTITY > GameManager.Instance.InventoryManager.GetProcessedIngredientMaxAmount())
        {
            return;
        }
        if (sauceGruyereIngredients.Count == 0)
        {
            AddIngredientToSauceGruyere(GetSauceGruyereComponent()[0]);
            return;

        }
        var missingIngredients = GetSauceGruyereComponent().FindAll((ingredient) => !sauceGruyereIngredients.Contains(ingredient));
        if (missingIngredients.Count == 0)
        {
            return;
        }
        AddIngredientToSauceGruyere(missingIngredients[0]);
    }

    public void FinishProcessingIngredients()
    {
        if (cookingTime == GlobalConstant.UNUSED_TIME_VALUE) { return; }
        if (isSauceGruyereBurnt)
        {
            RemoveSauceGruyere();
        }
        RemoveCookedSauceGruyere();
    }
}