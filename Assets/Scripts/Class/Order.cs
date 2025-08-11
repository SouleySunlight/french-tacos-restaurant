using System;
using System.Collections.Generic;

public class Order
{
    public Guid guid;
    public List<OrderItem> expectedOrder { get; private set; } = new();
    public float price;

    public Order(List<List<Ingredient>> ingredients)
    {
        guid = Guid.NewGuid();

        List<OrderItem> orderItems = new();

        foreach (var tacosComposition in ingredients)
        {
            orderItems.Add(new OrderItem(tacosComposition));
        }
        expectedOrder = orderItems;

        price = ingredients.Count * GlobalConstant.TACOS_PRICE;
    }
}