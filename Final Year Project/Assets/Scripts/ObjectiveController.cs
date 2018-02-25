using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveController : MonoBehaviour {
    //list of current objective toggles
    public Toggle[] togList;
    //list of completed objective toggles
    public Toggle[] compTogList;
    //list of all possible objectives
    private List<string> ObjectiveList;
    private int ObjectiveCount;
    public Text trophyCount;
    public GameObject player;
    public Material reward;
    void Awake () {
        //make list of obejectives
        ObjectiveList = new List<string>(new string[] {
            "Kill 25 aliens", "loot 25 weapon parts", "travel 225 million kilometres", 
            "Build a planet base", "Complete a level without getting hit",

            "kill 50 aliens","loot 50 weapon parts", "travel 1.2 billion kilometres",
            "Repair base", "Upgrade a base to level 2",

            "kill 150 aliens", "loot 150 weapon parts", "travel 4.3 billion kilometres",
            "Defeat re-inhabited planet level", "Fully upgrade base"
        });

    }
    //when the screen is enabled
    private void OnEnable()
    {
        if (!GameManagerS.AllObjCompleted)
        {
            //judges whether objectives have been completed or not
            if (GameManagerS.TotalAliensKilled > 24)
            {
                GameManagerS.CompleteObjList[0] = true;
                if (GameManagerS.TotalAliensKilled > 49)
                {
                    GameManagerS.CompleteObjList[5] = true;
                    if (GameManagerS.TotalAliensKilled > 149)
                    {
                        GameManagerS.CompleteObjList[10] = true;
                    }
                }
            }
            if (GameManagerS.TotalPartsCollected > 24)
            {
                GameManagerS.CompleteObjList[1] = true;
                if (GameManagerS.TotalPartsCollected > 49)
                {
                    GameManagerS.CompleteObjList[6] = true;
                    if (GameManagerS.TotalPartsCollected > 149)
                    {
                        GameManagerS.CompleteObjList[11] = true;
                    }
                }
            }

            if (GameManagerS.Stage == 2)
            {
                GameManagerS.CompleteObjList[2] = true;
                if (GameManagerS.Stage == 4)
                {
                    GameManagerS.CompleteObjList[7] = true;
                    if (GameManagerS.Stage == 6)
                    {
                        GameManagerS.CompleteObjList[12] = true;
                    }
                }
            }

            foreach (int i in GameManagerS.BaseLevels)
            {
                if (i > 0)
                {
                    GameManagerS.CompleteObjList[3] = true;
                    if (i > 1)
                    {
                        GameManagerS.CompleteObjList[9] = true;
                        if (i > 2)
                        {
                            GameManagerS.CompleteObjList[14] = true;
                        }
                    }
                }
            }
        }
        //reset completed objective count
        ObjectiveCount = 0;
        //set the number of complete objectives
        foreach (KeyValuePair<int, bool> pair in GameManagerS.CompleteObjList)
        {
            if (pair.Value)
            {
                ObjectiveCount++;
            }
        }
        //if completed all objectives
        if (ObjectiveCount == 15)
        {
            GameManagerS.AllObjCompleted=true;
            player.GetComponent<SpriteRenderer>().material = reward;
        }
        //set the toggle labels
        SetToggleLabels();
        //update the trophy count
        trophyCount.text = "" + ObjectiveCount / 5 + "/3";
    }

    void SetToggleLabels()
    {
        //create a temp list to hold the incomplete objectives
        string[] temp = new string[5];
        int i = 0;
        //for each index and bool pair in dict
        foreach(KeyValuePair<int, bool> pair in GameManagerS.CompleteObjList)
        {
            //if the objective has not been completed and 5 havent been chosen yet
            if (!pair.Value && i < 5)
            {
                //add it to the temp list
                temp[i] = ObjectiveList[pair.Key];
                i++;
            }
            else if (pair.Value)
            {
                compTogList[i].transform.GetChild(1).GetComponent<Text>().text = ObjectiveList[pair.Key];
            }
            
        }
        //for each toggle label
        i = 0;
        foreach (Toggle t in togList)
        {
            //set the text to the uncomplete objective text
            t.transform.GetChild(1).GetComponent<Text>().text = temp[i];
            i++;
        }
    }

}
