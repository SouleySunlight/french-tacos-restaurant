using System.Collections.Generic;

public class SettingsSaveData
{
    public bool isSoundOn = true;
    public bool isMusicOn = true;
    public string language;
    public TutosViewingSaveData tutosViewing = null;
    public RatingModalSaveData ratingModalSaveData = null;

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

[System.Serializable]
public class RatingModalSaveData
{
    public bool hasRateTheGame = false;
    public bool refuseRatingTheGame = false;
    public int ratingNumberOfTimeAsked = 0;
}