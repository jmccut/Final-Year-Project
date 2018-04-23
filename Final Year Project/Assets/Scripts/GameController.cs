using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {
    //statistics
    public float maxWallHeight;
    private float previousHeight;
    private float wallSpawnSpeed; 
    private float alienSpawnSpeed;
    private float middleWallSpawnSpeed;
    private int stage; 
    public static int numAliensToKill;
    public int numAliensMultiplier;
    //object references
    public GameObject wall; 
    public GameObject TopWall; 
    public GameObject alien;
    public GameObject alien2;
    public GameObject alien3;
    public GameObject alien32;
    public GameObject destination;
    public GameObject destination2;
    public GameObject tutorial;
    public GameObject tutorialArrow;
    public GameObject player;
    private GameObject wallCopy;
    private GameObject TopWallCopy;
    //references to scripts
    public GUIController GUI;
    public ChangeScene change;
    //flags
    public static bool IsRunning { get; set; }
    public static bool stageCleared;
    //GUI text to update
    public Text moneyT;
    public Text partsT;
    public Text levelText;
    //list of alive aliens
    public static List<GameObject> AliveAliens { get; set; }

    private void Awake()
    {
        //initialises game manager state for new game
        //does this on the first level and first stage (when player has started new game)
        if (GameManagerS.Level == 0 && GameManagerS.Stage == 0)
        {
            GameManagerS.LastTimeSaved = System.DateTime.Now;
            GameManagerS.Level = 1;
            GameManagerS.Stage = 1;
            GameManagerS.OnBossLevel = false;
            GameManagerS.OnBaseBossLevel = false;
            GameManagerS.ShipWepLevel = 1;
            GameManagerS.BossWepLevel = 1;
            GameManagerS.Money = 0;
            GameManagerS.Parts = 0;
            GameManagerS.PowerUps = new bool[3];
            GameManagerS.BaseDamage = new int[9];
            GameManagerS.BaseLevels = new int[9];
            GameManagerS.CompleteObjList = new Dictionary<int, bool>(15);
            GameManagerS.TotalAliensKilled = 0;
            GameManagerS.TotalPartsCollected = 0;
            for (int i = 0; i < 15; i++)
            {
                GameManagerS.CompleteObjList.Add(i, false);
            }
            //activates tutorial since it is new game
            tutorial.SetActive(true);
        }
        else
        {
            //show bouncing arrow if on levels 1 or 2 for reinforcement
            if (GameManagerS.Stage == 1 && GameManagerS.Level == 2)
            {
                tutorial.SetActive(true);
                tutorialArrow.SetActive(true);
            }
            else
            {
                tutorial.SetActive(false);
                tutorialArrow.SetActive(false);
            }
        }
        //changes to boss level if the save file was on the boss
        if (GameManagerS.OnBossLevel)
        {
            change.Change(2);
        }
        //changes to base boss level if the save file was in that scene
        if (GameManagerS.OnBaseBossLevel)
        {
            change.Change(6);
        }
        AliveAliens = new List<GameObject>();
    }

    void Start () {
        IsRunning = false;
        //sets number of aliens to kill
        ResetAliensToKill();
        previousHeight = -100; //arbitrary number so that the function knows it has not been initialised
        //sets wall and alien speeds
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
        //sets the text for the GUI
        levelText.text = "Level: " + GameManagerS.Level + "/4";
        moneyT.text = "£" + GameManagerS.Money;
        partsT.text = GameManagerS.Parts.ToString();
    }
    //handles everything needed to level up
    void LevelUp()
    { 
        EndGame();
        //turn missile mode off for the player if it was on
        if (GameManagerS.PowerUps[0])
        {
            GameManagerS.PowerUps[0] = false;
        }
        //if this is also the last level (the 4th)
        if (GameManagerS.Level % 4 == 0)
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
        //sets objective to complete if player was not hit that level
        if(player.GetComponent<PlayerController>().Health == player.GetComponent<PlayerController>().MaxHealth)
        {
            GameManagerS.CompleteObjList[4] = true;
        }
    }

    void makeEnemy()
    { 
        //spawns different enemies depending on the stage
        if (GameManagerS.Stage < 3)
        {
            //make standard enemy
            Instantiate(alien, new Vector3(-125f, Random.Range(-30, 30), 0), Quaternion.identity);
        }
        else if (GameManagerS.Stage < 6)
        {
            //spawn enemy at the beginning of the map in a random range along the y
            int n = 30;
            for (int i = 0; i < 5; i++)
            {
                Instantiate(alien2, new Vector3(-125f, n, 0), Quaternion.identity);
                n -= 15;
            } 
        }
        else
        {
            //creates third type of alien
            GameObject a1 =Instantiate(alien3, new Vector3(-125f, 5f, 0), Quaternion.identity);
            GameObject a2 = Instantiate(alien32, new Vector3(-125f, -5f, 0), Quaternion.identity);
            //spawns laser destination objects
            GameObject dest = Instantiate(destination.gameObject, destination.transform.position, Quaternion.identity);
            GameObject dest2 = Instantiate(destination2.gameObject, destination2.transform.position, Quaternion.identity);
            //sets the partner aliens for the sequence
            a1.GetComponent<AlienController>().partnerAlien = a2;
            a2.GetComponent<AlienController>().partnerAlien = a1;
            //sets alien references in destinations
            dest.GetComponent<LaserDestination>().alien = a1;
            dest2.GetComponent<LaserDestination>().alien = a2;
            //sets destinations in the lasers
            a1.transform.GetChild(1).GetComponent<ExtendedLaser>().destination = dest.transform;
            a1.transform.GetChild(1).GetComponent<ExtendedLaser>().destination2 = dest2.transform;

            a2.transform.GetChild(1).GetComponent<ExtendedLaser>().destination = dest.transform;
            a2.transform.GetChild(1).GetComponent<ExtendedLaser>().destination2 = dest2.transform;

        }
    }

    void makeMiddleWall()
    { //spawn a middle wall with a random y value and a random scale if the other walls aren't too high
        Vector3 randVect = new Vector3(0f, Random.Range(0f, maxWallHeight), 0f);
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
            previousHeight = Random.Range(13f, maxWallHeight);
            heightVect = new Vector3(0f, previousHeight, 0f);
            randDiff = 0;
        }
        else
        {
            float min = previousHeight - 10; //get min value from previous height
            //if below 13, make it equal to 13 so it doesn't get too small
            if(min < 13)
            {
                min = 13;
            }
            //get random number between min and max
            randDiff = Random.Range(min, previousHeight + 10);
            //create vector with it
            heightVect = new Vector3(0f, randDiff, 0f);
            //update previous height to this
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
    { 
        IsRunning = true;
        //start making walls at speed that is scaled by the game level
        InvokeRepeating("MakeWall", 0f, wallSpawnSpeed - (0.05f *GameManagerS.Level));
        InvokeRepeating("makeMiddleWall", 0f, middleWallSpawnSpeed - (0.5f * GameManagerS.Level));
        //instantiate different enemy types depending on the stage
        if (GameManagerS.Stage < 3)
        {
            InvokeRepeating("makeEnemy", 0f, alienSpawnSpeed - (1 * GameManagerS.Level));
        }
        else if (GameManagerS.Stage < 6)
        {
            makeEnemy();
        }
        else
        {
            InvokeRepeating("makeEnemy", 0f, (alienSpawnSpeed*2) - (1 * GameManagerS.Level));
        }
        //increase alien bullet speed scaled by the level
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
        else if(GameManagerS.Stage < 6)
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
        else
        {
            GameObject[] aliens = GameObject.FindGameObjectsWithTag("Alien3");
            foreach (GameObject g in aliens)
            {
                Destroy(g.gameObject);
            }
        }
        //enable GUI
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

    private void ResetAliensToKill()
    {
        //more aliens to kill per level
        numAliensToKill = GameManagerS.Level * numAliensMultiplier;
        //more aliens to kill for laser aliens as they come in pairs
        if (GameManagerS.Stage >= 6)
        {
            numAliensToKill *= 2;
        }
    }
}
