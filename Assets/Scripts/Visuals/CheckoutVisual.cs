using System.Collections.Generic;
using UnityEngine;

public class CheckoutVisual : MonoBehaviour, IView
{
    [SerializeField] private RectTransform firstTacosPosition;
    [SerializeField] private GameObject tacosPrefab;

    private List<GameObject> tacosToServe = new();


    public void AddTacosToServe(Tacos tacos)
    {
        var newTacos = Instantiate(tacosPrefab, firstTacosPosition.position, Quaternion.identity, firstTacosPosition);
        newTacos.GetComponent<TacosDisplayer>().tacosData = tacos;
        tacosToServe.Add(newTacos);
        UpdateVisuals();
    }

    public void RemoveTacosToServe(Tacos tacos)
    {
        var tacosToRemove = tacosToServe.Find(tacosPrefab => tacosPrefab.GetComponent<TacosDisplayer>().tacosData.guid == tacos.guid);
        if (tacosToRemove != null)
        {
            Destroy(tacosToRemove);
            tacosToServe.Remove(tacosToRemove);
        }
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        var index = 0;
        foreach (GameObject prefab in tacosToServe)
        {
            var position = new Vector3(firstTacosPosition.position.x + (index % 2 * GlobalConstant.TACOS_HORIZONTAL_GAP), firstTacosPosition.position.y + index / 2 * GlobalConstant.TACOS_VERTICAL_GAP, firstTacosPosition.position.z);
            prefab.GetComponent<RectTransform>().position = position;
            index++;
        }
    }
}
