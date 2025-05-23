using System;
using System.Collections.Generic;
using UnityEngine;

public class GrillManager : MonoBehaviour
{
    private List<Tacos> waitingToGrillTacos = new();
    private List<Tacos> grillingTacos = new();
    private readonly int MAX_WAITING_TO_GRILL_TACOS = 2;
    private readonly int MAX_GRILLING_TACOS = 2;


    private GrillVisual grillVisual;

    void Awake()
    {
        grillVisual = FindFirstObjectByType<GrillVisual>();
        for (int i = 0; i < MAX_GRILLING_TACOS; i++)
        {
            grillingTacos.Add(null);
        }
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
    public void AddTacosToGrill(Tacos tacos)
    {
        try
        {
            if (!waitingToGrillTacos.Contains(tacos))
            {
                return;
            }

            waitingToGrillTacos.Remove(tacos);

            for (int i = 0; i <= grillingTacos.Count; i++)
            {
                if (grillingTacos[i] == null)
                {
                    grillingTacos[i] = tacos;
                    grillVisual.GrillTacos(tacos, i);
                    return;
                }
            }
            throw new NotEnoughSpaceException();


        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
