using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TacosMakerVisual : MonoBehaviour
{
    [SerializeField] private GameObject tortillaPrefab;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private RectTransform onCreationTacosTransform;
    [SerializeField] private RectTransform ingredientButtonFirstTransform;
    [SerializeField] private RectTransform doneTacosFirstPosition;

    private GameObject onCreationTacos;
    private TacosMakerManager tacosMakerManager;
    private readonly int NUMBER_OF_BUTTON_PER_ROW = 3;


    void Awake()
    {
        tacosMakerManager = FindFirstObjectByType<TacosMakerManager>();
    }

    public void CreateTacos()
    {
        onCreationTacos = Instantiate(tortillaPrefab, onCreationTacosTransform.position, Quaternion.identity, onCreationTacosTransform);
    }

    public void SetupIngredients(List<Ingredient> ingredients)
    {
        var index = 0;
        foreach (Ingredient ingredient in ingredients)
        {
            var buttonPosition = new Vector3(
                ingredientButtonFirstTransform.position.x + GlobalConstant.INGREDIENT_BUTTON_HORIZONTAL_GAP * (index % NUMBER_OF_BUTTON_PER_ROW),
                ingredientButtonFirstTransform.position.y + GlobalConstant.INGREDIENT_BUTTON_VERTICAL_GAP * (index / NUMBER_OF_BUTTON_PER_ROW),
                ingredientButtonFirstTransform.position.z
            );

            var buttonPrefab = Instantiate(ingredientButtonPrefab, buttonPosition, Quaternion.identity, ingredientButtonFirstTransform);
            buttonPrefab.GetComponent<Button>().onClick.AddListener(() => AddIngredient(ingredient));
            buttonPrefab.GetComponentInChildren<TMP_Text>().text = ingredient.name;
            index++;
        }
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (onCreationTacos == null) { return; }
        var createdIngredient = Instantiate(ingredientPrefab, onCreationTacos.GetComponent<RectTransform>().position, Quaternion.identity, onCreationTacos.GetComponent<RectTransform>());
        createdIngredient.GetComponent<IngredientDisplayer>().ingredientData = ingredient;

        tacosMakerManager.AddIngredients(ingredient);
    }

    public void WrapTacos(Tacos createdTacos)
    {
        Destroy(onCreationTacos);
        onCreationTacos = null;
    }
}
