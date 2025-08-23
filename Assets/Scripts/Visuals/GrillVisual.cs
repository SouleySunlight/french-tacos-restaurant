using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrillVisual : MonoBehaviour, IView
{
    [SerializeField] private GameObject tacosToGrillPrefab;
    [SerializeField] private RectTransform grillPosition = new();
    [SerializeField] private GameObject grillTop;
    private List<GameObject> tacosToGrillList = new();
    private List<GameObject> grillingTacos = new();
    [SerializeField] private List<GameObject> completionBars = new();
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip grillOpening;


    public void Setup()
    {
        GetComponent<GrillMovement>().CloseGrill.AddListener(CloseGrill);
        GetComponent<GrillMovement>().OpenGrill.AddListener(OpenGrill);

    }

    public void OnViewDisplayed()
    {
        UpdateVisual();
        UpdateUngrilledTacosVisual();
        if (GameManager.Instance.GrillManager.isGrillOpened)
        {
            animator.Play("OPENED_Grill");
            animator.SetBool("isGrillOpened", true);
            return;
        }
        animator.Play("CLOSED_Grill");
        animator.SetBool("isGrillOpened", false);
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
            rectTransform.anchoredPosition = new Vector2(0, -75);
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


        var rectTransform = tacosToGrill.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.3f + position * 0.4f, 0.175f);
        rectTransform.anchorMax = new Vector2(0.3f + position * 0.4f, 0.175f);
        rectTransform.anchoredPosition = new Vector2(0, 0);


        grillingTacos.Add(tacosToGrill);
    }

    public void UpdateTacosVisual(Tacos tacos)
    {
        var tacosToUpdate = grillingTacos.Find(tacosPrefab => tacosPrefab.GetComponent<TacosDisplayer>().tacosData == tacos);
        tacosToUpdate.GetComponent<TacosDisplayer>().UpdateTacosVisual();

    }

    public void UpdateTimer(int index, float percentage)
    {
        completionBars[index].GetComponent<RoundedCompletionBarDisplayer>().UpdateTimer(percentage);
    }

    public void RemoveTacosOfTheGrill(Tacos tacosToServe, int index)
    {
        var tacosToRemoveIndex = grillingTacos.FindIndex((tacos) => tacos.GetComponent<TacosDisplayer>().tacosData.guid == tacosToServe.guid);
        Destroy(grillingTacos[tacosToRemoveIndex]);
        grillingTacos.RemoveAt(tacosToRemoveIndex);
        completionBars[index].GetComponent<RoundedCompletionBarDisplayer>().UpdateTimer(0);

    }

    public void CloseGrill(GameObject gameObject)
    {
        grillTop.GetComponent<Image>().raycastTarget = true;
        GameManager.Instance.GrillManager.CloseGrill(gameObject);
        GameManager.Instance.SoundManager.PlaySFX(grillOpening);

    }
    public void OpenGrill(GameObject gameObject)
    {
        grillTop.GetComponent<Image>().raycastTarget = false;
        GameManager.Instance.GrillManager.OpenGrill(gameObject);
        GameManager.Instance.SoundManager.PlaySFX(grillOpening);
    }

    public void UpdateAnimation(bool isGrillOpened)
    {
        animator.SetBool("isGrillOpened", isGrillOpened);
    }

    public void RemoveAllTacosFromGrill()
    {
        foreach (GameObject tacos in grillingTacos)
        {
            Destroy(tacos);
        }
        grillingTacos.Clear();
        tacosToGrillList.Clear();
        UpdateVisual();
    }
}
