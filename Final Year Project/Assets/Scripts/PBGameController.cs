using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PBGameController : MonoBehaviour
{
    //stats
    public static int numAliensToKill;
    private int numOfAlien1;
    private int numOfAlien2;
    private int numOfAlien3;
    private int wave;
    private int spawnRate;
    //list of alive aliens
    public static List<GameObject> AliveAliens { get; set; }
    //references
    public ChangeScene change;
    public GameObject alien1;
    public GameObject alien2;
    public GameObject alien3;
    //flags
    public static bool Paused {set;get;}
 
    private void Awake()
    {
        numAliensToKill = 25;
        AliveAliens = new List<GameObject>();
    }

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {   
        //spawns appropriate aliens given wave
        if (wave == 0)
        {
            spawnRate = 1;
            InvokeRepeating("MakeAlien2", 0, spawnRate);
        }
        else if (wave == 1)
        {
            spawnRate = 5;
            CancelInvoke();
            InvokeRepeating("MakeAlien1", 0, spawnRate);
        }
        else if (wave == 2)
        {
            spawnRate = 15;
            CancelInvoke();
            InvokeRepeating("MakeAlien3", 0, spawnRate);
        }
    }

    private void Update()
    {
        //set the different wave numbers when certain number of aliens killed for each type
        if(numAliensToKill == 11 && wave == 0)
        {
            wave = 1;
            SpawnEnemies();
        }
        else if (numAliensToKill == 6 && wave == 1)
        {
            wave = 2;
            SpawnEnemies();
        }
        else if(numAliensToKill == 0)
        {
            CancelInvoke();
            EndGame();
        }
    }

    public void Restart()
    {
        //loads the scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        //reset the damage and levels of the planet which this level referred to
        GameManagerS.BaseDamage[GameManagerS.CurrentPlanet] = 0;
        GameManagerS.BaseLevels[GameManagerS.CurrentPlanet] = 0;
        //set objective to complete
        GameManagerS.CompleteObjList[13] = true;
        //turns off upgrades if active
        if (GameManagerS.PowerUps[0])
        {
            GameManagerS.PowerUps[0] = false;
        }
        if (GameManagerS.PowerUps[1])
        {
            GameManagerS.PowerUps[1] = false;
        }
        //change back to main
        change.Change(1);
        GameManagerS.OnBaseBossLevel = false;
    }

    public void Pause()
    {
        //stops game time
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Paused = true;
        }
        else
        {
            Paused = false;
            Time.timeScale = 1;
        }
    }

    //methods to make relevant aliens

    public void MakeAlien1()
    {
        Instantiate(alien1, new Vector3(65f, Random.Range(-40, 40f), 0f), alien1.transform.rotation);
    }
    public void MakeAlien2()
    {
        Instantiate(alien2, new Vector3(65f, Random.Range(-40, 40f), 0f), alien2.transform.rotation);
    }
    public void MakeAlien3()
    {
        Instantiate(alien3, new Vector3(65f, 20f, 0f), alien3.transform.rotation);
        Instantiate(alien3, new Vector3(65f, -20f, 0f), alien3.transform.rotation);
    }
}
