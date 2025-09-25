using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrillManager : MonoBehaviour, IWorkStation
{
    private List<Tacos> waitingToGrillTacos = new();
    private List<Tacos> grillingTacos = new();
    private List<float> grillingTime = new();
    private List<float> totalGrillingTime = new();

    private readonly int MAX_WAITING_TO_GRILL_TACOS = 2;
    private readonly int MAX_GRILLING_TACOS = 2;
    private readonly float GRILL_BASE_DURATION = 20f;
    private readonly float BURN_BASE_DURATION = 10f;
    private float currentGrillDuration;

    public bool canUserRemoveTacos { get; private set; } = true;

    private GrillVisual grillVisual;

    private GameManager gameManager;
    private Worker currentWorker = null;
    private bool isWorkerTaskDone = false;
    public bool isGrillOpened { get; private set; } = true;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        grillVisual = FindFirstObjectByType<GrillVisual>(FindObjectsInactive.Include);
        grillVisual.Setup();
        for (int i = 0; i < MAX_GRILLING_TACOS; i++)
        {
            grillingTacos.Add(null);
            grillingTime.Add(GlobalConstant.UNUSED_TIME_VALUE);
            totalGrillingTime.Add(GlobalConstant.UNUSED_TIME_VALUE);
        }
    }

    void Update()
    {
        if (GameManager.Instance.isGamePaused) { return; }

        SidebarTimer.UpdateSidebarTimer(grillingTime, totalGrillingTime, ViewToShowEnum.GRILL);
        if (isGrillOpened) { return; }
        for (int i = 0; i < grillingTime.Count; i++)
        {
            if (grillingTime[i] == GlobalConstant.UNUSED_TIME_VALUE) { continue; }

            grillingTime[i] += Time.deltaTime;

            if (grillingTime[i] >= totalGrillingTime[i] + BURN_BASE_DURATION && !grillingTacos[i].isBurnt && GameManager.Instance.DayCycleManager.GetCurrentDay() > 0)
            {
                grillingTacos[i].BurnTacos();
                grillVisual.UpdateTacosVisual(grillingTacos[i]);
                continue;
            }


            if (grillingTime[i] >= totalGrillingTime[i] && !grillingTacos[i].isGrilled)
            {
                grillingTacos[i].GrillTacos();
                grillVisual.UpdateTacosVisual(grillingTacos[i]);
                continue;
            }

            var percentage = grillingTime[i] / totalGrillingTime[i];
            grillVisual.UpdateTimer(i, percentage);
        }
    }

    public void AddTacosToWaitingZone(Tacos tacos)
    {
        waitingToGrillTacos.Add(tacos);
        grillVisual.ReceiveTacosToGrill(tacos);
    }

    public bool CanAddTacosToGrillWaitingZone()
    {
        return waitingToGrillTacos.Count < MAX_WAITING_TO_GRILL_TACOS;
    }

    public bool CanAddTacosToGrill()
    {
        for (int i = 0; i < grillingTacos.Count; i++)
        {
            if (grillingTacos[i] == null)
            {
                return true;
            }
        }
        return false;
    }

    public void OnClickOnTacos(Tacos tacos)
    {
        if (!canUserRemoveTacos)
        {
            return;
        }

        try
        {
            if (!isGrillOpened)
            {
                return;
            }

            if (tacos.isBurnt)
            {
                if (GameManager.Instance.DayCycleManager.GetCurrentDay() == 0)
                {
                    ServeTacos(tacos);
                    return;
                }
                DiscardBurntTacos(tacos);
                return;
            }

            if (tacos.isGrilled)
            {
                ServeTacos(tacos);
                return;
            }

            if (CanAddTacosToGrillWaitingZone())
            {
                RemoveTacosOfTheGrill(tacos);
                AddTacosToWaitingZone(tacos);
                return;
            }

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public void AddTacosToGrill(Tacos tacos)
    {
        if (!isGrillOpened)
        {
            return;
        }
        waitingToGrillTacos.Remove(tacos);

        for (int i = 0; i <= grillingTacos.Count; i++)
        {
            if (grillingTacos[i] == null)
            {

                grillingTacos[i] = tacos;
                grillVisual.GrillTacos(tacos, i);
                grillingTime[i] = 0f;
                UpdateGrillingTime();
                totalGrillingTime[i] = currentGrillDuration;
                return;
            }
        }
        throw new NotEnoughSpaceException();


    }

    void ServeTacos(Tacos tacos)
    {
        gameManager.OnTacosGrilled(tacos);

    }

    public void RemoveTacosOfTheGrill(Tacos tacos)
    {
        var tacosToRemoveIndex = grillingTacos.FindIndex((grillTacos) => grillTacos != null && grillTacos.guid == tacos.guid);
        grillingTacos[tacosToRemoveIndex] = null;
        grillingTime[tacosToRemoveIndex] = GlobalConstant.UNUSED_TIME_VALUE;
        totalGrillingTime[tacosToRemoveIndex] = GlobalConstant.UNUSED_TIME_VALUE;
        grillVisual.RemoveTacosOfTheGrill(tacos, tacosToRemoveIndex);

    }

    public void DiscardTacos(Tacos tacos)
    {
        var tacosToDiscard = waitingToGrillTacos.Find(waitingTacos => waitingTacos.guid == tacos.guid);
        if (tacosToDiscard == null) { return; }
        waitingToGrillTacos.Remove(tacosToDiscard);
        grillVisual.DiscardTacos(tacosToDiscard);

    }

    public void DiscardBurntTacos(Tacos tacos)
    {
        GameManager.Instance.SoundManager.PlayTrashSound();
        GameManager.Instance.HelpTextManager.ShowBurntTacosMessage();
        RemoveTacosOfTheGrill(tacos);

    }

    public void UpdateGrillingTime()
    {
        currentGrillDuration = GRILL_BASE_DURATION * GameManager.Instance.UpgradeManager.GetSpeedfactor("GRILL");
    }

    public void SetupGrillingTime()
    {
        UpdateGrillingTime();
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
        WorkerRemoveDoneTacosFromGrill();
        if (isWorkerTaskDone)
        {
            return;
        }
        if (currentWorker.level >= 2)
        {
            WorkerAddTacosToGrill();
        }
    }

    void WorkerRemoveDoneTacosFromGrill()
    {

        if (GetTacosDone().Count == 0) { return; }
        StartCoroutine(WorkerRemoveTacosCoroutine());
        return;

    }

    List<Tacos> GetTacosDone()
    {
        List<Tacos> doneTacos = new();
        for (int i = 0; i < grillingTacos.Count; i++)
        {
            if (grillingTacos[i] == null) { continue; }

            if (grillingTacos[i].isGrilled || grillingTacos[i].isBurnt)
            {
                doneTacos.Add(grillingTacos[i]);
            }
        }
        return doneTacos;
    }

    IEnumerator WorkerRemoveTacosCoroutine()
    {
        if (!isGrillOpened)
        {
            OpenGrill();
            yield return new WaitForSeconds(0.5f);
        }
        foreach (var tacos in GetTacosDone())
        {
            gameManager.OnTacosGrilled(tacos);
            isWorkerTaskDone = true;
        }

        if (currentWorker.level >= 2)
        {
            while (waitingToGrillTacos.Count != 0 && CanAddTacosToGrill())
            {
                AddTacosToGrill(waitingToGrillTacos[0]);
            }
        }
        yield return new WaitForSeconds(0.5f);
        CloseGrill();
    }

    void WorkerAddTacosToGrill()
    {

        if (waitingToGrillTacos.Count == 0) { return; }
        if (!CanAddTacosToGrill()) { return; }
        StartCoroutine(WorkerAddTacosToGrillCoroutine());
        isWorkerTaskDone = true;
    }

    IEnumerator WorkerAddTacosToGrillCoroutine()
    {
        if (!isGrillOpened)
        {
            OpenGrill();
            yield return new WaitForSeconds(0.5f);
        }
        while (waitingToGrillTacos.Count != 0 && CanAddTacosToGrill())
        {
            AddTacosToGrill(waitingToGrillTacos[0]);
        }
        yield return new WaitForSeconds(0.5f);
        CloseGrill();

    }

    void CloseGrill()
    {
        if (isGrillOpened)
        {
            isGrillOpened = false;
            grillVisual.UpdateAnimation(isGrillOpened);
        }
    }
    void OpenGrill()
    {
        if (!isGrillOpened)
        {
            isGrillOpened = true;
            grillVisual.UpdateAnimation(isGrillOpened);
        }
    }

    public void CloseGrill(GameObject gameObject)
    {
        if (isGrillOpened)
        {
            isGrillOpened = false;
            grillVisual.UpdateAnimation(isGrillOpened);
        }
    }

    public void OpenGrill(GameObject gameObject)
    {
        if (!isGrillOpened)
        {
            isGrillOpened = true;
            grillVisual.UpdateAnimation(isGrillOpened);
        }
    }

    public void RemoveAllTacos()
    {
        for (int i = 0; i < grillingTacos.Count; i++)
        {
            if (grillingTacos[i] != null)
            {
                RemoveTacosOfTheGrill(grillingTacos[i]);
            }
        }
        waitingToGrillTacos.Clear();
        grillVisual.RemoveAllTacosFromGrill();
        grillVisual.UpdateVisual();

    }
    public void OnEndDrag(GameObject tacos)
    {
        if (tacos.GetComponent<TacosMovemement>().isAboveTrash)
        {
            grillVisual.ThrowTacos(tacos);
            GameManager.Instance.SoundManager.PlayTrashSound();
        }
        grillVisual.UpdateVisual();
    }

    public bool ContainsAtLeastOneTacos()
    {
        foreach (var tacos in grillingTacos)
        {
            if (tacos != null)
            {
                return true;
            }
        }
        return false;
    }

    public RectTransform GetFirstTacosTransform()
    {
        return grillVisual.GetFirstTacosTransform();
    }

    public RectTransform GetGrillPosition()
    {
        return grillVisual.GetGrillPosition();
    }

    public void EnableUserToRemoveTacos()
    {
        canUserRemoveTacos = true;
    }

    public void DisableUserToRemoveTacos()
    {
        canUserRemoveTacos = false;
    }

    public void PreventUserFromOpeningOrClosingGrill()
    {
        grillVisual.PreventUserFromOpeningOrClosingGrill();
    }

    public void AllowUserToOpenOrCloseGrill()
    {
        grillVisual.AllowUserToOpenOrCloseGrill();
    }

    public bool IsFirstTacosGrilled()
    {
        if (grillingTacos[0] == null) { return false; }
        return grillingTacos[0].isGrilled;
    }

    public RectTransform GetFirstGrillingTacosTransform()
    {
        return grillVisual.GetFirstGrillingTacosTransform();
    }

}
