using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrillVisual : MonoBehaviour
{
    [SerializeField] private GameObject tacosToGrillPrefab;
    [SerializeField] private RectTransform tacosToGrillFirstTransform;
    private List<GameObject> tacosToGrillList = new();
    private List<GameObject> grillingTacos = new();
    [SerializeField] private List<RectTransform> grillTransforms = new();
    [SerializeField] private List<Image> grillTimers = new();
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
        var tacosToGrill = Instantiate(tacosToGrillPrefab, tacosToGrillFirstTransform.position, Quaternion.identity, tacosToGrillFirstTransform);
        tacosToGrill.GetComponent<TacosMovemement>().ClickEventGrill.AddListener(OnClickOnTacos);
        tacosToGrill.GetComponent<TacosDisplayer>().tacosData = tacos;
        tacosToGrillList.Add(tacosToGrill);
        UpdateVisual();
    }

    void OnClickOnTacos(GameObject gameObject)
    {
        FindFirstObjectByType<GrillManager>().AddTacosToGrill(gameObject.GetComponent<TacosDisplayer>().tacosData);
    }

    public void GrillTacos(Tacos tacos, int position)
    {
        var tacosToGrill = tacosToGrillList.Find(tacosPrefab => tacosPrefab.GetComponent<TacosDisplayer>().tacosData == tacos);
        tacosToGrillList.Remove(tacosToGrill);
        tacosToGrill.GetComponent<RectTransform>().position = grillTransforms[position].position;
        grillingTacos.Add(tacosToGrill);
    }

    public void UpdateTacosVisual(Tacos tacos)
    {
        var tacosToUpdate = grillingTacos.Find(tacosPrefab => tacosPrefab.GetComponent<TacosDisplayer>().tacosData == tacos);
        tacosToUpdate.GetComponent<TacosDisplayer>().UpdateTacosVisual();

    }

    public void UpdateTimer(int index, float percentage)
    {
        grillTimers[index].fillAmount = percentage;
    }
}
