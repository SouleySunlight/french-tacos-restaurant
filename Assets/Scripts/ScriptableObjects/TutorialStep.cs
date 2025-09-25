using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Step")]
public class TutorialStep : ScriptableObject
{
    public TutorialStepType tutorialStepType;
    public ViewToShowEnum viewToShow = ViewToShowEnum.TACOS_MAKER;
    public Ingredient ingredient = null;
    public List<Ingredient> order = new();
    public string messagekey = null;
    public int messageModalYPosition = 0;
    public bool shouldShowMessageBackground = false;

}

public enum TutorialStepType
{
    ADD_ORDER,
    MOVE_TO_WINDOW,
    ADD_INGREDIENT_TO_HOTPLATE,
    REMOVE_INGREDIENT_FROM_HOTPLATE,
    PREPARE_TACOS,
    SHOW_MESSAGE,
    PUT_TACOS_TO_GRILL,
    GRILL_TACOS,
    REMOVE_GRILLED_TACOS,
    SERVE_TACOS,
    ADD_INGREDIENT_TO_FRYER,
    REMOVE_INGREDIENT_FROM_FRYER,
    ADD_INGREDIENT_TO_GRUYERE_POT,
    REMOVE_INGREDIENT_FROM_GRUYERE_POT,
    FINISH_DAY,
    PRESS_UPGRADE_BUTTON
}

public enum TutorialType
{
    DAY_ZERO,
    UPGRADE,
}