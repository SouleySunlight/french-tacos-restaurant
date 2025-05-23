using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TacosMakerManager TacosMakerManager { get; private set; }
    public GrillManager GrillManager { get; private set; }


    private void Awake()
    {
        InitializeManagers();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    public void WrapTacos()
    {
        try
        {
            if (GrillManager.CanAddTacosToGrill())
            {
                throw new NotEnoughSpaceException();
            }
            var wrappedTacos = TacosMakerManager.WrapTacos();
            GrillManager.ReceiveTacosToGrill(wrappedTacos);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void InitializeManagers()
    {
        TacosMakerManager = GetComponentInChildren<TacosMakerManager>();
        GrillManager = GetComponentInChildren<GrillManager>();

        if (TacosMakerManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/TacosMakerManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load TacosMakerManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            TacosMakerManager = GetComponentInChildren<TacosMakerManager>();
        }

        if (GrillManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/GrillManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load GrillManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            GrillManager = GetComponentInChildren<GrillManager>();
        }
    }


}