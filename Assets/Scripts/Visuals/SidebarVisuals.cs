using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SidebarVisuals : MonoBehaviour
{
    [SerializeField] private GameObject sidebuttonPrefab;
    [SerializeField] private List<SidebarOptions> sidebarOptions;

    private readonly int VERTICAL_GAP = -120;
    private readonly int VERTICAL_OFFSET = -220;


    void Awake()
    {
        var index = 0;
        foreach (var sidebarOption in sidebarOptions)
        {
            var createdOption = Instantiate(sidebuttonPrefab, this.transform);
            createdOption.GetComponent<Button>().onClick.AddListener(() => UpdateView(sidebarOption.viewToShow));
            createdOption.GetComponent<SidebarButtonDisplayer>().sidebarOption = sidebarOption;

            var rectTransform = createdOption.GetComponent<RectTransform>();


            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 0f);

            rectTransform.anchoredPosition = new Vector2(0, index * VERTICAL_GAP + VERTICAL_OFFSET);
            index++;
        }

    }

    void Start()
    {
        UpdateView(sidebarOptions[0].viewToShow);
    }

    void UpdateView(ViewToShowEnum viewToShow)
    {
        FindFirstObjectByType<PlayzoneVisual>().DisplayView(viewToShow);
    }
}
