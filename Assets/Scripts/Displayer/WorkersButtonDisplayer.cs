using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WorkersButtonDisplayer : MonoBehaviour
{
    public Worker workerData;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button button;

    private bool isWorkerHired = false;

    void Start()
    {
        UpdateVisual();
    }
    public void UpdateVisual()
    {
        descriptionText.text = workerData.role + " - LVL " + workerData.level + " - " + workerData.pricePerDay + " €";
        button.GetComponentInChildren<TMP_Text>().text = isWorkerHired ? "Fire" : "Hire";
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    public bool IsWorkerHired()
    {
        return isWorkerHired;
    }
    public void SetIsWorkerHired(bool isHired)
    {
        isWorkerHired = isHired;
    }

}