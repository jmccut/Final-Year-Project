using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InsideController : MonoBehaviour {
    public GameObject enemy;
    public GameObject boss;
    public static int KilledCount { get; set; } //counts number of enemies killed
    public int numberOfEnemies; //number of normal enemies in level
    public GameObject ladder;
    public ChangeScene change;
    public Button restart;
    public GameObject healthPack;

	void Start () {
        numberOfEnemies = 5;
        KilledCount = 0;
        //instantiates enemies
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(enemy);
        }
        Instantiate(healthPack);
        //make boss if on last scene of the ship
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            Instantiate(boss);
        }
        restart.gameObject.SetActive(false);
	}

    private void Update()
    {
        //if this is not the boss level
        if (SceneManager.GetActiveScene().buildIndex != 4)
        {
            //if the player kills all enemies, show ladder
            if (KilledCount < numberOfEnemies)
            {
                ladder.gameObject.SetActive(false);
            }
            else
            {
                ladder.gameObject.SetActive(true);
            }
        }
        else
        {
            //if the player has killed all of the enemies as well as the boss
            if (KilledCount == (numberOfEnemies+1))
            {
                GameManagerS.OnBossLevel = false;
                change.Change(1);
            }
        }
        //if the player dies, show the restart button
        if (CharController.Dead)
        {
            restart.gameObject.SetActive(true);
        }
    }

    public void Restart()
    {
        //loads the scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        //stops game time
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
