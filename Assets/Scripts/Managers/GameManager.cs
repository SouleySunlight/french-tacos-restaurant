using System;
using System.Collections;
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
    public HotplateManager HotplateManager { get; private set; }
    public InventoryManager InventoryManager { get; private set; }
    public ShopManager ShopManager { get; private set; }
    public UpgradeManager UpgradeManager { get; private set; }
    public DayCycleManager DayCycleManager { get; private set; }
    public WorkersManager WorkersManager { get; private set; }
    public FryerManager FryerManager { get; private set; }
    public SauceGruyereManager SauceGruyereManager { get; private set; }

    public bool isGamePaused { get; private set; } = false;
    private bool isLoaded = false;


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

    void Start()
    {
        StartCoroutine(OnGameLoadedCoroutine());
        LoadGame();
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        GameSaveData gameSaveData = new()
        {
            playerMoney = WalletManager.GetCurrentAmount(),
            currentDay = DayCycleManager.GetCurrentDay(),
            inventorySaveData = InventoryManager.GetInventorySaveData(),
            unprocessedInventorySaveData = InventoryManager.GetUnprocessedInventorySaveData(),
            upgradeSaveData = UpgradeManager.GetInventorySaveData()
        };

        SaveSystem.Save(gameSaveData);
    }

    void LoadGame()
    {
        GameSaveData data = SaveSystem.Load();
        WalletManager.SetCurrentAmount(data.playerMoney);
        DayCycleManager.SetCurrentDay(data.currentDay == 0 ? 1 : data.currentDay);
        InventoryManager.LoadInventoryFromSaveData(data.inventorySaveData);
        InventoryManager.LoadUnprocessedInventoryFromSaveData(data.unprocessedInventorySaveData);
        UpgradeManager.LoadUpgradesFromSaveData(data.upgradeSaveData);
        isLoaded = true;
    }

    private IEnumerator OnGameLoadedCoroutine()
    {
        yield return new WaitUntil(() => isLoaded);
        HotplateManager.SetupIngredients();
        TacosMakerManager.SetupIngredients();
        FryerManager.SetupIngredients();
        SauceGruyereManager.SetupIngredients();
        ShopManager.SetupShop();
        UpgradeManager.SetupUpgrades();
        GrillManager.SetupGrillingTime();
        DayCycleManager.SetupDayCycle();
        InventoryManager.SetupInventoriesMaxAmount();
        WorkersManager.SetupWorkers();

    }

    public void PauseGame()
    {
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
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
        SaveGame();
    }

    public void UnlockIngredient(Ingredient ingredient)
    {
        if (!WalletManager.HasEnoughMoney(ingredient.priceToUnlock))
        {
            return;
        }
        WalletManager.SpendMoney(ingredient.priceToUnlock);
        ShopManager.UnlockIngredient(ingredient);
        TacosMakerManager.AddAvailableIngredient(ingredient);
        HotplateManager.AddAvailableIngredient(ingredient);
    }

    public void RefillIngredient(Ingredient ingredient)
    {
        if (!WalletManager.HasEnoughMoney(ingredient.priceToRefill))
        {
            return;
        }
        WalletManager.SpendMoney(ingredient.priceToRefill);
        ShopManager.RefillIngredient(ingredient);
    }

    public void UpgradeElement(UpgradeSlot upgrade)
    {
        if (!WalletManager.HasEnoughMoney(upgrade.upgrade.GetCostAtLevel(upgrade.currentLevel)))
        {
            return;
        }
        WalletManager.SpendMoney(upgrade.upgrade.GetCostAtLevel(upgrade.currentLevel));
        UpgradeManager.UpgradeElement(upgrade.upgrade.id);

    }

    void InitializeManagers()
    {
        TacosMakerManager = GetComponentInChildren<TacosMakerManager>();
        GrillManager = GetComponentInChildren<GrillManager>();
        CheckoutManager = GetComponentInChildren<CheckoutManager>();
        OrdersManager = GetComponentInChildren<OrdersManager>();
        WalletManager = GetComponentInChildren<WalletManager>();
        HotplateManager = GetComponentInChildren<HotplateManager>();
        InventoryManager = GetComponentInChildren<InventoryManager>();
        ShopManager = GetComponentInChildren<ShopManager>();
        UpgradeManager = GetComponentInChildren<UpgradeManager>();
        DayCycleManager = GetComponentInChildren<DayCycleManager>();
        WorkersManager = GetComponentInChildren<WorkersManager>();
        FryerManager = GetComponentInChildren<FryerManager>();
        SauceGruyereManager = GetComponentInChildren<SauceGruyereManager>();


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
        if (HotplateManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/HotplateManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load HotplateManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            HotplateManager = GetComponentInChildren<HotplateManager>();
        }
        if (InventoryManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/InventoryManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load InventoryManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            InventoryManager = GetComponentInChildren<InventoryManager>();
        }
        if (ShopManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/ShopManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load ShopManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            ShopManager = GetComponentInChildren<ShopManager>();
        }
        if (UpgradeManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/UpgradeManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load UpgradeManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            UpgradeManager = GetComponentInChildren<UpgradeManager>();
        }
        if (DayCycleManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/DayCycleManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load DayCycleManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            DayCycleManager = GetComponentInChildren<DayCycleManager>();
        }
        if (WorkersManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/WorkersManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load WorkersManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            WorkersManager = GetComponentInChildren<WorkersManager>();
        }
        if (FryerManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/FryerManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load FryerManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            FryerManager = GetComponentInChildren<FryerManager>();
        }
        if (SauceGruyereManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/SauceGruyereManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load SauceGruyereManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            SauceGruyereManager = GetComponentInChildren<SauceGruyereManager>();
        }
    }


}