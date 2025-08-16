using System.Collections.Generic;
using UnityEngine;

public class CompletionBarManager : MonoBehaviour
{
    private CompletionBarVisual completionBarVisual;

    [SerializeField] List<Reward> rewards = new();
    private int currentRewardIndex = 0;
    private int target = 0;
    private int current = 0;
    private int numberOfTacosServed = 0;
    private bool isMaximumReached = false;



    void Awake()
    {
        completionBarVisual = FindFirstObjectByType<CompletionBarVisual>(FindObjectsInactive.Include);
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
        if (isMaximumReached) return;
        numberOfTacosServed++;
        current++;
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
                return;
            }
            current -= rewards[i].numberOfTacosToUnlock;
        }
        completionBarVisual.ShowMaximum();
        isMaximumReached = true;

    }

}