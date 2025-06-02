using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrillManager : MonoBehaviour
{
    private List<Tacos> waitingToGrillTacos = new();
    private List<Tacos> grillingTacos = new();
    private List<float> grillingTime = new();
    private readonly int MAX_WAITING_TO_GRILL_TACOS = 2;
    private readonly int MAX_GRILLING_TACOS = 2;
    private readonly float GRILL_DURATION = 10f;
    private readonly float BURN_DURATION = 20f;



    private GrillVisual grillVisual;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        grillVisual = FindFirstObjectByType<GrillVisual>(FindObjectsInactive.Include);
        for (int i = 0; i < MAX_GRILLING_TACOS; i++)
        {
            grillingTacos.Add(null);
            grillingTime.Add(GlobalConstant.UNUSED_TIME_VALUE);
        }
    }

    void Update()
    {
        for (int i = 0; i < grillingTime.Count; i++)
        {
            if (grillingTime[i] == -10f) { continue; }

            grillingTime[i] += Time.deltaTime;

            if (grillingTime[i] >= BURN_DURATION && !grillingTacos[i].isBurnt)
            {
                grillingTacos[i].BurnTacos();
                grillVisual.UpdateTacosVisual(grillingTacos[i]);
                continue;
            }


            if (grillingTime[i] >= GRILL_DURATION && !grillingTacos[i].isGrilled)
            {
                grillingTacos[i].GrillTacos();
                grillVisual.UpdateTacosVisual(grillingTacos[i]);
                continue;
            }

            grillVisual.UpdateTimer(i, grillingTime[i] / GRILL_DURATION);

        }
    }

    public void AddTacosToWaitingZone(Tacos tacos)
    {
        waitingToGrillTacos.Add(tacos);
        grillVisual.ReceiveTacosToGrill(tacos);
    }

    public bool CanAddTacosToGrillWaitingZone()
    {
        return waitingToGrillTacos.Count < MAX_WAITING_TO_GRILL_TACOS;
    }

    public void OnClickOnTacos(Tacos tacos)
    {
        // try
        // {
        if (waitingToGrillTacos.Contains(tacos))
        {
            AddTacosToGrill(tacos);
            return;
        }

        if (tacos.isGrilled || tacos.isBurnt)
        {
            ServeTacos(tacos);
            return;
        }

        if (CanAddTacosToGrillWaitingZone())
        {
            RemoveTacosOfTheGrill(tacos);
            AddTacosToWaitingZone(tacos);
            return;
        }

        // }
        // catch (Exception e)
        // {
        //     Debug.Log(e);
        // }

    }

    public void AddTacosToGrill(Tacos tacos)
    {
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

    void ServeTacos(Tacos tacos)
    {
        RemoveTacosOfTheGrill(tacos);
        gameManager.OnTacosGrilled(tacos);

    }

    void RemoveTacosOfTheGrill(Tacos tacos)
    {
        var tacosToRemoveIndex = grillingTacos.FindIndex((grillTacos) => grillTacos != null && grillTacos.guid == tacos.guid);
        grillingTacos[tacosToRemoveIndex] = null;
        grillingTime[tacosToRemoveIndex] = GlobalConstant.UNUSED_TIME_VALUE;
        grillVisual.RemoveTacosOfTheGrill(tacos, tacosToRemoveIndex);

    }
}
