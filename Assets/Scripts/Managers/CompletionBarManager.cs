using System.Collections.Generic;
using UnityEngine;

public class CompletionBarManager : MonoBehaviour
{
    private CompletionBarVisual completionBarVisual;

    [SerializeField] List<Reward> rewards = new();

    private int numberOfTacosServed = 0;



    void Awake()
    {
        completionBarVisual = FindFirstObjectByType<CompletionBarVisual>(FindObjectsInactive.Include);
        completionBarVisual.UpdateVisual(0, 100);
        OrderReward();
    }

    public int GetNumberOfTacosServed()
    {
        return numberOfTacosServed;
    }

    public void LoadNumberOfTacosServed(int numberOfTacos)
    {
        numberOfTacosServed = numberOfTacos;
        completionBarVisual.UpdateVisual(numberOfTacosServed, 100);
    }

    public void IncrementNumberOfTacosServed()
    {
        numberOfTacosServed++;
    }

    void OrderReward()
    {
        rewards.Sort((x, y) => x.numberOfTacosToUnlock.CompareTo(y.numberOfTacosToUnlock));
    }

}