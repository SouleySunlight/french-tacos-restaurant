using System.Collections.Generic;
using UnityEngine;

public class GrillVisual : MonoBehaviour
{
    [SerializeField] private GameObject tacosToGrillPrefab;
    [SerializeField] private RectTransform tacosToGrillFirstTransform;

    private List<GameObject> tacosToGrillList = new();
    private readonly int TACOS_TO_GRILL_HORIZONTAL_GAP = 500;

    public void UpdateVisual()
    {
        UpdateUngrilledTacosVisual();
    }

    public void UpdateUngrilledTacosVisual()
    {
        var index = 0;
        foreach (GameObject prefab in tacosToGrillList)
        {
            var position = new Vector3(tacosToGrillFirstTransform.position.x + index * TACOS_TO_GRILL_HORIZONTAL_GAP, tacosToGrillFirstTransform.position.y, tacosToGrillFirstTransform.position.z);
            prefab.GetComponent<RectTransform>().position = position;
            index++;
        }
    }

    public void ReceiveTacosToGrill(Tacos tacos)
    {
        Debug.Log("coucou");
        var tacosToGrill = Instantiate(tacosToGrillPrefab, tacosToGrillFirstTransform.position, Quaternion.identity, tacosToGrillFirstTransform);
        tacosToGrill.GetComponent<TacosInteraction>().tacosData = tacos;
        tacosToGrillList.Add(tacosToGrill);
        UpdateVisual();
    }
}
