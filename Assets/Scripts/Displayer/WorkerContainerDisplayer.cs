using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class WorkerContainerDisplayer : MonoBehaviour
{
    public Worker worker;
    [SerializeField] private TMP_Text workerName;
    [SerializeField] private TMP_Text workerDescription;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private LocalizeStringEvent costPerDayLocalize;
    [SerializeField] private GameObject hireButton;
    [SerializeField] private GameObject hireForADayButton;
    [SerializeField] private GameObject fireButton;

    [SerializeField] private Image containerImage;

    public void UpdateVisuals()
    {
        workerName.text = worker.id;
        workerDescription.text = worker.id;
        cost.text = worker.pricePerDay.ToString();
        costPerDayLocalize.StringReference.Arguments = new object[] { worker.pricePerDay.ToString() };
        costPerDayLocalize.RefreshString();
        UpdateHiredRelativeVisuals();
    }

    public void OnHireButtonClicked()
    {
        GameManager.Instance.WorkersManager.HireWorker(worker);
        UpdateHiredRelativeVisuals();
    }
    public void OnHireForADayButtonClicked()
    {
        GameManager.Instance.WorkersManager.HireForADayWorker(worker);
        UpdateHiredRelativeVisuals();
    }

    public void OnFireButtonClicked()
    {
        GameManager.Instance.WorkersManager.FireWorker(worker);
        UpdateHiredRelativeVisuals();
    }

    void UpdateHiredRelativeVisuals()
    {
        if (GameManager.Instance.WorkersManager.IsWorkerHired(worker))
        {
            hireButton.SetActive(false);
            hireForADayButton.SetActive(false);
            fireButton.SetActive(true);
            Color selectedColor = Colors.GetColorFromHexa(Colors.SELECTED_WORKERS_CONTAINER);
            selectedColor.a = 0.25f;
            containerImage.color = selectedColor;

            return;
        }

        hireButton.SetActive(true);
        hireForADayButton.SetActive(true);
        fireButton.SetActive(false);
        Color unselectedColor = Colors.GetColorFromHexa(Colors.UNSELECTED_WORKERS_CONTAINER);
        unselectedColor.a = 0.5f;
        containerImage.color = unselectedColor;
    }
}