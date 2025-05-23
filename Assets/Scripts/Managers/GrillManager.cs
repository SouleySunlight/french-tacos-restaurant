using System;
using System.Collections.Generic;
using UnityEngine;

public class GrillManager : MonoBehaviour
{
    private List<Tacos> waitingToGrillTacos = new();
    private List<Tacos> grillingTacos = new();
    private List<float> grillingTime = new();
    private readonly int MAX_WAITING_TO_GRILL_TACOS = 2;
    private readonly int MAX_GRILLING_TACOS = 2;
    private readonly float UNUSED_TIME_VALUE = -10f;
    private readonly float GRILL_DURATION = 10f;


    private GrillVisual grillVisual;

    void Awake()
    {
        grillVisual = FindFirstObjectByType<GrillVisual>();
        for (int i = 0; i < MAX_GRILLING_TACOS; i++)
        {
            grillingTacos.Add(null);
            grillingTime.Add(UNUSED_TIME_VALUE);
        }
    }

    void Update()
    {
        for (int i = 0; i < grillingTime.Count; i++)
        {
            if (grillingTime[i] == -10f) { continue; }

            grillingTime[i] += Time.deltaTime;

            if (grillingTime[i] >= GRILL_DURATION && !grillingTacos[i].isGrilled)
            {
                grillingTacos[i].GrillTacos();
                grillVisual.UpdateTacosVisual(grillingTacos[i]);
                continue;
            }

            grillVisual.UpdateTimer(i, grillingTime[i] / GRILL_DURATION);

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
                    grillingTime[i] = 0f;
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
