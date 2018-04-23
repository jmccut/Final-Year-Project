using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAI : MonoBehaviour {
    private int rand;
    public int damage;
    public System.TimeSpan timeDiff; 
    private static BackgroundAI instance = null;
    float mins;

    private void Awake()
    {
        damage = 25; //damage ai does
        //used to execute awake method only once
        //this is so it does not call the damage method each time the main scene is navigated to
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        //get the difference between the time now and the time the player last saved
        timeDiff = System.DateTime.Now - GameManagerS.LastTimeSaved;
        //call planet base damage script
        DoDamage(timeDiff);
    }

    public void DoDamage(System.TimeSpan diff)
    {
        //gets total number of minutes since player last played
        mins = diff.Minutes + (diff.Hours*60);
        //the longer the player has been away the more damage is done
        for (int y = 0; y < mins/4; y++)
        {
            //for each base
            for (int i = 0; i < GameManagerS.BaseLevels.Length; i++)
            {
                //if they have unlocked the planet
                if (GameManagerS.Stage > (i + 1))
                {
                    //if there is no base on it yet
                    if (GameManagerS.BaseLevels[i] == 0)
                    {
                        //1 in 15 chance of taking damage
                        rand = Random.Range(0, 15);
                        if (rand == 0)
                        {
                            //planet is dead as there is no base on it
                            GameManagerS.BaseDamage[i] = 100;
                            GameManagerS.BaseLevels[i] = 0;
                        }
                    }
                    //if there is a base
                    else
                    {
                        //the chance of attacking decreases per level
                        rand = Random.Range(0, 15 * GameManagerS.BaseLevels[i]);
                        if (rand == 0)
                        {
                            //do damage to base
                            GameManagerS.BaseDamage[i] += damage;
                            //if dead, reset its level
                            if(GameManagerS.BaseDamage[i] == 100)
                            {
                                GameManagerS.BaseLevels[i] = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}
