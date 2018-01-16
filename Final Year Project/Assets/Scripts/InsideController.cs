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

	void Start () {

        numberOfEnemies = 5;
        KilledCount = 0;
        //instantiates enemies
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(enemy);
        }
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
            //if the player has not killed all enemies yet, the ladder 
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
                change.Change(1);
            }
        }
        if (CharController.Dead)
        {
            restart.gameObject.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
