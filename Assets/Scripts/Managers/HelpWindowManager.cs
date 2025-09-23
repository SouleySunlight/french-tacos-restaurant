using System;
using System.Collections.Generic;
using UnityEngine;

public class HelpWindowManager : MonoBehaviour
{
    private TutoVisual tutoVisual;
    private Dictionary<ViewToShowEnum, bool> tutosViewing = new();

    void Awake()
    {
        tutoVisual = FindFirstObjectByType<TutoVisual>(FindObjectsInactive.Include);
        tutoVisual.InstantiateDictionary();
    }

    public void ShowTutorial(ViewToShowEnum view)
    {
        GameManager.Instance.PauseGame();
        tutoVisual.ShowTutorial(view);
    }

    public void HideTutorial()
    {
        GameManager.Instance.ResumeGame();
        tutoVisual.HideTutorial();
    }
}