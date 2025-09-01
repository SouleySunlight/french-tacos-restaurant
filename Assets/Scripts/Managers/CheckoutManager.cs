using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutManager : MonoBehaviour, IWorkStation
{
    private List<Tacos> tacosToServe = new();

    private CheckoutVisual checkoutVisual;

    private static readonly int MAX_TACOS_TO_SERVE = 6;

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

    public void RemoveAllTacos()
    {
        foreach (Tacos tacos in tacosToServe)
        {
            checkoutVisual.RemoveTacosToServe(tacos);
        }
        tacosToServe.Clear();
    }

    public void RefuseTacos()
    {
        GameManager.Instance.HelpTextManager.ShowWrongTacosMessage();
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
        foreach (var tacos in tacosToServe)
        {
            GameManager.Instance.OrdersManager.WorkerTryToServeTacos(tacos);
            if (isWorkerTaskDone)
            {
                return;
            }
        }
    }

    public bool CanAddTacosToCheckout()
    {
        return tacosToServe.Count < MAX_TACOS_TO_SERVE;
    }

    public void MarkWorkerTaskAsDone()
    {
        isWorkerTaskDone = true;
    }

    public void OnEndDrag(GameObject tacos)
    {
        if (tacos.GetComponent<TacosMovemement>().isAboveTrash)
        {
            checkoutVisual.ThrowTacos(tacos);
            GameManager.Instance.SoundManager.PlayTrashSound();
        }
        checkoutVisual.UpdateVisuals();
    }

    public void DiscardTacos(Tacos tacos)
    {
        var tacosToDiscard = tacosToServe.Find(waitingTacos => waitingTacos.guid == tacos.guid);
        if (tacosToDiscard == null) { return; }
        tacosToServe.Remove(tacosToDiscard);
        checkoutVisual.RemoveTacosToServe(tacosToDiscard);

    }
}
