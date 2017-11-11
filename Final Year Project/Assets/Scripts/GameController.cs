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
    private int stage;

    // Use this for initialization
    void Start () {
        IsRunning = false;
        Level = 4;
        Stage = 1;
        numAliensToKill = Level ;
    }
    public static bool IsRunning { get; set; }
    public static int Level { get; set; }

    public static int Stage { get; set; }

    private void Update()
    {
        /*
        if the number of aliens left has reached 0 in the current level;
            -End the level
            -update level count
            -adjust the number of aliens to kill for next round
            -check if the number of levels passed yet is enough to clear the stage
         */
        if (IsRunning && numAliensToKill == 0)
        {
            EndGame();
            
            if (Level % 5 == 0)
            {
                stageCleared = true;
                Stage++;
            }
            Level++;
            numAliensToKill = Level;
        }
        //sets the text for the health HUD
        healthText.text = "Health: "+ PlayerController.health;
    }

    void makeEnemy()
    {
        //spawn enemy at the beginning of the map in a random range along the y
        Instantiate(alien, new Vector3(-120f, Random.Range(-30,30), 0), Quaternion.identity);
    }

    void makeMiddleWall()
    {
        //spawn a middle wall with a random y value and a random scale
        Vector3 randVect = new Vector3(0f, Random.Range(0f, wallHeightRange), 0f);
        GameObject middleWall = Instantiate(wall, new Vector3(-120f, Random.Range(-26f,26), wall.transform.position.z), Quaternion.identity);
        middleWall.transform.localScale += randVect;
       
    }

    void MakeWall()
    {
        //make two copies of walls, 1 top & 1 bottom and instantiate them with the same random scale
        Vector3 randVect = new Vector3(0f, Random.Range(0f, wallHeightRange), 0f);

        GameObject wallCopy = Instantiate(wall, new Vector3(-120, -46.3f, wall.transform.position.z), Quaternion.identity);
        GameObject TopWallCopy = Instantiate(TopWall, new Vector3(-120, 46.3f, wall.transform.position.z), Quaternion.identity);

        wallCopy.transform.localScale += randVect;
        wallCopy.transform.position += new Vector3(0f, randVect.y * 0.15f, 0f);

        TopWallCopy.transform.localScale += randVect;
        TopWallCopy.transform.position += new Vector3(0f, randVect.y * -0.15f, 0f);

        
        
    }

    public void StartGame()
    {
        //if the game is running, instantiate walls
        IsRunning = true;
        InvokeRepeating("MakeWall", 0f, 0.35f);
        InvokeRepeating("makeMiddleWall", 0f, 5f);
        InvokeRepeating("makeEnemy", 0f, 5f);
        GUI.DisableCanvas();
    }

    public void EndGame()
    {
        //if the game has ended, stop making walls, kill the aliens and enable the GUI again
        IsRunning = false;
        CancelInvoke();
        GameObject[] aliens = GameObject.FindGameObjectsWithTag("Alien1");
        foreach(GameObject g in aliens)
        {
            Destroy(g.gameObject);
        }

        StartCoroutine(GUI.EnableCanvas());
    }

    public void LevelUp()
    {
        
    }
}
