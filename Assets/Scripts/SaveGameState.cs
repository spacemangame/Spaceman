using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public static class SaveGameState
{
    public static GameState savedGameState = new GameState(0,0);

    public static void Save(GameState currState)
    {
        savedGameState = currState;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, SaveGameState.savedGameState);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveGameState.savedGameState = (GameState)bf.Deserialize(file);
            file.Close();
        }
    }
}
