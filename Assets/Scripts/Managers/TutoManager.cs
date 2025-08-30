using UnityEngine;

public class TutoManager : MonoBehaviour
{
    private TutoVisual tutoVisual;

    void Awake()
    {
        tutoVisual = FindFirstObjectByType<TutoVisual>(FindObjectsInactive.Include);
        tutoVisual.InstantiateDictionary();
    }

    public void ShowTutorial(ViewToShowEnum view)
    {
        tutoVisual.ShowTutorial(ViewToShowEnum.TACOS_MAKER);
    }

    public void HideTutorial()
    {
        tutoVisual.HideTutorial();
    }
}