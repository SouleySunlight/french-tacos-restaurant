using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class WorkerContainerDisplayer : MonoBehaviour
{
    public Worker worker;
    [SerializeField] private TMP_Text workerName;
    [SerializeField] private TMP_Text workerDescription;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private LocalizeStringEvent costPerDayLocalize;

    public void UpdateVisuals()
    {
        workerName.text = worker.id;
        workerDescription.text = worker.id;
        cost.text = worker.pricePerDay.ToString();
        costPerDayLocalize.StringReference.Arguments = new object[] { worker.pricePerDay.ToString() };
        costPerDayLocalize.RefreshString();
    }

    public void OnButtonPressed()
    {
    }
}