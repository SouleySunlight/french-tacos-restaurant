using System.Collections.Generic;
using UnityEngine;

public class WorkersManager : MonoBehaviour
{
    [SerializeField] private List<Worker> availableWorkers = new();
    private List<Worker> hiredWorkers = new();
    private WorkerModalVisual workerModalVisual;
    private WorkersButtonDisplayer workersButtonDisplayer;

    void Awake()
    {
        workerModalVisual = FindFirstObjectByType<WorkerModalVisual>(FindObjectsInactive.Include);
        workersButtonDisplayer = FindFirstObjectByType<WorkersButtonDisplayer>(FindObjectsInactive.Include);
    }


    public void HireWorker(Worker worker)
    {
        if (!availableWorkers.Contains(worker))
        { return; }

        if (!GameManager.Instance.WalletManager.HasEnoughMoney(worker.pricePerDay))
        { return; }

        availableWorkers.Remove(worker);
        hiredWorkers.Add(worker);
        GameManager.Instance.WalletManager.SpendMoney(worker.pricePerDay, SpentCategoryEnum.WORKERS);

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
            case WorkersRole.FRYER:
                GameManager.Instance.FryerManager.HireWorker(worker);
                break;
            case WorkersRole.GRUYERE:
                GameManager.Instance.SauceGruyereManager.HireWorker(worker);
                break;
        }
    }

    public void FireWorker(Worker worker)
    {
        if (!hiredWorkers.Contains(worker))
        { return; }

        hiredWorkers.Remove(worker);
        availableWorkers.Add(worker);

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
            case WorkersRole.FRYER:
                GameManager.Instance.FryerManager.FireWorker(worker);
                break;
            case WorkersRole.GRUYERE:
                GameManager.Instance.SauceGruyereManager.FireWorker(worker);
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

            GameManager.Instance.WalletManager.SpendMoney(worker.pricePerDay, SpentCategoryEnum.WORKERS);
        }
        foreach (var worker in workerToFire)
        {
            FireWorker(worker);
        }
    }

    public void ShowWorkerModal()
    {
        workerModalVisual.ShowWorkerModal();
    }

    public void UpdateWorkerModalVisual()
    {
        workerModalVisual.UpdateModalContent();
        workersButtonDisplayer.UpdateVisual();
    }

    public List<Worker> GetAvailableWorkers()
    {
        return availableWorkers;
    }

    public List<Worker> GetWorkersByType(WorkersRole role)
    {
        List<Worker> workersByType = new();
        foreach (var worker in availableWorkers)
        {
            if (worker.role == role)
            {
                workersByType.Add(worker);
            }
        }
        return workersByType;
    }
}