using System.Collections.Generic;
using UnityEngine;

public class GrillManager : MonoBehaviour
{
    private List<Tacos> waitingToGrillTacos = new();

    private readonly int MAX_WAITING_TO_GRILL_TACOS = 2;

    private GrillVisual grillVisual;

    void Awake()
    {
        grillVisual = FindFirstObjectByType<GrillVisual>();
    }

    public void ReceiveTacosToGrill(Tacos tacos)
    {
        waitingToGrillTacos.Add(tacos);
        grillVisual.ReceiveTacosToGrill(tacos);
    }

    public bool CanAddTacosToGrill()
    {
        return waitingToGrillTacos.Count >= MAX_WAITING_TO_GRILL_TACOS;
    }
}
