using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public static class SaveSystem
{
    public static void SaveGame(string file)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + file;
        FileStream stream = new FileStream(path, FileMode.Create);
        GameData sharedData = new GameData(Object.FindObjectOfType<PersistentManager>(), Resources.FindObjectsOfTypeAll<PlayerController>().OrderBy(a => a.name).ToArray());
        formatter.Serialize(stream, sharedData);
        stream.Close();
    }

    public static GameData LoadGame(string file)
    {
        string path = Application.persistentDataPath + file;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save file not found at " + path);
            return null;
        }
    }
}