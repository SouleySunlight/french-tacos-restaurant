using System.Collections.Generic;
using UnityEngine;

public class WorkersVisual : MonoBehaviour, IView
{
    [SerializeField] private GameObject workerButtonPrefab;
    [SerializeField] private Transform firstButtonPosition;

    private List<GameObject> buttons = new();
    public void SetupWorkers(List<Worker> workers)
    {
        buttons.Clear();
        foreach (Worker worker in workers)
        {
            var buttonPrefab = Instantiate(workerButtonPrefab, firstButtonPosition.position, Quaternion.identity, firstButtonPosition);
            buttonPrefab.GetComponent<WorkersButtonDisplayer>().workerData = worker;
            buttonPrefab.GetComponent<WorkersButtonDisplayer>().AddListener(() => OnClickOnWorkerButton(worker));

            buttons.Add(buttonPrefab);
        }
        UpdateVisual();
    }
    void UpdateVisual()
    {
        var index = 0;
        foreach (var button in buttons)
        {
            var buttonPosition = new Vector3(
                firstButtonPosition.position.x,
                firstButtonPosition.position.y + GlobalConstant.LEGACY_INGREDIENT_BUTTON_VERTICAL_GAP * index,
                firstButtonPosition.position.z
            );
            button.GetComponent<RectTransform>().position = buttonPosition;
            index++;
        }
    }

    void OnClickOnWorkerButton(Worker worker)
    {
        WorkersButtonDisplayer clickedButton = buttons.Find(button => button.GetComponent<WorkersButtonDisplayer>().workerData.id == worker.id)
            .GetComponent<WorkersButtonDisplayer>();
        if (clickedButton != null)
        {
            if (clickedButton.IsWorkerHired())
            {
                GameManager.Instance.WorkersManager.FireWorker(worker);
            }
            else
            {
                GameManager.Instance.WorkersManager.HireWorker(worker);

            }
            clickedButton.SetIsWorkerHired(!clickedButton.IsWorkerHired());
            clickedButton.UpdateVisual();
        }

    }

    public void UpdateButtonsVisual()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<WorkersButtonDisplayer>().UpdateVisual();
        }
    }
}
