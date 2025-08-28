using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckoutVisual : MonoBehaviour, IView
{
    [SerializeField] private RectTransform firstTacosPosition;
    [SerializeField] private RectTransform parentPosition;
    [SerializeField] private GameObject tacosPrefab;
    [SerializeField] private GameObject trash;

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
            if (prefab.GetComponent<TacosMovemement>().isAboveTrash)
            {
                continue;
            }
            var position = new Vector3(firstTacosPosition.position.x + (index % 2 * GlobalConstant.TACOS_HORIZONTAL_GAP), firstTacosPosition.position.y + index / 2 * GlobalConstant.TACOS_VERTICAL_GAP, firstTacosPosition.position.z);
            prefab.GetComponent<RectTransform>().position = position;
            index++;
        }
    }

    public void DragTacos(PointerEventData eventData)
    {
        var draggedTacos = eventData.pointerDrag;
        var rectTransform = draggedTacos.GetComponent<RectTransform>();
        var tacosMovemement = draggedTacos.GetComponent<TacosMovemement>();
        float distance = Vector2.Distance(eventData.position, trash.transform.position);

        if (tacosMovemement.isAboveTrash)
        {
            if (distance >= 200)
            {
                RemoveTacosFromAboveTrash(draggedTacos);
                rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
            }
            return;
        }
        if (distance < 200 && !tacosMovemement.isAboveTrash)
        {
            PlaceTacosAboveTrash(draggedTacos);
            return;
        }
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;

    }

    void PlaceTacosAboveTrash(GameObject tacos)
    {
        tacos.GetComponent<TacosMovemement>().isAboveTrash = true;
        var rectTransform = tacos.GetComponent<RectTransform>();

        rectTransform.localScale = Vector3.one * 0.4f;
        rectTransform.position = new(trash.transform.position.x, trash.transform.position.y + 150);

    }

    void RemoveTacosFromAboveTrash(GameObject tacos)
    {
        tacos.GetComponent<TacosMovemement>().isAboveTrash = false;
        var rectTransform = tacos.GetComponent<RectTransform>();

        rectTransform.localScale = Vector3.one;

    }

    public void ThrowTacos(GameObject tacos)
    {
        tacos.GetComponent<TacosMovemement>().ThrowTacos(tacos, trash.GetComponent<RectTransform>());
    }

    public void PutTacosAbove(GameObject tacos)
    {
        tacos.GetComponent<RectTransform>().SetParent(parentPosition);
    }
}
