public class UpgradeSlot
{
    public BaseUpgrade upgrade;
    public int currentLevel;

    public UpgradeSlot(BaseUpgrade up, int level)
    {
        upgrade = up;
        currentLevel = level;
    }
}