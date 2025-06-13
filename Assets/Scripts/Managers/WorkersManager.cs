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

        availableWorkers.Remove(worker);
        hiredWorkers.Add(worker);

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
}