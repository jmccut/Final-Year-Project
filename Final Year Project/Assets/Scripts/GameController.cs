using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

    public float wallHeightRange; //max height the wall can be
    public GameObject wall; //reference to the bottom wall prefab
    public GameObject TopWall; //reference to the top wall prefab
    public GameObject alien; //reference to the alien to spawn
    private bool isRunning; //flag to indicate if the game is running
    private int level; //used to indicate what level of the game the ship is on
    public GUIController GUI; //used to enable the GUI again after the level has ended
    public Text healthText; //used to set the health text
    public PlanetController planet; //used to instantiate planet at the end of a level
    public static int numAliensToKill;//holds the number of aliens to kill per level
    public static bool stageCleared; //flag for when a number of stages are cleared
    private int stage; //holds which stage the game is on
    public GameObject player; //holds reference to the player so they can be revived
    private float previousHeight; //holds the previous height of the walls
    private float wallSpawnSpeed; //holds the speed to spawn the walls
    private GameObject wallCopy;
    private GameObject TopWallCopy;

    void Start () {
        //sets the game to: not running, level 1, stage 1 and number of aliens to kill as 5
        IsRunning = false;
        Level = 1;
        Stage = 1;
        numAliensToKill = Level * 5; //sets the number of aliens to kill to level up
        previousHeight = -100; //arbitrary number so that the function knows it has not been initialised
        WallController.speed = 17; //sets starting speed for walls
        wallSpawnSpeed = 0.35f;
    }

    //used to safely access running flag
    public static bool IsRunning { get; set; }

    //used to safely access level
    public static int Level { get; set; }

    //used to safely access the stage number
    public static int Stage { get; set; }

    private void Update()
    {
        // if the game is running and the player has killed all aliens, level up
        if (IsRunning && numAliensToKill == 0)
        {
            LevelUp();
        }

        //sets the text for the health HUD
        healthText.text = "Health: "+ PlayerController.health;
    }

    void LevelUp()
    { //handles everything needed to level up

        EndGame();

        //if this is also the last level (the 5th)
        if (Level % 5 == 0)
        {
            //set flag, increment stage count and reset level count
            stageCleared = true;
            Stage++;
            Level = 0;
            WallController.speed = 17;
            wallSpawnSpeed = 0.35f;
        }

        //increment the level count
        Level++;

        //speeds up the wall speed and instantiation
        WallController.speed += 5;
        wallSpawnSpeed -= 0.05f;

        //set the number of aliens to kill for the current level
        numAliensToKill = Level *5;
    }

    void makeEnemy()
    { //spawn enemy at the beginning of the map in a random range along the y
        if (GameController.IsRunning)
        {
            Instantiate(alien, new Vector3(-120f, Random.Range(-30, 30), 0), Quaternion.identity);
        }
    }

    void makeMiddleWall()
    { //spawn a middle wall with a random y value and a random scale if the other walls aren't too high
        Vector3 randVect = new Vector3(0f, Random.Range(0f, wallHeightRange), 0f);
        if (wallCopy.transform.localScale.y < 95f)
        {
            GameObject middleWall = Instantiate(wall, new Vector3(-120f, Random.Range(-26f, 26), wall.transform.position.z), Quaternion.identity);
            middleWall.transform.localScale += randVect;
        }
        
    }

    void MakeWall()
    { //make two copies of walls, 1 top & 1 bottom and instantiate them with the same with random heights
      //based on height before it

      //------------------------------------------------------------------------------------
      //TODO: fix this so that when the wall speed increases it doesn't just spawn low walls
      //------------------------------------------------------------------------------------

        Vector3 heightVect; //holds the height vector to be applied
        float randDiff; //random difference to be applied to next wall

        //if the previous height has not been stored yet, make the previous height a random size
        if (previousHeight == -100)
        {
            previousHeight = Random.Range(0, wallHeightRange);
            heightVect = new Vector3(0f, previousHeight, 0f);
            randDiff = 0;
        }
        //otherwise, make a random height from the previous one differing by 10 at most
        else
        {
            randDiff = Random.Range(previousHeight -10, previousHeight +10);
            heightVect = new Vector3(0f, randDiff, 0f);
            previousHeight = randDiff;
        }

        //instantiate wall objects at location off the screen
        wallCopy = Instantiate(wall, new Vector3(-120, -46.3f, wall.transform.position.z), Quaternion.identity);
        TopWallCopy = Instantiate(TopWall, new Vector3(-120, 46.3f, wall.transform.position.z), Quaternion.identity);

        //if applying the scaling would result in a wall of negative height, give it a new height in the range 1-10
        if ((wallCopy.transform.localScale + heightVect).y < 0)
        {  
            heightVect = new Vector3(0f, Random.Range(-24f, -15f), 0f);
        }
        //if applying the scaling would make the wall too tall, give it a new height in the range 60-90
        else if ((wallCopy.transform.localScale + heightVect).y > 120)
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
        InvokeRepeating("MakeWall", 0f, wallSpawnSpeed);
        InvokeRepeating("makeMiddleWall", 0f, 5f);
        InvokeRepeating("makeEnemy", 0f, 5f);
        GUI.DisableCanvas();
    }

    public void EndGame()
    { //set the game to ended, stop making walls, kill the aliens and enable the GUI again
        IsRunning = false;
        CancelInvoke();
        GameObject[] aliens = GameObject.FindGameObjectsWithTag("Alien1");
        foreach(GameObject g in aliens)
        {
            Destroy(g.gameObject);
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

        numAliensToKill = Level * 5;
        player.GetComponent<Renderer>().enabled = true;
        player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(0.0f, 0.0f, 0.0f), 50 * Time.deltaTime);
    }
}
