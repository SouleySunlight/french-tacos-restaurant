using System;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour
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
        MarkTutoAsSeen(view);
        tutoVisual.ShowTutorial(view);
        GameManager.Instance.SaveSettings();
    }

    public void HideTutorial()
    {
        GameManager.Instance.ResumeGame();
        tutoVisual.HideTutorial();
    }

    public void ShowTutoIfNeeded(ViewToShowEnum view)
    {
        if (!tutosViewing.ContainsKey(view))
        {
            return;
        }
        if (tutosViewing[view])
        {
            return;
        }
        ShowTutorial(view);
    }

    public bool HasSeenViewTuto(ViewToShowEnum view)
    {
        return tutosViewing[view];
    }

    public bool MarkTutoAsSeen(ViewToShowEnum view)
    {
        return tutosViewing[view] = true;
    }

    public TutosViewingSaveData GetTutosSaveData()
    {
        var data = new TutosViewingSaveData();
        foreach (var pair in tutosViewing)
        {
            data.slots.Add(new TutosViewingSlotSaveData
            {
                viewId = pair.Key.ToString(),
                hasBeenViewed = pair.Value,
            });
        }
        return data;
    }

    public void LoadTutosData(TutosViewingSaveData data)
    {
        if (data.slots.Count == 0)
        {
            foreach (ViewToShowEnum view in Enum.GetValues(typeof(ViewToShowEnum)))
            {
                tutosViewing[view] = false;
            }
            ShowTutoIfNeeded(ViewToShowEnum.TACOS_MAKER);

            return;
        }

        foreach (var slot in data.slots)
        {
            tutosViewing[(ViewToShowEnum)Enum.Parse(typeof(ViewToShowEnum), slot.viewId)] = slot.hasBeenViewed;
        }

        ShowTutoIfNeeded(ViewToShowEnum.TACOS_MAKER);
    }
}