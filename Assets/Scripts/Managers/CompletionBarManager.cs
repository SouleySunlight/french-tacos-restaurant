using UnityEngine;

public class CompletionBarManager : MonoBehaviour
{
    private CompletionBarVisual completionBarVisual;

    void Awake()
    {
        completionBarVisual = FindFirstObjectByType<CompletionBarVisual>(FindObjectsInactive.Include);
        completionBarVisual.UpdateVisual(0, 100);
    }


}