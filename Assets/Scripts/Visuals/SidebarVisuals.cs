using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SidebarVisuals : MonoBehaviour
{
    [SerializeField] private GameObject sidebuttonPrefab;
    [SerializeField] private List<SidebarOptions> sidebarOptions;
    [SerializeField] private RectTransform firstButtonTransform;

    private readonly int VERTICAL_GAP = 200;


    void Awake()
    {
        var index = 0;
        foreach (var sidebarOption in sidebarOptions)
        {
            var position = new Vector3(firstButtonTransform.position.x + index * VERTICAL_GAP, firstButtonTransform.position.y, firstButtonTransform.position.z);
            var createdOption = Instantiate(sidebuttonPrefab, position, Quaternion.identity, firstButtonTransform);
            createdOption.GetComponent<Button>().onClick.AddListener(() => UpdateView(sidebarOption.viewToShow));
            createdOption.GetComponentInChildren<TMP_Text>().text = sidebarOption.name;

        }

        UpdateView(sidebarOptions[0].viewToShow);
    }

    void UpdateView(ViewToShowEnum viewToShow)
    {
        FindFirstObjectByType<PlayzoneVisual>().DisplayView(viewToShow);
    }
}
