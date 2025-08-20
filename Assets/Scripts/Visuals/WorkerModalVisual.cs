using UnityEngine;

public class WorkerModalVisual : MonoBehaviour
{
    [SerializeField] private GameObject workerModal;

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