using System.IO;
using UnityEngine;

public static class SaveSystem
{

    private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "save.json");
    private static readonly string SettingsSavePath = Path.Combine(Application.persistentDataPath, "settings.json");


    public static void Save(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public static GameSaveData Load()
    {
        if (!File.Exists(SavePath))
            return new GameSaveData();

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<GameSaveData>(json);
    }

    public static void SaveSettings(SettingsSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SettingsSavePath, json);
    }

    public static SettingsSaveData LoadSettings()
    {
        if (!File.Exists(SettingsSavePath))
        {
            return new SettingsSaveData();
        }
        string json = File.ReadAllText(SettingsSavePath);
        return JsonUtility.FromJson<SettingsSaveData>(json);
    }

}