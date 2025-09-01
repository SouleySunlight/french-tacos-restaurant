using System.Collections.Generic;

public class SettingsSaveData
{
    public bool isSoundOn = true;
    public bool isMusicOn = true;
    public string language;
    public TutosViewingSaveData tutosViewing = null;

}

[System.Serializable]
public class TutosViewingSlotSaveData
{
    public string viewId;
    public bool hasBeenViewed;
}

[System.Serializable]
public class TutosViewingSaveData
{
    public List<TutosViewingSlotSaveData> slots = new();
}