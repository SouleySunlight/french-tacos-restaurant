using System.Collections.Generic;
using UnityEngine;

public class CheckoutManager : MonoBehaviour
{
    private List<Tacos> tacosToServe = new();

    private CheckoutVisual checkoutVisual;

    void Awake()
    {
        checkoutVisual = FindFirstObjectByType<CheckoutVisual>(FindObjectsInactive.Include);
    }

    public void ReceiveTacosToServe(Tacos tacos)
    {
        tacosToServe.Add(tacos);
        checkoutVisual.AddTacosToServe(tacos);
    }

    public void ServeTacos(Tacos tacos)
    {
        tacosToServe.Remove(tacos);
        checkoutVisual.RemoveTacosToServe(tacos);
    }

    public void RefuseTacos()
    {
        checkoutVisual.UpdateVisuals();

    }
}
