using System;
using System.Collections.Generic;
using UnityEngine;

public class Tacos
{

    public Guid guid;
    public List<Ingredient> ingredients;

    public bool isGrilled;
    public bool isBurnt;

    public Tacos()
    {
        guid = Guid.NewGuid();
        ingredients = new();
        isGrilled = false;
        isBurnt = false;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ingredients.Add(ingredient);
    }

    public void GrillTacos()
    {
        isGrilled = true;
    }

    public bool IsGrilled()
    {
        return isGrilled;
    }

    public void BurnTacos()
    {
        isBurnt = true;
    }

    public bool IsBurnt()
    {
        return isBurnt;
    }

}
