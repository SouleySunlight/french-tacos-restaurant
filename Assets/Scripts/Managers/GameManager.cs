using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

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
    public UpgradeManager UpgradeManager { get; private set; }
    public DayCycleManager DayCycleManager { get; private set; }
    public WorkersManager WorkersManager { get; private set; }
    public FryerManager FryerManager { get; private set; }
    public SauceGruyereManager SauceGruyereManager { get; private set; }
    public CompletionBarManager CompletionBarManager { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public BackgroundManager BackgroundManager { get; private set; }
    public SettingsManager SettingsManager { get; private set; }
    public bool isGamePaused = false;
    private bool isLoaded = false;


    private void Awake()
    {
        InitializeManagers();
        Application.targetFrameRate = 30;
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
        LoadSettings();
        LoadGame();
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    public void SaveGame()
    {
        GameSaveData gameSaveData = new()
        {
            playerMoney = WalletManager.GetCurrentAmount(),
            currentDay = DayCycleManager.GetCurrentDay(),
            numberOfTacosServed = CompletionBarManager.GetNumberOfTacosServed(),
            maxNumberOfOrders = OrdersManager.GetMaxNumberOfOrders(),
            tacosPrice = OrdersManager.GetTacosPrice(),
            processedIngredientInventorySaveData = InventoryManager.GetInventoryProcessedIngredientSaveData(),
            unlockedIngredients = InventoryManager.GetUnlockedIngredientIds(),
            upgradeSaveData = UpgradeManager.GetInventorySaveData()
        };

        SaveSystem.Save(gameSaveData);
    }

    void LoadGame()
    {
        GameSaveData data = SaveSystem.Load();
        WalletManager.SetCurrentAmount(data.playerMoney);
        DayCycleManager.SetCurrentDay(data.currentDay == 0 ? 1 : data.currentDay);
        InventoryManager.LoadInventory(data.processedIngredientInventorySaveData, data.unlockedIngredients);
        UpgradeManager.LoadUpgradesFromSaveData(data.upgradeSaveData);
        CompletionBarManager.LoadNumberOfTacosServed(data.numberOfTacosServed);
        OrdersManager.SetMaxNumberOfOrders(data.maxNumberOfOrders);
        OrdersManager.SetTacosPrice(data.tacosPrice);
        isLoaded = true;
    }

    public void SaveSettings()
    {
        SettingsSaveData settingsSaveData = new()
        {
            isSoundOn = SoundManager.areSoundsOn,
            isMusicOn = SoundManager.isMusicOn,
            language = LocalizationSettings.SelectedLocale.Identifier.Code
        };


        SaveSystem.SaveSettings(settingsSaveData);
    }

    void LoadSettings()
    {
        SettingsSaveData settingsSaveData = SaveSystem.LoadSettings();
        SoundManager.LoadIsMusicOn(settingsSaveData.isMusicOn);
        SoundManager.LoadAreSoundsOn(settingsSaveData.isSoundOn);

        var locale = LocalizationSettings.AvailableLocales.GetLocale(settingsSaveData.language);
        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
        }

    }

    private IEnumerator OnGameLoadedCoroutine()
    {
        yield return new WaitUntil(() => isLoaded);
        HotplateManager.SetupIngredients();
        TacosMakerManager.SetupIngredients();
        FryerManager.SetupIngredients();
        SauceGruyereManager.SetupIngredients();
        GrillManager.SetupGrillingTime();
        SauceGruyereManager.UpdateCookingTime();
        DayCycleManager.SetupDayCycle();
        InventoryManager.SetupInventoriesMaxAmount();

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
        if (!CheckoutManager.CanAddTacosToCheckout())
        {
            return;
        }

        GrillManager.RemoveTacosOfTheGrill(tacos);
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

    public void UpgradeElement(UpgradeSlot upgrade)
    {
        if (!WalletManager.HasEnoughMoney(upgrade.upgrade.GetCostAtLevel(upgrade.currentLevel)))
        {
            return;
        }
        WalletManager.SpendMoney(upgrade.upgrade.GetCostAtLevel(upgrade.currentLevel), SpentCategoryEnum.UPGRADE);
        UpgradeManager.UpgradeElement(upgrade.upgrade.id);

    }

    public void ResetViewForNewDay()
    {
        GrillManager.RemoveAllTacos();
        CheckoutManager.RemoveAllTacos();
        TacosMakerManager.DiscardTacos();
        HotplateManager.FinishProcessingIngredients();
        FryerManager.FinishProcessingIngredients();
        SauceGruyereManager.FinishProcessingIngredients();

    }

    public void OnViewChanged()
    {
        UpgradeManager.UpdateUpgradeButtonVisuals();
        WorkersManager.UpdateWorkerModalVisual();
    }

    void OnLocaleChanged(UnityEngine.Localization.Locale newLocale)
    {
        var languageButtons = FindObjectsByType<LanguageButtonDisplayer>(FindObjectsSortMode.None);
        foreach (var languageButton in languageButtons)
        {
            languageButton.GetComponent<LanguageButtonDisplayer>().UpdateVisual();
        }
        FindFirstObjectByType<SoundsButtonDisplayer>().UpdateVisual();
        FindFirstObjectByType<MusicButtonDisplayer>().UpdateVisual();
        WorkersManager.UpdateWorkerModalVisual();



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
        UpgradeManager = GetComponentInChildren<UpgradeManager>();
        DayCycleManager = GetComponentInChildren<DayCycleManager>();
        WorkersManager = GetComponentInChildren<WorkersManager>();
        FryerManager = GetComponentInChildren<FryerManager>();
        SauceGruyereManager = GetComponentInChildren<SauceGruyereManager>();
        CompletionBarManager = GetComponentInChildren<CompletionBarManager>();
        SoundManager = GetComponentInChildren<SoundManager>();
        BackgroundManager = GetComponentInChildren<BackgroundManager>();
        SettingsManager = GetComponentInChildren<SettingsManager>();

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
        if (CompletionBarManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/CompletionBarManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load CompletionBarManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            CompletionBarManager = GetComponentInChildren<CompletionBarManager>();
        }
        if (SoundManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/SoundManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load SoundManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            SoundManager = GetComponentInChildren<SoundManager>();
        }
        if (BackgroundManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/BackgroundManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load BackgroundManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            BackgroundManager = GetComponentInChildren<BackgroundManager>();
        }
        if (SettingsManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/Managers/SettingsManager");
            if (prefab == null)
            {
                Debug.LogError("Unable to load SettingsManager");
                return;
            }
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
            SettingsManager = GetComponentInChildren<SettingsManager>();


        }
    }
}