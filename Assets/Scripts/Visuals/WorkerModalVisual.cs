using System.Collections.Generic;
using UnityEngine;

public class WorkerModalVisual : MonoBehaviour
{
    [SerializeField] private GameObject workerModal;
    [SerializeField] private RectTransform modalBody;
    [SerializeField] private GameObject workerContainerDisplayer;
    private List<GameObject> workerContainers = new();

    public void UpdateModalContent()
    {
        var workers = GameManager.Instance.WorkersManager.GetAvailableWorkers();
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
}