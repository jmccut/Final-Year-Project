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
    public bool OnBossLevel;
    public float Health;
    public int Money;
    public int ShipWepLevel;
    public int BossWepLevel;
    public bool[] PowerUps;
}
public class GameManagerS : MonoBehaviour
{
    public static int Level { get; set; } //holds current level
    public static int Stage { get; set; }
    public static float Health { get; set; }
    public static bool OnBossLevel { get; set; }
    public static int Money { get; set; }
    public static int ShipWepLevel { get; set; }
    public static int BossWepLevel { get; set; }
    public static bool[] PowerUps { get; set; }


    private void Awake()
    {
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
        pd.Health = Health;
        pd.OnBossLevel = OnBossLevel;
        pd.Money = Money;
        pd.ShipWepLevel = ShipWepLevel;
        pd.BossWepLevel = BossWepLevel;
        pd.PowerUps = PowerUps;
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
            Health = pd.Health;
            Money = pd.Money;
            ShipWepLevel = pd.ShipWepLevel;
            BossWepLevel = pd.BossWepLevel;
            PowerUps = pd.PowerUps;
            OnBossLevel = pd.OnBossLevel;
            file.Close();
        }
    }

    public void Reset()
    {
        //resets all player data ready for new game
        Stage = 0;
        Level = 0;
        OnBossLevel = false;
        Health = 100f;

        Money = 0;
        ShipWepLevel = 0;
        BossWepLevel = 0;
        PowerUps = new bool[3];
    }
}
