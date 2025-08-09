using System.Collections.Generic;
using UnityEngine;

public class PlayzoneVisual : MonoBehaviour
{
    [System.Serializable]
    private class ViewEntry
    {
        public ViewToShowEnum viewType;
        public GameObject viewObject;
    }


    [SerializeField] private List<ViewEntry> viewEntries;
    private Dictionary<ViewToShowEnum, GameObject> views = new();

    public static ViewToShowEnum currentView { get; private set; }

    void Awake()
    {
        foreach (ViewEntry viewEntry in viewEntries)
        {
            views[viewEntry.viewType] = viewEntry.viewObject;
        }
    }


    public void DisplayView(ViewToShowEnum zoneToShow)
    {
        currentView = zoneToShow;
        foreach (var view in views)
        {
            view.Value.SetActive(zoneToShow == view.Key);
        }
        views[zoneToShow].GetComponent<IView>().OnViewDisplayed();

    }
}
