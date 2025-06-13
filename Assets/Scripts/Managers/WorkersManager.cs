using System.Collections.Generic;
using UnityEngine;

public class WorkersManager : MonoBehaviour
{
    [SerializeField] private List<Worker> availableWorkers = new();
    private List<Worker> hiredWorkers = new();


    private WorkersVisual workersVisual;

    void Awake()
    {
        workersVisual = FindFirstObjectByType<WorkersVisual>();
    }

    public void SetupWorkers()
    {
        workersVisual.SetupWorkers(availableWorkers);
    }

    public void HireWorker(Worker worker)
    {
        if (!availableWorkers.Contains(worker))
        { return; }

        if (!GameManager.Instance.WalletManager.HasEnoughMoney(worker.pricePerDay))
        { return; }

        availableWorkers.Remove(worker);
        hiredWorkers.Add(worker);
        workersVisual.UpdateButtonsVisual();
        GameManager.Instance.WalletManager.SpendMoney(worker.pricePerDay);

        switch (worker.role)
        {
            case WorkersRole.GRILL:
                GameManager.Instance.GrillManager.HireWorker(worker);
                break;
            case WorkersRole.HOTPLATE:
                GameManager.Instance.HotplateManager.HireWorker(worker);
                break;
            case WorkersRole.CHECKOUT:
                GameManager.Instance.CheckoutManager.HireWorker(worker);
                break;
        }
    }

    public void FireWorker(Worker worker)
    {
        if (!hiredWorkers.Contains(worker))
        { return; }

        hiredWorkers.Remove(worker);
        availableWorkers.Add(worker);
        workersVisual.UpdateButtonsVisual();

        switch (worker.role)
        {
            case WorkersRole.GRILL:
                GameManager.Instance.GrillManager.FireWorker(worker);
                break;
            case WorkersRole.HOTPLATE:
                GameManager.Instance.HotplateManager.FireWorker(worker);
                break;
            case WorkersRole.CHECKOUT:
                GameManager.Instance.CheckoutManager.FireWorker(worker);
                break;
        }
    }

    public void RenewWorkers()
    {
        var workerToFire = new List<Worker>();
        foreach (var worker in hiredWorkers)
        {
            if (!GameManager.Instance.WalletManager.HasEnoughMoney(worker.pricePerDay))
            {
                workerToFire.Add(worker);
                continue;
            }

            GameManager.Instance.WalletManager.SpendMoney(worker.pricePerDay);
        }
        foreach (var worker in workerToFire)
        {
            FireWorker(worker);
        }
    }
}