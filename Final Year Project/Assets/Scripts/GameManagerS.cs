using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
/*
 * portions of this script have been taken from CE318 lecture notes
 */
[Serializable]
class PlayerData
{
    public int Level;
    public int Stage;
}
public class GameManagerS : MonoBehaviour
{
    public static int Level { get; set; } //holds current level
    public static int Stage { get; set; }


    private void Awake()
    {
        //Level = 1;
        //Stage = 1;
        PlayerPrefs.SetInt("IsMuted", 0);
        //keeps this object persistent through the game
        DontDestroyOnLoad(gameObject);
    }
    public void Save()
    {
        //sets file path
        string filename = Application.persistentDataPath + "/playInfo.dat";
        BinaryFormatter bf = new BinaryFormatter();
        //open file stream to overwrite or create file
        FileStream file = File.Open(filename, FileMode.OpenOrCreate);
        //sets the level of the player data to the current level
        PlayerData pd = new PlayerData();
        pd.Level = Level;
        pd.Stage = Stage;
        //serialize then close
        bf.Serialize(file, pd);
        file.Close();
    }
    public void Load()
    {
        //sets the file path
        string filename = Application.persistentDataPath + "/playInfo.dat";
        //if the file exists
        if (File.Exists(filename))
        {
            //open it and set the current level to that specified
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filename, FileMode.Open);
            PlayerData pd = (PlayerData)bf.Deserialize(file);
            Level = pd.Level;
            Stage = pd.Stage;
            file.Close();
        }
    }
}
