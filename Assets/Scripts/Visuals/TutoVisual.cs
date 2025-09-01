using System.Collections.Generic;
using UnityEngine;

public class TutoVisual : MonoBehaviour
{
    [System.Serializable]
    private class TutoEntry
    {
        public ViewToShowEnum tutoType;
        public GameObject tutoObject;
    }

    [SerializeField] List<TutoEntry> tutoEntries;
    [SerializeField] GameObject window;

    private Dictionary<ViewToShowEnum, GameObject> tutos = new();

    public void ShowTutorial(ViewToShowEnum view)
    {
        foreach (var tuto in tutos)
        {
            tuto.Value.SetActive(tuto.Key == view);
        }
        window.SetActive(true);
    }

    public void HideTutorial()
    {
        window.SetActive(false);
    }

    public void InstantiateDictionary()
    {
        foreach (TutoEntry tutoEntry in tutoEntries)
        {
            tutos[tutoEntry.tutoType] = tutoEntry.tutoObject;
        }
    }
}