using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TacosMakerManager TacosMakerManager { get; private set; }
    public GrillManager GrillManager { get; private set; }
    public CheckoutManager CheckoutManager { get; private set; }
    public OrdersManager OrdersManager { get; private set; }
    public WalletManager WalletManager { get; private set; }

    [SerializeField] private List<Ingredient> initialAvailableIngredients = new();
    [HideInInspector] public List<Ingredient> AvailableIngredients;

    private void Awake()
    {
        AvailableIngredients = initialAvailableIngredients;
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
            if (!GrillManager.CanAddTacosToGrillWaitingZone())
            {
                throw new NotEnoughSpaceException();
            }
            var wrappedTacos = TacosMakerManager.WrapTacos();
            GrillManager.AddTacosToWaitingZone(wrappedTacos);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void OnTacosGrilled(Tacos tacos)
    {
        CheckoutManager.ReceiveTacosToServe(tacos);

    }

    public void ServeTacos(Tacos tacos)
    {
        CheckoutManager.ServeTacos(tacos);
    }
    public void RefuseTacos()
    {
        CheckoutManager.RefuseTacos();
    }
    public void CompleteOrder(Order order)
    {
        WalletManager.ReceiveMoney(order.price);
    }

    void InitializeManagers()
    {
        TacosMakerManager = GetComponentInChildren<TacosMakerManager>();
        GrillManager = GetComponentInChildren<GrillManager>();
        CheckoutManager = GetComponentInChildren<CheckoutManager>();
        OrdersManager = GetComponentInChildren<OrdersManager>();
        WalletManager = GetComponentInChildren<WalletManager>();


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
        if (CheckoutManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/CheckoutManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load CheckoutManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            CheckoutManager = GetComponentInChildren<CheckoutManager>();
        }
        if (OrdersManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/OrdersManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load OrdersManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            OrdersManager = GetComponentInChildren<OrdersManager>();
        }
        if (WalletManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/WalletManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load WalletManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            WalletManager = GetComponentInChildren<WalletManager>();
        }
    }


}