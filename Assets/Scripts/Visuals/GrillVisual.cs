using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GrillVisual : MonoBehaviour, IView
{
    [SerializeField] private GameObject tacosToGrillPrefab;
    [SerializeField] private GameObject trash;

    [SerializeField] private RectTransform grillPosition;
    [SerializeField] private RectTransform parentPosition;

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
        GameManager.Instance.SoundManager.StopAmbient();
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

    public void PutTacosAbove(GameObject tacos)
    {
        var draggingTacos = tacosToGrillList.Find((tacosPrefab) => tacosPrefab == tacos);
        if (!draggingTacos) { return; }
        draggingTacos.transform.SetParent(parentPosition);
    }

    public void UpdateUngrilledTacosVisual()
    {
        var index = 0;
        foreach (GameObject prefab in tacosToGrillList)
        {
            var rectTransform = prefab.GetComponent<RectTransform>();
            if (!prefab.GetComponent<TacosMovemement>().isAboveTrash)
            {
                rectTransform.SetParent(grillPosition);

            }

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

        tacosToGrill.gameObject.transform.SetParent(grillPosition);


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

    public void DiscardTacos(Tacos tacos)
    {
        var tacosToDiscard = tacosToGrillList.Find(tacosToGrill => tacosToGrill.GetComponent<TacosDisplayer>().tacosData.guid == tacos.guid);
        if (tacosToDiscard == null) { return; }
        tacosToGrillList.Remove(tacosToDiscard);
        Destroy(tacosToDiscard);
    }
    public void DiscardBurntTacos(Tacos tacos)
    {
        var tacosToDiscard = grillingTacos.Find(tacosToGrill => tacosToGrill.GetComponent<TacosDisplayer>().tacosData.guid == tacos.guid);
        if (tacosToDiscard == null) { return; }
        grillingTacos.Remove(tacosToDiscard);
        Destroy(tacosToDiscard);
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
}
