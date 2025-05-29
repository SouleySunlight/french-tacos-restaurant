using System;
using System.Collections.Generic;

public class OrderItem
{
    public List<Ingredient> tacosIngredients;
    public bool isServed;

    public OrderItem(List<Ingredient> ingredients)
    {
        tacosIngredients = ingredients;
        isServed = false;
    }
}