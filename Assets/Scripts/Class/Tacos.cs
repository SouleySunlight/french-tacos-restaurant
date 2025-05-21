using System;
using System.Collections.Generic;
using UnityEngine;

public class Tacos
{

    public Guid guid;
    public List<Ingredient> ingredients;

    public Tacos()
    {
        guid = Guid.NewGuid();
        ingredients = new();
    }

}
