using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutManager : MonoBehaviour, IWorkStation
{
    private List<Tacos> tacosToServe = new();

    private CheckoutVisual checkoutVisual;


    private Worker currentWorker = null;
    private bool isWorkerTaskDone = false;

    void Awake()
    {
        checkoutVisual = FindFirstObjectByType<CheckoutVisual>(FindObjectsInactive.Include);
    }

    public void ReceiveTacosToServe(Tacos tacos)
    {
        tacosToServe.Add(tacos);
        checkoutVisual.AddTacosToServe(tacos);
    }

    public void ServeTacos(Tacos tacos)
    {
        tacosToServe.Remove(tacos);
        checkoutVisual.RemoveTacosToServe(tacos);
    }

    public void RefuseTacos()
    {
        checkoutVisual.UpdateVisuals();

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
        foreach (var tacos in tacosToServe)
        {
            GameManager.Instance.OrdersManager.WorkerTryToServeTacos(tacos);
            if (isWorkerTaskDone)
            {
                return;
            }
        }
    }

    public void MarkWorkerTaskAsDone()
    {
        isWorkerTaskDone = true;
    }
}
