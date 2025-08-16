using System;
using System.Collections.Generic;

public class Order
{
    public Guid guid;
    public List<Ingredient> expectedOrder { get; private set; } = new();
    public float price;

    public Order(List<Ingredient> ingredients)
    {
        guid = Guid.NewGuid();

        expectedOrder = ingredients;

        price = GlobalConstant.TACOS_PRICE;
    }
}