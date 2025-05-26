using System.Collections.Generic;
using UnityEngine;

public class CheckoutManager : MonoBehaviour
{
    private List<Tacos> tacosToServe = new();

    private CheckoutVisual checkoutVisual;

    void Awake()
    {
        checkoutVisual = FindFirstObjectByType<CheckoutVisual>();
    }

    public void ReceiveTacosToServe(Tacos tacos)
    {
        tacosToServe.Add(tacos);
        checkoutVisual.AddTacosToServe(tacos);


    }
}
