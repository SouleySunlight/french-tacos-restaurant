using System;
using System.Collections.Generic;

public class Order
{
    public Guid guid;
    public List<List<Ingredient>> expectedOrder { get; private set; } = new();

    public Order(List<List<Ingredient>> ingredients)
    {
        guid = Guid.NewGuid();
        expectedOrder = ingredients;
    }
}