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
    public bool OnBaseBossLevel;
    public float Health;
    public int Money;
    public int Parts;
    public int ShipWepLevel;
    public int BossWepLevel;
    //ORDER : Missles, shield, triple shot
    public bool[] PowerUps;
    //stats used for the base building feature
    public int[] BaseDamage;
    public int[] BaseLevels;
    public int CurrentPlanet;
    public DateTime LastTimeSaved;
    //stats used for objectives feature
    public Dictionary<int, bool> CompleteObjList;
    public int TotalAliensKilled;
    public int TotalPartsCollected;
    public bool AllObjCompleted;
}
public class GameManagerS : MonoBehaviour
{
    public static int Level { get; set; }
    public static int Stage { get; set; }
    public static float Health { get; set; }
    public static bool OnBossLevel { get; set; }
    public static int Money { get; set; }
    public static int Parts { get; set; }
    public static int ShipWepLevel { get; set; }
    public static int BossWepLevel { get; set; }
    public static bool[] PowerUps { get; set; }
    public static int[] BaseDamage { get; set; }
    public static int[] BaseLevels { set; get; }
    public static DateTime LastTimeSaved { get; set; }
    public static int CurrentPlanet { get; set; }
    public static Dictionary<int, bool> CompleteObjList{get;set;}
    public static int TotalAliensKilled { get; set; }
    public static int TotalPartsCollected { get; set; }
    public static bool AllObjCompleted { get; set; }
    public static bool OnBaseBossLevel { get; set; }
    private void Awake()
    {
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
        //set player data
        pd.Level = Level;
        pd.Stage = Stage;
        pd.Health = Health;
        pd.OnBossLevel = OnBossLevel;
        pd.Money = Money;
        pd.Parts = Parts;
        pd.ShipWepLevel = ShipWepLevel;
        pd.BossWepLevel = BossWepLevel;
        pd.OnBaseBossLevel = OnBaseBossLevel;
        pd.PowerUps = PowerUps;
        pd.BaseDamage = BaseDamage;
        pd.BaseLevels = BaseLevels;
        pd.CurrentPlanet = CurrentPlanet;
        pd.CompleteObjList = CompleteObjList;
        pd.TotalAliensKilled = TotalAliensKilled;
        pd.TotalPartsCollected = TotalPartsCollected;
        pd.AllObjCompleted = AllObjCompleted;
        //sets the last time saved to now
        LastTimeSaved = DateTime.Now;
        pd.LastTimeSaved = LastTimeSaved;
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
            //set data from player data
            Level = pd.Level;
            Stage = pd.Stage;
            Health = pd.Health;
            Money = pd.Money;
            Parts = pd.Parts;
            CurrentPlanet = pd.CurrentPlanet;
            ShipWepLevel = pd.ShipWepLevel;
            BossWepLevel = pd.BossWepLevel;
            PowerUps = pd.PowerUps;
            OnBossLevel = pd.OnBossLevel;
            OnBaseBossLevel = pd.OnBaseBossLevel;
            BaseDamage = pd.BaseDamage;
            BaseLevels = pd.BaseLevels;
            CompleteObjList = pd.CompleteObjList;
            LastTimeSaved = pd.LastTimeSaved;
            TotalPartsCollected = pd.TotalPartsCollected;
            TotalAliensKilled = pd.TotalAliensKilled;
            AllObjCompleted = pd.AllObjCompleted;
            file.Close();
        }
    }

    public void Reset()
    {
        //resets all player data ready for new game
        Stage = 0;
        Level = 0;
        OnBossLevel = false;
        OnBaseBossLevel = false;
        Health = 100f;
        Parts = 0;
        Money = 0;
        ShipWepLevel = 0;
        BossWepLevel = 0;
        CurrentPlanet = 0;
        PowerUps = new bool[3];
        BaseDamage = new int[9];
        BaseLevels = new int[9];
        CompleteObjList = new Dictionary<int, bool>(15);
        LastTimeSaved = new DateTime();
        TotalPartsCollected = 0;
        TotalAliensKilled = 0;
        AllObjCompleted = false;
    }
}
