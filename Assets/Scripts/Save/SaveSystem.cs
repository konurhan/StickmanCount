using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveData<T>(string relativePath, T data)
    {
        string path = Application.persistentDataPath + relativePath;

        if (File.Exists(path))
        {
            Debug.Log("Data exists. Deleting old file and writing a new one!");
            File.Delete(path);
        }
        else
        {
            Debug.Log("Writing file for the first time!");
        }
        using FileStream stream = File.Create(path);
        stream.Close();
        File.WriteAllText(path, JsonConvert.SerializeObject(data));
    }

    public static T LoadData<T>(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path))
        {
            Debug.Log($"Cannot load file at {path}. File does not exist!");
            return default;
        }

        T data;
        data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));

        return data;
    }
}

public class GameProgress
{
    public int lastUnlockedLevel;
    public int coinCount;
    public int startingUnitsCount;
    public GameProgress()
    {
        lastUnlockedLevel = 0; coinCount = 0; startingUnitsCount = 1;
    }
}