using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class FryerManager : MonoBehaviour, IWorkStation
{
    private FryerVisual fryerVisuals;
    private List<Ingredient> fryingIngredients = new();
    private List<float> fryingTimes = new();
    private List<float> totalFryingTimes = new();
    private List<int> fryingQuantities = new();
    private List<bool> isFrying = new();

    private static int BASKET_SIZE = 3;


    private Worker currentWorker = null;
    private bool isWorkerTaskDone = false;
    [SerializeField] private AudioClip fryingSound;
    private bool isAmbientPlaying = false;

    void Awake()
    {
        fryerVisuals = FindFirstObjectByType<FryerVisual>(FindObjectsInactive.Include);
        fryerVisuals.Setup();
        for (int i = 0; i < GlobalConstant.MAX_FRYING_INGREDIENTS; i++)
        {
            fryingIngredients.Add(null);
            fryingTimes.Add(GlobalConstant.UNUSED_TIME_VALUE);
            totalFryingTimes.Add(GlobalConstant.UNUSED_TIME_VALUE);
            fryingQuantities.Add(0);
            isFrying.Add(false);
        }
    }

    void Update()
    {
        if (GameManager.Instance.isGamePaused) { return; }

        for (int i = 0; i < fryingTimes.Count; i++)
        {
            if (fryingTimes[i] == GlobalConstant.UNUSED_TIME_VALUE) { continue; }

            if (fryingTimes[i] >= totalFryingTimes[i])
            {
                fryerVisuals.OnIngredientCooked(i);
            }

            if (fryingTimes[i] >= totalFryingTimes[i] + fryingIngredients[i].wastingTimeOffset)
            {
                fryerVisuals.OnIngredientBurnt(i);
            }

            fryingTimes[i] += Time.deltaTime;
            fryerVisuals.UpdateTimer(i, fryingTimes[i] / totalFryingTimes[i]);
        }
    }

    public void SetupIngredients()
    {
        fryerVisuals.SetupIngredients(GetIngredientsToFry());
    }

    List<Ingredient> GetIngredientsToFry()
    {
        return GameManager.Instance.InventoryManager.UnlockedIngredients.FindAll(ingredient => ingredient.processingMethod == ProcessingMethodEnum.FRYER);
    }

    public void AddAvailableIngredient(Ingredient ingredient)
    {
        if (ingredient.processingMethod == ProcessingMethodEnum.FRYER)
        {
            fryerVisuals.AddAvailableIngredient(ingredient);
        }
    }


    public void FryIngredients(Ingredient ingredient)
    {
        for (int i = 0; i < fryingIngredients.Count; i++)
        {
            if (fryingIngredients[i] != ingredient && fryingIngredients[i] != null)
            {
                continue;
            }
            if (isFrying[i])
            {
                continue;
            }
            if (fryingQuantities[i] >= BASKET_SIZE)
            {
                continue;
            }
            if (!GameManager.Instance.InventoryManager.IsUnprocessedIngredientAvailable(ingredient))
            {
                return;
            }
            GameManager.Instance.InventoryManager.ConsumeUnprocessedIngredient(ingredient);
            if (fryingIngredients[i] == null)
            {
                fryingIngredients[i] = ingredient;
            }
            fryingQuantities[i]++;
            fryerVisuals.FryIngredients(ingredient, i);
            return;
        }
        throw new NotEnoughSpaceException();

    }
    public void OnClickOnBasket(int position)
    {
        if (!isFrying[position])
        {
            StartFryingIngredient(position);
            return;
        }

        var fryingTime = fryingTimes[position];
        if (fryingTime > totalFryingTimes[position] && fryingTime < totalFryingTimes[position] + fryingIngredients[position].wastingTimeOffset)
        {
            OnIngredientCookedClicked(position);
            return;
        }
        if (fryingTime >= totalFryingTimes[position] + fryingIngredients[position].wastingTimeOffset)
        {
            OnIngredientBurntClicked(position);
            return;
        }
    }

    void StartFryingIngredient(int position)
    {
        if (position < 0 || position >= fryingIngredients.Count)
        {
            return;
        }

        if (fryingIngredients[position] == null)
        {
            return;
        }

        isFrying[position] = true;
        fryingTimes[position] = 0;
        totalFryingTimes[position] = fryingIngredients[position].processingTime * GameManager.Instance.UpgradeManager.GetSpeedfactor("FRYER");
        fryerVisuals.StartFrying(position);
        ManageFryingSound();
    }

    void RemoveIngredientFromFrying(int position)
    {
        fryingIngredients[position] = null;
        fryingTimes[position] = GlobalConstant.UNUSED_TIME_VALUE;
        totalFryingTimes[position] = GlobalConstant.UNUSED_TIME_VALUE;
        isFrying[position] = false;
        fryingQuantities[position] = 0;
        fryerVisuals.RemoveIngredientFromFryer(position);
    }

    void OnIngredientCookedClicked(int position, bool? doneByWorker = false)
    {
        var ingredientToAdd = fryingIngredients[position];
        for (int i = 0; i < fryingQuantities[position]; i++)
        {
            if (GameManager.Instance.InventoryManager.CanAddProcessedIngredient(ingredientToAdd))
            {
                GameManager.Instance.InventoryManager.AddProcessedIngredient(ingredientToAdd);
            }
        }
        RemoveIngredientFromFrying(position);
        ManageFryingSound();
        if (doneByWorker == true)
        {
            isWorkerTaskDone = true;
        }
    }

    void OnIngredientBurntClicked(int position, bool? doneByWorker = false)
    {
        RemoveIngredientFromFrying(position);
        ManageFryingSound();
        if (doneByWorker == true)
        {
            isWorkerTaskDone = true;
        }
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
        WorkerRemoveFriedIngredient();
        if (isWorkerTaskDone)
        {
            return;
        }
        WorkerRemoveBurntIngredient();
        if (isWorkerTaskDone || currentWorker.level < 2)
        {
            return;
        }
        WorkerAddIngredientToFry();

    }
    void WorkerRemoveFriedIngredient()
    {
        for (int i = 0; i < fryingIngredients.Count; i++)
        {
            if (fryingIngredients[i] == null) { continue; }
            if (fryingTimes[i] == GlobalConstant.UNUSED_TIME_VALUE) { continue; }

            if (fryingTimes[i] < totalFryingTimes[i]) { continue; }
            if (fryingTimes[i] > totalFryingTimes[i] + fryingIngredients[i].wastingTimeOffset)
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
        for (int i = 0; i < fryingIngredients.Count; i++)
        {
            if (fryingIngredients[i] == null) { continue; }

            if (fryingTimes[i] < totalFryingTimes[i] + fryingIngredients[i].wastingTimeOffset)
            {
                continue;
            }
            OnIngredientBurntClicked(i, true);
            return;
        }
    }

    int GetNumberOfIngredientsFrying(Ingredient ingredientToFind)
    {
        var count = 0;
        for (int i = 0; i < fryingIngredients.Count; i++)
        {
            if (fryingIngredients[i] == null) { continue; }
            if (fryingIngredients[i] != ingredientToFind) { continue; }
            count += fryingQuantities[i];

        }
        return count;
    }

    void WorkerAddIngredientToFry()
    {
        var unprocessedIngredients = GetIngredientsToFry();
        Ingredient ingredientToAdd = null;
        int minIngredientAvailable = 1000;
        foreach (var unprocessedIngredient in unprocessedIngredients)
        {
            if (!CanAddIngredientToFry(unprocessedIngredient)) { return; }
            var currentIngredientAvailableQuantity = GameManager.Instance.InventoryManager.GetProcessedIngredientQuantity(unprocessedIngredient) + GetNumberOfIngredientsFrying(unprocessedIngredient);

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
        var remainingSlotAvailable = GameManager.Instance.InventoryManager.GetProcessedIngredientMaxAmount() - minIngredientAvailable;

        var numberOfIngredientToFry = remainingSlotAvailable < BASKET_SIZE ? remainingSlotAvailable : BASKET_SIZE;


        var positionOfIngredient = fryingIngredients.FindIndex(ingredient => ingredient == null);
        for (int i = 0; i < numberOfIngredientToFry; i++)
        {
            if (CanAddIngredientToFry(ingredientToAdd))
            {
                FryIngredients(ingredientToAdd);
            }
        }
        StartFryingIngredient(positionOfIngredient);
    }
    public bool CanAddIngredientToFry(Ingredient ingredient)
    {
        for (int i = 0; i < fryingIngredients.Count; i++)
        {
            if (fryingIngredients[i] == null)
            {
                return true;
            }
            if (fryingIngredients[i] == ingredient && fryingQuantities[i] < BASKET_SIZE)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateButtonsVisual()
    {
        fryerVisuals.UpdateIngredientButtons();
    }

    public void FinishProcessingIngredients()
    {
        for (int i = 0; i < fryingIngredients.Count; i++)
        {
            if (fryingIngredients[i] == null) { continue; }
            if (fryingTimes[i] >= totalFryingTimes[i] + fryingIngredients[i].wastingTimeOffset)
            {
                OnIngredientBurntClicked(i);
            }
            OnIngredientCookedClicked(i);
        }
    }

    bool AreSomeIngredientsCooking()
    {
        for (int i = 0; i < isFrying.Count; i++)
        {
            if (isFrying[i])
            {
                return true;
            }
        }
        return false;
    }

    void ManageFryingSound()
    {
        if (AreSomeIngredientsCooking() && !isAmbientPlaying)
        {
            GameManager.Instance.SoundManager.PlayAmbient(fryingSound);
            isAmbientPlaying = true;
        }
        if (isAmbientPlaying && !AreSomeIngredientsCooking())
        {
            GameManager.Instance.SoundManager.StopAmbient();
            isAmbientPlaying = false;
        }
    }

    public void ManageFryingSoundOnViewChanged()
    {
        if (isAmbientPlaying)
        {
            GameManager.Instance.SoundManager.PlayAmbient(fryingSound);
            return;
        }

        GameManager.Instance.SoundManager.StopAmbient();

    }
}