using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SidebarVisuals : MonoBehaviour
{
    [SerializeField] private GameObject sidebuttonPrefab;
    [SerializeField] private List<SidebarOptions> sidebarOptions;

    private List<GameObject> sidebarButtons = new();

    private readonly int VERTICAL_GAP = -120;
    private readonly int VERTICAL_OFFSET = -220;


    void Start()
    {
        UpdateView(sidebarOptions[0].viewToShow);
    }

    public void UpdateSidebarVisual()
    {
        var index = 0;
        foreach (var sidebarOption in sidebarOptions)
        {
            var createdOption = Instantiate(sidebuttonPrefab, this.transform);
            createdOption.GetComponent<Button>().onClick.AddListener(() => UpdateView(sidebarOption.viewToShow));
            createdOption.GetComponent<SidebarButtonDisplayer>().sidebarOption = sidebarOption;

            sidebarButtons.Add(createdOption);

            var rectTransform = createdOption.GetComponent<RectTransform>();


            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            rectTransform.anchoredPosition = new Vector2(0, index * VERTICAL_GAP + VERTICAL_OFFSET);
            index++;
        }
        InitializeTimers();
    }

    public void DeactivateAllButtons()
    {
        foreach (var sidebarButton in sidebarButtons)
        {
            sidebarButton.GetComponent<Button>().interactable = false;
        }
    }

    public void ActivateAllButtons()
    {
        foreach (var sidebarButton in sidebarButtons)
        {
            sidebarButton.GetComponent<Button>().interactable = true;
        }
    }

    public void UpdateTimer(ViewToShowEnum viewToShow, float timer)
    {
        var sidebarButton = sidebarButtons.Find(button => button.GetComponent<SidebarButtonDisplayer>().sidebarOption.viewToShow == viewToShow);
        if (sidebarButton == null)
        {
            return;
        }
        sidebarButton.GetComponent<SidebarButtonDisplayer>().UpdateTimer(timer);
    }

    public void InitializeTimers()
    {
        foreach (var sidebarButton in sidebarButtons)
        {
            sidebarButton.GetComponent<SidebarButtonDisplayer>().UpdateTimer(0f);
        }
    }

    void UpdateView(ViewToShowEnum viewToShow)
    {
        FindFirstObjectByType<PlayzoneVisual>().DisplayView(viewToShow);
    }

    public RectTransform GetButtonRectTransform(ViewToShowEnum viewToShow)
    {
        var sidebarButton = sidebarButtons.Find(button => button.GetComponent<SidebarButtonDisplayer>().sidebarOption.viewToShow == viewToShow);
        return sidebarButton?.GetComponent<RectTransform>();
    }
}
