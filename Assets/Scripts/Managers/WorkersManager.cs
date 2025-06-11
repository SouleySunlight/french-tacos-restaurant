using System.Collections.Generic;
using UnityEngine;

public class WorkersManager : MonoBehaviour
{
    [SerializeField] private List<Worker> availableWorkers = new();

    private WorkersVisual workersVisual;

    void Awake()
    {
        workersVisual = FindFirstObjectByType<WorkersVisual>();
    }

    public void SetupWorkers()
    {
        workersVisual.SetupWorkers(availableWorkers);
    }
}