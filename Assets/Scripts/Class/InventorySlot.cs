public class InventorySlot
{
    public int currentAmount;
    public int maxAmount;

    public InventorySlot()
    {
        currentAmount = GlobalConstant.DEFAULT_INGREDIENT_AMOUNT;
        maxAmount = GlobalConstant.DEFAULT_INGREDIENT_MAX_AMOUNT;
    }
}