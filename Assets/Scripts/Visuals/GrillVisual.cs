using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrillVisual : MonoBehaviour, IView
{
    [SerializeField] private GameObject tacosToGrillPrefab;
    [SerializeField] private RectTransform grillPosition = new();
    private List<GameObject> tacosToGrillList = new();
    private List<GameObject> grillingTacos = new();
    [SerializeField] private List<RectTransform> grillTransforms = new();
    [SerializeField] private List<Image> grillTimers = new();
    [SerializeField] private Animator animator;


    public void Setup()
    {
        GetComponent<GrillMovement>().CloseGrill.AddListener(CloseGrill);
        GetComponent<GrillMovement>().OpenGrill.AddListener(OpenGrill);

    }

    public void UpdateVisual()
    {
        UpdateUngrilledTacosVisual();
    }

    public void UpdateUngrilledTacosVisual()
    {
        var index = 0;
        foreach (GameObject prefab in tacosToGrillList)
        {
            var rectTransform = prefab.GetComponent<RectTransform>();



            rectTransform.anchorMin = new Vector2(0.3f + index * 0.4f, 1);
            rectTransform.anchorMax = new Vector2(0.3f + index * 0.4f, 1);
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0, 0);
            index++;
        }
    }

    public void ReceiveTacosToGrill(Tacos tacos)
    {
        var tacosToGrill = Instantiate(tacosToGrillPrefab, grillPosition);
        tacosToGrill.GetComponent<TacosMovemement>().ClickEventGrill.AddListener(OnClickOnTacos);
        tacosToGrill.GetComponent<TacosDisplayer>().tacosData = tacos;
        tacosToGrillList.Add(tacosToGrill);
        UpdateVisual();
    }

    void OnClickOnTacos(GameObject gameObject)
    {
        FindFirstObjectByType<GrillManager>().OnClickOnTacos(gameObject.GetComponent<TacosDisplayer>().tacosData);
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

    public void RemoveTacosOfTheGrill(Tacos tacosToServe, int index)
    {
        var tacosToRemoveIndex = grillingTacos.FindIndex((tacos) => tacos.GetComponent<TacosDisplayer>().tacosData.guid == tacosToServe.guid);
        Destroy(grillingTacos[tacosToRemoveIndex]);
        grillingTacos.RemoveAt(tacosToRemoveIndex);
        grillTimers[index].fillAmount = 0;

    }

    public void CloseGrill(GameObject gameObject)
    {
        GameManager.Instance.GrillManager.CloseGrill(gameObject);
    }
    public void OpenGrill(GameObject gameObject)
    {
        GameManager.Instance.GrillManager.OpenGrill(gameObject);
    }

    public void UpdateAnimation(bool isGrillOpened)
    {
        animator.SetBool("isGrillOpened", isGrillOpened);
    }
}
