using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SettingSystem 
{
    static string path = Application.persistentDataPath + "/setting.save";
   public static void SaveGame(VolumeSetting setting) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path,FileMode.Create);

        SettingModel saveData = new SettingModel(setting);

        formatter.Serialize(stream, saveData);
        stream.Close();
   }

   public static SettingModel LoadGame() {
    if(File.Exists(path)) 
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path,FileMode.Open);
        SettingModel saveData = formatter.Deserialize(stream) as SettingModel;
        stream.Close();

        return saveData;
    } else {
        Debug.Log("No saved setting");
        return null;
    }
   }
}