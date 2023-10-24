using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class LeaderboardSystem 
{
    static string path = Application.persistentDataPath + "/savefile.save";
   public static void SaveGame(LeaderBoardManager leaderBoardManager) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path,FileMode.Create);

        LeaderboardModel saveData = new LeaderboardModel(leaderBoardManager);

        formatter.Serialize(stream, saveData);
        stream.Close();
   }

   public static LeaderboardModel LoadGame() {
    if(File.Exists(path)) 
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path,FileMode.Open);
        LeaderboardModel saveData = formatter.Deserialize(stream) as LeaderboardModel;
        stream.Close();

        return saveData;
    } else {
        Debug.Log("No Save File");
        return null;
    }
   }
}