using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class WorkerModalVisual : MonoBehaviour
{
    [SerializeField] private GameObject workerModal;
    [SerializeField] private RectTransform modalBody;
    [SerializeField] private GameObject workerContainerDisplayer;
    [SerializeField] private GameObject watchAdContainer;
    [SerializeField] private GameObject adWatchedContainer;

    private List<GameObject> workerContainers = new();

    public void UpdateModalContent()
    {
        var role = GameManager.Instance.WorkersManager.GetRoleByView();
        var workers = GameManager.Instance.WorkersManager.GetWorkersByType(role);
        foreach (var container in workerContainers)
        {
            Destroy(container);
        }
        workerContainers.Clear();
        foreach (var worker in workers)
        {
            var workerContainer = Instantiate(workerContainerDisplayer, modalBody);
            workerContainers.Add(workerContainer);
            workerContainer.GetComponent<WorkerContainerDisplayer>().worker = worker;
            workerContainer.GetComponent<WorkerContainerDisplayer>().UpdateVisuals();
        }
        PlaceContainers();
        DisplayAdRelatedContainer();
    }

    public void PlaceContainers()
    {
        for (int i = 0; i < workerContainers.Count; i++)
        {
            var container = workerContainers[i];
            container.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300 - i * 200);
        }
    }

    public void ShowWorkerModal()
    {
        GameManager.Instance.isGamePaused = true;
        workerModal.SetActive(true);
    }

    public void HideWorkerModal()
    {
        GameManager.Instance.isGamePaused = false;
        workerModal.SetActive(false);
    }

    public void UpdateContainerHiredRelatedVisual()
    {
        foreach (var container in workerContainers)
        {
            container.GetComponent<WorkerContainerDisplayer>().UpdateHiredRelativeVisuals();
        }
    }

    public void DisplayAdRelatedContainer()
    {
        if (GameManager.Instance.WorkersManager.hiredWorkerViaAd == null)
        {
            DisplayWatchAdContainer();
            return;
        }
        DisplayAdWatchedContainer();
    }

    void DisplayWatchAdContainer()
    {
        watchAdContainer.SetActive(true);
        adWatchedContainer.SetActive(false);
    }

    void DisplayAdWatchedContainer()
    {
        watchAdContainer.SetActive(false);
        adWatchedContainer.SetActive(true);
        var workerNameKey = LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "WORKERS.TITLE." + GameManager.Instance.WorkersManager.hiredWorkerViaAd.id);
        adWatchedContainer.GetComponentInChildren<TMP_Text>().text = string.Format(LocalizationSettings.StringDatabase
            .GetLocalizedString("UI_Texts", "WORKER.UNLOCKED_WORKER"), workerNameKey);
    }
}