using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

    public float wallHeightRange; //max height the wall can be
    public GameObject wall; //reference to the bottom wall prefab
    public GameObject TopWall; //reference to the top wall prefab
    public GameObject alien; //reference to the alien to spawn
    public GameObject alien2;
    private bool isRunning; //flag to indicate if the game is running
    public GUIController GUI; //used to enable the GUI again after the level has ended
    public Text levelText; //used to display the current level
    public static int numAliensToKill;//holds the number of aliens to kill per level
    public int numAliensMultiplier; //how much alien numbers increase over levels
    public static bool stageCleared; //flag for when a number of stages are cleared
    private int stage; //holds which stage the game is on
    public GameObject player; //holds reference to the player so they can be revived
    private float previousHeight; //holds the previous height of the walls
    private float wallSpawnSpeed; //holds the speed to spawn the walls
    private float alienSpawnSpeed;
    private float middleWallSpawnSpeed;
    private GameObject wallCopy;
    private GameObject TopWallCopy;
    public ChangeScene change;
    public Text moneyT;

    //used to safely access running flag
    public static bool IsRunning { get; set; }

    private void Awake()
    {
        //initialises game manager state
        if (GameManagerS.Level == 0 && GameManagerS.Stage == 0)
        {
            GameManagerS.Level = 1;
            GameManagerS.Stage = 1;
            GameManagerS.OnBossLevel = false;
            GameManagerS.ShipWepLevel = 1;
            GameManagerS.BossWepLevel = 1;
            GameManagerS.Money = 0;
            GameManagerS.PowerUps = new bool[3];
        }
        //changes to boss level if the save file was on the boss
        else if (GameManagerS.OnBossLevel)
        {
            change.Change(2);
        }
    }

    void Start () {
        //sets the game to: not running, level 1, stage 1 and number of aliens to kill as 5
        IsRunning = false;
        ResetAliensToKill();
         //sets the number of aliens to kill to level up
        previousHeight = -100; //arbitrary number so that the function knows it has not been initialised
        //WallController.speed = 17; //sets starting speed for walls
        wallSpawnSpeed = 0.4f;
        alienSpawnSpeed = 6f;
        middleWallSpawnSpeed = 5.5f;
    }

    private void Update()
    {
        // if the game is running and the player has killed all aliens, level up
        if (IsRunning && numAliensToKill == 0)
        {
            LevelUp();
        }

        //sets the text for the health HUD
        levelText.text = "Level: " + GameManagerS.Level + "/5";
        moneyT.text = "£" + GameManagerS.Money;
    }

    void LevelUp()
    { //handles everything needed to level up

        EndGame();

        //if this is also the last level (the 5th)
        if (GameManagerS.Level % 5 == 0)
        {
            //if that was the last stage go to end scene
            if (GameManagerS.Stage == 9)
            {
                change.Change(5);
            }
            //set flag, increment stage count and reset level count
            stageCleared = true;
            GameManagerS.Stage++;
            GameManagerS.Level = 0;
        }
        //increment the level count
        GameManagerS.Level++;
        //resets alien numbers
        ResetAliensToKill();
        //resets random wall generator
        previousHeight = -100;
    }

    void makeEnemy()
    { 
        if (GameManagerS.Stage < 3)
        {
            //make standard enemy
            Instantiate(alien, new Vector3(-125f, Random.Range(-30, 30), 0), Quaternion.identity);
        }
        else
        {
            //spawn enemy at the beginning of the map in a random range along the y
            int n = 30;
            for (int i = 0; i < 5; i++)
            {
                Instantiate(alien2, new Vector3(-125f, n, 0), Quaternion.identity);
                n -= 15;
            } 
        }
    }

    void makeMiddleWall()
    { //spawn a middle wall with a random y value and a random scale if the other walls aren't too high
        Vector3 randVect = new Vector3(0f, Random.Range(0f, wallHeightRange), 0f);
        if (wallCopy.transform.localScale.y < 85f)
        {
            GameObject middleWall = Instantiate(wall, new Vector3(-125f, Random.Range(-26f, 26), wall.transform.position.z), Quaternion.identity);
            middleWall.transform.localScale += randVect;
        }
        
    }

    void MakeWall()
    { //make two copies of walls, 1 top & 1 bottom and instantiate them with the same with random heights
      //based on height before it

        Vector3 heightVect; //holds the height vector to be applied
        float randDiff; //random difference to be applied to next wall

        //if the previous height has not been stored yet, make the previous height a random size
        if (previousHeight == -100)
        {
            previousHeight = Random.Range(13f, wallHeightRange);
            heightVect = new Vector3(0f, previousHeight, 0f);
            randDiff = 0;
        }
        //otherwise, make a random height from the previous one differing by 10 at most
        else
        {
            float test = previousHeight - 10;
            if(test < 13)
            {
                test = 13;
            }
            randDiff = Random.Range(test, previousHeight + 10);
            heightVect = new Vector3(0f, randDiff, 0f);
            previousHeight = randDiff;
        }

        //instantiate wall objects at location off the screen
        wallCopy = Instantiate(wall, new Vector3(-125, -46.3f, wall.transform.position.z), Quaternion.identity);
        TopWallCopy = Instantiate(TopWall, new Vector3(-125, 46.3f, wall.transform.position.z), Quaternion.identity);

        //if applying the scaling would make the wall too tall, give it a new height in the range 60-90
        if ((wallCopy.transform.localScale + heightVect).y > 120)
        {
            heightVect = new Vector3(0f, Random.Range(60f, 90f), 0f);
        }

        //apply the scaling to the walls and update their position so they are not off the screen after scaling
        wallCopy.transform.localScale += heightVect;
        wallCopy.transform.position += new Vector3(0f, heightVect.y * 0.15f, 0f);

        TopWallCopy.transform.localScale += heightVect;
        TopWallCopy.transform.position += new Vector3(0f, heightVect.y * -0.15f, 0f);
    }

    public void StartGame()
    { //set the game to running, instantiate walls and disable the GUI
        IsRunning = true;
        InvokeRepeating("MakeWall", 0f, wallSpawnSpeed - (0.05f *GameManagerS.Level));
        InvokeRepeating("makeMiddleWall", 0f, middleWallSpawnSpeed - (0.5f * GameManagerS.Level));
        if (GameManagerS.Stage < 3)
        {
            InvokeRepeating("makeEnemy", 0f, alienSpawnSpeed - (1 * GameManagerS.Level));
        }
        else
        {
            makeEnemy();
        }
        BulletController.Speed *= GameManagerS.Level;   
        GUI.DisableCanvas();
    }

    public void EndGame()
    { //set the game to ended, stop making walls, kill the aliens and bullets and enable the GUI again
        IsRunning = false;
        CancelInvoke();
        if (GameManagerS.Stage < 3)
        {
            GameObject[] aliens = GameObject.FindGameObjectsWithTag("Alien1");
            foreach (GameObject g in aliens)
            {
                Destroy(g.gameObject);
            }
        }
        else
        {
            GameObject[] aliens = GameObject.FindGameObjectsWithTag("Alien2");
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Alien Bullet");
            foreach (GameObject g in aliens)
            {
                Destroy(g.gameObject);
            }
            foreach (GameObject g in bullets)
            {
                Destroy(g.gameObject);
            }
        }

        StartCoroutine(GUI.EnableCanvas());

    }

    public void RestartGame()
    { //end the game, kill all the walls and render the player again
        EndGame();
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject w in walls)
        {
            Destroy(w.gameObject);
        }

        ResetAliensToKill();
        previousHeight = -100;
        player.GetComponent<Renderer>().enabled = true;
    }

    public void MuteGame()
    {
        //if the game was unmuted
        if (PlayerPrefs.GetInt("IsMuted") == 0)
        {
            //mute the game
            PlayerPrefs.SetInt("IsMuted", 1);
            GetComponent<AudioSource>().mute = true;
        }
        //if the game was muted
        else if (PlayerPrefs.GetInt("IsMuted") == 1)
        {
            //unmute the game
            PlayerPrefs.SetInt("IsMuted", 0);
            GetComponent<AudioSource>().mute = false;
        }
    }

    private void ResetAliensToKill()
    {
        numAliensToKill = GameManagerS.Level * numAliensMultiplier;
    }
}
