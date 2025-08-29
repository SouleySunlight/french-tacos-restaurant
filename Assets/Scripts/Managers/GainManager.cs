using UnityEngine;

public class GainManager : MonoBehaviour
{
    private GainVisual gainVisual;

    void Awake()
    {
        gainVisual = FindFirstObjectByType<GainVisual>(FindObjectsInactive.Include);
    }

    public void CreateNewGain(Sprite sprite, int quantity)
    {
        gainVisual.CreateNewGain(sprite, quantity);
    }
}