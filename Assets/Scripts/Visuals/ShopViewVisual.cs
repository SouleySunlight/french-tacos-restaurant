using System.Collections.Generic;
using UnityEngine;

public class ShopViewVisual : MonoBehaviour
{
    [System.Serializable]
    private class ViewEntry
    {
        public ShopViewEnum viewType;
        public GameObject viewObject;
    }


    [SerializeField] private List<ViewEntry> viewEntries;
    private Dictionary<ShopViewEnum, GameObject> views = new();

    public static ShopViewEnum currentView { get; private set; }

    void Awake()
    {
        foreach (ViewEntry viewEntry in viewEntries)
        {
            views[viewEntry.viewType] = viewEntry.viewObject;
        }
    }


    public void DisplayView(ShopViewEnum zoneToShow)
    {
        currentView = zoneToShow;
        views[zoneToShow].GetComponent<IView>().OnViewDisplayed();
        foreach (var view in views)
        {
            view.Value.SetActive(zoneToShow == view.Key);
        }
    }

}