using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TacosMakerVisual : MonoBehaviour
{
    [SerializeField] private GameObject tacosMakerWindow;
    [SerializeField] private GameObject tortillaPrefab;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private RectTransform onCreationTacosTransform;
    [SerializeField] private RectTransform ingredientButtonFirstTransform;

    private GameObject onCreationTacos;
    private TacosMakerManager tacosMakerManager;
    private readonly int INGREDIENT_BUTTON_HORIZONTAL_GAP = 345;
    private readonly int INGREDIENT_BUTTON_VERTICAL_GAP = -200;
    private readonly int NUMBER_OF_BUTTON_PER_ROW = 3;


    void Awake()
    {
        tacosMakerManager = FindFirstObjectByType<TacosMakerManager>();
    }

    public void OpenTacosMakerWindow()
    {
        tacosMakerWindow.SetActive(true);
    }

    public void CloseTacosMakerWindow()
    {
        tacosMakerWindow.SetActive(false);
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
                ingredientButtonFirstTransform.position.x + INGREDIENT_BUTTON_HORIZONTAL_GAP * (index % NUMBER_OF_BUTTON_PER_ROW),
                ingredientButtonFirstTransform.position.y + INGREDIENT_BUTTON_VERTICAL_GAP * (index / NUMBER_OF_BUTTON_PER_ROW),
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
        var ingredientVisual = Instantiate(ingredient.sprite, onCreationTacos.GetComponent<RectTransform>().position, Quaternion.identity, onCreationTacos.GetComponent<RectTransform>());
        tacosMakerManager.AddIngredients(ingredient);
    }
}
