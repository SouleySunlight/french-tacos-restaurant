using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] private List<TutorialStep> dayZeroSteps;
    private TutorialPanelVisual tutorialPanelVisual;
    private TutorialMessageDisplayer tutorialMessageDisplayer;
    private TutoCursorDisplayer tutorialCursorDisplayer;
    private int currentStep = 0;

    private bool isNextButtonClicked = false;

    void Awake()
    {
        tutorialPanelVisual = FindFirstObjectByType<TutorialPanelVisual>(FindObjectsInactive.Include);
        tutorialMessageDisplayer = FindFirstObjectByType<TutorialMessageDisplayer>(FindObjectsInactive.Include);
        tutorialCursorDisplayer = FindFirstObjectByType<TutoCursorDisplayer>(FindObjectsInactive.Include);
    }

    public void StartDayZeroTutorial()
    {
        currentStep = -1;
        PlayNextStep();
    }

    void PlayNextStep()
    {
        currentStep++;
        tutorialMessageDisplayer.HideMessage();

        if (currentStep >= dayZeroSteps.Count)
        {
            tutorialPanelVisual.HidePanel();
            return;
        }

        var step = dayZeroSteps[currentStep];
        switch (step.tutorialStepType)
        {
            case TutorialStepType.MOVE_TO_WINDOW:
                MoveToWindowAction(step);
                break;
            case TutorialStepType.ADD_ORDER:
                AddOrderAction(step);
                break;
            case TutorialStepType.SHOW_MESSAGE:
                ShowMessageAction(step);
                break;
            case TutorialStepType.ADD_INGREDIENT_TO_HOTPLATE:
                AddIngredientToHotplateAction(step);
                break;
            case TutorialStepType.REMOVE_INGREDIENT_FROM_HOTPLATE:
                RemoveIngredientFromHotplateAction(step);
                break;
            case TutorialStepType.PREPARE_TACOS:
                PrepareTacosAction(step);
                break;
            case TutorialStepType.PUT_TACOS_TO_GRILL:
                PutTacosToGrillAction(step);
                break;
            case TutorialStepType.GRILL_TACOS:
                GrillTacos(step);
                break;
            case TutorialStepType.REMOVE_GRILLED_TACOS:
                RemoveGrilledTacosAction(step);
                break;
            case TutorialStepType.SERVE_TACOS:
                ServeTacosAction(step);
                break;
            case TutorialStepType.ADD_INGREDIENT_TO_FRYER:
                AddIngredientToFryerAction(step);
                break;
            case TutorialStepType.REMOVE_INGREDIENT_FROM_FRYER:
                RemoveIngredientToFryerAction(step);
                break;
            case TutorialStepType.ADD_INGREDIENT_TO_GRUYERE_POT:
                AddIngredientToSauceGruyereAction(step);
                break;
            case TutorialStepType.REMOVE_INGREDIENT_FROM_GRUYERE_POT:
                RemoveIngredientToSauceGruyereAction(step);
                break;
            case TutorialStepType.FINISH_DAY:
                GameManager.Instance.DayCycleManager.TryToFinishDay();
                break;
            default:
                Debug.LogWarning("Tutorial step type not handled: " + step.tutorialStepType);
                break;
        }
    }

    void MoveToWindowAction(TutorialStep step)
    {
        if (PlayzoneVisual.currentView == step.viewToShow)
        {
            return;
        }
        StartCoroutine(MoveToWindowCoroutine(step));
    }

    void AddOrderAction(TutorialStep step)
    {
        GameManager.Instance.OrdersManager.AddNewOrder(new Order(step.order));
        if (string.IsNullOrEmpty(step.messagekey))
        {
            PlayNextStep();
            return;
        }
        var target = GameManager.Instance.OrdersManager.GetFirstOrderTransform();
        tutorialPanelVisual.FocusOn(target, 0.1f);
        StartCoroutine(ShowMessageCoroutine(step));

    }

    void ShowMessageAction(TutorialStep step)
    {
        StartCoroutine(ShowMessageCoroutine(step));
    }
    void AddIngredientToFryerAction(TutorialStep step)
    {
        StartCoroutine(AddIngredientToFryerCoroutine(step));
    }
    void RemoveIngredientToFryerAction(TutorialStep step)
    {
        StartCoroutine(RemoveIngredientToFryerCoroutine(step));
    }
    void AddIngredientToSauceGruyereAction(TutorialStep step)
    {
        StartCoroutine(AddIngredientToSauceGruyereCoroutine(step));
    }
    void RemoveIngredientToSauceGruyereAction(TutorialStep step)
    {
        StartCoroutine(RemoveIngredientToSauceGruyereCoroutine(step));
    }
    void AddIngredientToHotplateAction(TutorialStep step)
    {
        StartCoroutine(AddIngredientToHotplateCoroutine(step));
    }
    void RemoveIngredientFromHotplateAction(TutorialStep step)
    {
        StartCoroutine(RemoveIngredientsFromHotplateCoroutine(step));
    }

    void PrepareTacosAction(TutorialStep step)
    {
        StartCoroutine(PrepareTacosActionCoroutine(step));
    }

    void PutTacosToGrillAction(TutorialStep step)
    {
        StartCoroutine(PutTacosToGrillCoroutine(step));
    }

    void GrillTacos(TutorialStep step)
    {
        StartCoroutine(GrillTacosCoroutine(step));
    }

    void RemoveGrilledTacosAction(TutorialStep step)
    {
        StartCoroutine(RemoveGrilledTacosActionCoroutine(step));
    }

    void ServeTacosAction(TutorialStep step)
    {
        StartCoroutine(ServeTacosCoroutine(step));
    }

    IEnumerator ServeTacosCoroutine(TutorialStep step)
    {
        GameManager.Instance.SidebarManager.DeactivateAllSidebarButtons();
        if (step.messagekey != null && step.messagekey != "")
        {
            tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
        }
        var initialPosition = GameManager.Instance.CheckoutManager.GetFirstTacosTransform().position;
        var finalPos = GameManager.Instance.OrdersManager.GetFirstOrderTransform().position;
        tutorialPanelVisual.HidePanel();
        tutorialCursorDisplayer.MoveCursor(initialPosition, finalPos, 1);

        yield return new WaitUntil(() => GameManager.Instance.OrdersManager.GetCurrentOrdersCount() == 0);
        tutorialCursorDisplayer.StopCursorMovement();
        GameManager.Instance.SidebarManager.ActivateAllSidebarButtons();
        PlayNextStep();
    }

    IEnumerator RemoveGrilledTacosActionCoroutine(TutorialStep step)
    {
        GameManager.Instance.SidebarManager.DeactivateAllSidebarButtons();
        if (step.messagekey != null && step.messagekey != "")
        {
            tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
        }

        var grillPosition = GameManager.Instance.GrillManager.GetGrillPosition().position;
        var initialPosition = grillPosition + new Vector3(0, -200, 0);
        var finalPos = grillPosition + new Vector3(0, 200, 0);
        tutorialPanelVisual.HidePanel();
        tutorialCursorDisplayer.MoveCursor(initialPosition, finalPos, 1);
        yield return new WaitUntil(() => GameManager.Instance.GrillManager.isGrillOpened);
        yield return new WaitForSeconds(0.25f);
        tutorialCursorDisplayer.StopCursorMovement();
        GameManager.Instance.GrillManager.PreventUserFromOpeningOrClosingGrill();
        var grilledTacosTransform = GameManager.Instance.GrillManager.GetFirstGrillingTacosTransform();
        tutorialPanelVisual.FocusOn(grilledTacosTransform, 0.12f);
        yield return new WaitUntil(() => !GameManager.Instance.GrillManager.ContainsAtLeastOneTacos());
        GameManager.Instance.GrillManager.AllowUserToOpenOrCloseGrill();
        GameManager.Instance.SidebarManager.ActivateAllSidebarButtons();
        PlayNextStep();
    }

    IEnumerator PutTacosToGrillCoroutine(TutorialStep step)
    {
        GameManager.Instance.SidebarManager.DeactivateAllSidebarButtons();
        GameManager.Instance.GrillManager.DisableUserToRemoveTacos();
        GameManager.Instance.GrillManager.PreventUserFromOpeningOrClosingGrill();


        if (step.messagekey != null && step.messagekey != "")
        {
            tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
        }

        var initialPosition = GameManager.Instance.GrillManager.GetFirstTacosTransform().position + new Vector3(0, 200, 0);
        var finalPos = GameManager.Instance.GrillManager.GetGrillPosition().position;
        tutorialPanelVisual.HidePanel();
        tutorialCursorDisplayer.MoveCursor(initialPosition, finalPos, 1);

        yield return new WaitUntil(() => GameManager.Instance.GrillManager.ContainsAtLeastOneTacos());

        tutorialCursorDisplayer.StopCursorMovement();
        GameManager.Instance.GrillManager.AllowUserToOpenOrCloseGrill();

        GameManager.Instance.SidebarManager.ActivateAllSidebarButtons();
        PlayNextStep();
    }

    IEnumerator GrillTacosCoroutine(TutorialStep step)
    {
        GameManager.Instance.SidebarManager.DeactivateAllSidebarButtons();
        GameManager.Instance.GrillManager.DisableUserToRemoveTacos();

        if (step.messagekey != null && step.messagekey != "")
        {
            tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
        }

        var grillPosition = GameManager.Instance.GrillManager.GetGrillPosition().position;
        var initialPosition = grillPosition + new Vector3(0, 200, 0);
        var finalPos = grillPosition + new Vector3(0, -200, 0);
        tutorialPanelVisual.HidePanel();
        tutorialCursorDisplayer.MoveCursor(initialPosition, finalPos, 1);

        yield return new WaitUntil(() => !GameManager.Instance.GrillManager.isGrillOpened);
        tutorialCursorDisplayer.StopCursorMovement();
        GameManager.Instance.GrillManager.PreventUserFromOpeningOrClosingGrill();
        yield return new WaitUntil(() => GameManager.Instance.GrillManager.IsFirstTacosGrilled());

        GameManager.Instance.GrillManager.AllowUserToOpenOrCloseGrill();
        GameManager.Instance.GrillManager.EnableUserToRemoveTacos();
        GameManager.Instance.SidebarManager.ActivateAllSidebarButtons();
        PlayNextStep();
    }

    IEnumerator PrepareTacosActionCoroutine(TutorialStep step)
    {
        var tacosIngredients = GameManager.Instance.OrdersManager.GetFirstOrderExpectedIngredients();
        if (step.messagekey != null && step.messagekey != "")
        {
            tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
        }
        foreach (var ingredient in tacosIngredients)
        {
            var ingredientButton = GameManager.Instance.TacosMakerManager.GetIngredientButtonTransform(ingredient);
            tutorialPanelVisual.FocusOn(ingredientButton.GetComponent<RectTransform>(), 0.035f);
            yield return new WaitUntil(() => GameManager.Instance.TacosMakerManager.IsIngredientInTacos(ingredient));
        }
        var onCreationTacosTransform = GameManager.Instance.TacosMakerManager.GetOnCreationTacosTransform();
        tutorialPanelVisual.FocusOn(onCreationTacosTransform, 0.18f);
        yield return new WaitUntil(() => GameManager.Instance.TacosMakerManager.IsOnCreationTacosEmpty());
        tutorialMessageDisplayer.HideMessage();
        PlayNextStep();


    }

    IEnumerator RemoveIngredientsFromHotplateCoroutine(TutorialStep step)
    {
        for (int i = 0; i < GlobalConstant.MAX_COOKING_INGREDIENTS; i++)
        {
            var cookingIngredientTransform = GameManager.Instance.HotplateManager.GetCookingIngredientTransform(i);
            tutorialPanelVisual.FocusOn(cookingIngredientTransform, 0.05f);
            if (step.messagekey != null && step.messagekey != "")
            {
                tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
            }
            yield return new WaitUntil(() => !GameManager.Instance.HotplateManager.IsIngredientCookingOnPosition(i));
        }
        tutorialMessageDisplayer.HideMessage();
        PlayNextStep();
    }

    IEnumerator AddIngredientToHotplateCoroutine(TutorialStep step)
    {
        var ingredientButton = GameManager.Instance.HotplateManager.GetIngredientButtonTransform(step.ingredient);
        tutorialPanelVisual.FocusOn(ingredientButton.GetComponent<RectTransform>(), 0.05f);
        if (step.messagekey != null && step.messagekey != "")
        {
            tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
        }
        yield return new WaitUntil(() => !GameManager.Instance.HotplateManager.CanAddIngredientToCook());
        tutorialMessageDisplayer.HideMessage();
        PlayNextStep();
    }
    IEnumerator AddIngredientToFryerCoroutine(TutorialStep step)
    {
        var ingredientButton = GameManager.Instance.FryerManager.GetFirstIngredientButtonTransform(step.ingredient);
        tutorialPanelVisual.FocusOn(ingredientButton.GetComponent<RectTransform>(), 0.05f);
        if (step.messagekey != null && step.messagekey != "")
        {
            tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
        }
        yield return new WaitUntil(() => GameManager.Instance.FryerManager.GetFirstBasketQuantity() == 3);
        var basketTransform = GameManager.Instance.FryerManager.GetFirstBasketTransform();
        tutorialPanelVisual.FocusOn(basketTransform.GetComponent<RectTransform>(), 0.15f);
        yield return new WaitUntil(() => GameManager.Instance.FryerManager.IsFirstBasketFrying());
        tutorialMessageDisplayer.HideMessage();
        PlayNextStep();
    }

    IEnumerator RemoveIngredientToFryerCoroutine(TutorialStep step)
    {
        if (step.messagekey != null && step.messagekey != "")
        {
            tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
        }
        var basketTransform = GameManager.Instance.FryerManager.GetFirstBasketTransform();
        tutorialPanelVisual.FocusOn(basketTransform.GetComponent<RectTransform>(), 0.15f);
        yield return new WaitUntil(() => !GameManager.Instance.FryerManager.IsFirstBasketFrying());
        tutorialMessageDisplayer.HideMessage();
        PlayNextStep();
    }

    IEnumerator AddIngredientToSauceGruyereCoroutine(TutorialStep step)
    {
        if (step.messagekey != null && step.messagekey != "")
        {
            tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
        }
        var ingredientButton = GameManager.Instance.SauceGruyereManager.GetIngredientButtonRectTransform(step.order[0]);
        tutorialPanelVisual.FocusOn(ingredientButton.GetComponent<RectTransform>(), 0.05f);
        yield return new WaitUntil(() => GameManager.Instance.SauceGruyereManager.IsIngredientInPot(step.order[0]));
        ingredientButton = GameManager.Instance.SauceGruyereManager.GetIngredientButtonRectTransform(step.order[1]);
        tutorialPanelVisual.FocusOn(ingredientButton.GetComponent<RectTransform>(), 0.05f);
        yield return new WaitUntil(() => GameManager.Instance.SauceGruyereManager.IsIngredientInPot(step.order[1]));
        tutorialMessageDisplayer.HideMessage();
        PlayNextStep();
    }

    IEnumerator RemoveIngredientToSauceGruyereCoroutine(TutorialStep step)
    {
        if (step.messagekey != null && step.messagekey != "")
        {
            tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
        }
        var potTransform = GameManager.Instance.SauceGruyereManager.GetPotRectTransform();
        tutorialPanelVisual.FocusOn(potTransform.GetComponent<RectTransform>(), 0.15f);
        yield return new WaitUntil(() => GameManager.Instance.SauceGruyereManager.IsPotEmpty());
        tutorialMessageDisplayer.HideMessage();
        PlayNextStep();
    }

    IEnumerator MoveToWindowCoroutine(TutorialStep step)
    {
        var target = GameManager.Instance.SidebarManager.GetSidebarButtonRectTransform(step.viewToShow);
        tutorialPanelVisual.FocusOn(target, 0.03f);
        if (step.messagekey != null && step.messagekey != "")
        {
            tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, true);
        }
        yield return new WaitUntil(() => PlayzoneVisual.currentView == step.viewToShow);
        tutorialMessageDisplayer.HideMessage();
        PlayNextStep();
    }

    IEnumerator ShowMessageCoroutine(TutorialStep step)
    {
        Debug.Log("Showing tutorial message: " + step.messagekey);
        tutorialMessageDisplayer.ShowMessage(step.messagekey, step.messageModalYPosition, false, step.shouldShowMessageBackground);
        isNextButtonClicked = false;
        yield return new WaitUntil(() => isNextButtonClicked);
        tutorialMessageDisplayer.HideMessage();
        PlayNextStep();
    }

    public void OnNextButtonClicked()
    {
        isNextButtonClicked = true;
    }
}