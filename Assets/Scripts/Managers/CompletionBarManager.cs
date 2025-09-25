using System.Collections.Generic;
using UnityEngine;

public class CompletionBarManager : MonoBehaviour
{
    private CompletionBarVisual completionBarVisual;
    private RewardModalVisual rewardModalVisual;

    [SerializeField] List<Reward> rewards = new();
    private int currentRewardIndex = 0;
    private int target = 0;
    private int current = 0;
    private int numberOfTacosServed = 0;
    private bool isMaximumReached = false;



    void Awake()
    {
        completionBarVisual = FindFirstObjectByType<CompletionBarVisual>(FindObjectsInactive.Include);
        rewardModalVisual = FindFirstObjectByType<RewardModalVisual>(FindObjectsInactive.Include);
        OrderReward();
    }

    public int GetNumberOfTacosServed()
    {
        return numberOfTacosServed;
    }

    public void LoadNumberOfTacosServed(int numberOfTacos)
    {
        numberOfTacosServed = numberOfTacos;
        SetObjectives();

    }

    public void IncrementNumberOfTacosServed()
    {
        if (GameManager.Instance.DayCycleManager.GetCurrentDay() == 0) { return; }
        if (isMaximumReached) return;
        numberOfTacosServed++;
        current++;
        if (current >= target)
        {
            UnlockReward();
            rewardModalVisual.ShowRewardModal();
            currentRewardIndex++;
            current = 0;
            target = rewards[currentRewardIndex].numberOfTacosToUnlock;
        }
        completionBarVisual.UpdateVisual(current, target);
    }

    void OrderReward()
    {
        rewards.Sort((x, y) => x.numberOfTacosToUnlock.CompareTo(y.numberOfTacosToUnlock));
    }

    void SetObjectives()
    {
        current = numberOfTacosServed;
        for (int i = 0; i < rewards.Count; i++)
        {
            if (current < rewards[i].numberOfTacosToUnlock)
            {
                target = rewards[i].numberOfTacosToUnlock;
                currentRewardIndex = i;
                completionBarVisual.UpdateVisual(current, target);
                rewardModalVisual.LoadNextRewardModal(rewards[i]);
                return;
            }
            current -= rewards[i].numberOfTacosToUnlock;
        }
        completionBarVisual.ShowMaximum();
        isMaximumReached = true;

    }

    void UnlockReward()
    {
        switch (rewards[currentRewardIndex].rewardType)
        {
            case RewardType.UNLOCK_INGREDIENT:
                GameManager.Instance.InventoryManager.UnlockIngredient(rewards[currentRewardIndex].ingredientToUnlock);
                break;
            case RewardType.PRICE_INCREASE:
                GameManager.Instance.OrdersManager.IncrementTacosPrice();
                break;
            case RewardType.MORE_ORDERS:
                GameManager.Instance.OrdersManager.IncrelmentMaxNumberOfOrders();
                break;
            case RewardType.INCREASE_MAX_INGREDIENTS:
                GameManager.Instance.InventoryManager.UpdateProcessedInventoryMaxAmount(5);
                break;
        }
    }

    public void OnClickOnRewardModalButton()
    {
        rewardModalVisual.LoadNextRewardModal(rewards[currentRewardIndex]);
        rewardModalVisual.HideRewardModal();
    }

}