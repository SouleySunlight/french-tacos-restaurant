using System.Collections.Generic;
using UnityEngine;

public class CheckoutVisual : MonoBehaviour
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

    void UpdateVisuals()
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
