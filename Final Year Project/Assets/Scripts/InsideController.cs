using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InsideController : MonoBehaviour {
    //object references
    public GameObject enemy;
    public GameObject boss;
    public GameObject healthPack;
    public Button restart;
    //stats
    public static int KilledCount { get; set; } //counts number of enemies killed
    public static int numberOfEnemies { get; set; } //number of normal enemies in level
    //script reference
    public ChangeScene change;
    //text to update
    public Text startText;
    public Text moneyT;
    public Text partsT;
    public Text hint;
    //list of alive aliens
    public static List<GameObject> AliveInvaders { get; set; }

    void Start() {
        numberOfEnemies = 5;
        //display start text
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 4) {
            StartCoroutine(FadeTextToZeroAlpha(4, startText));
        }
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
        //if this is the first level of the ship
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            //if this is the first time the player is playing the boss level
            if (GameManagerS.Stage == 2)
            {
                //show hint
                hint.gameObject.SetActive(true);
            }
            else
            {
                hint.gameObject.SetActive(false);
            }
        }
        AliveInvaders = new List<GameObject>();
    }
    private void Update()
    {
        //update text
        moneyT.text = "£" + GameManagerS.Money;
        partsT.text = "" + GameManagerS.Parts;

        //if this is the boss level and all enemies are dead
        if (SceneManager.GetActiveScene().buildIndex == 4 &&
            KilledCount == (numberOfEnemies + 1))
        {
            GameManagerS.OnBossLevel = false;
            //if double resources were on, turn them off
            if (GameManagerS.PowerUps[2])
            {
                GameManagerS.PowerUps[2] = false;
            }
            change.Change(1); //change back to main scene
            
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

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        //fade text
        i.gameObject.SetActive(true);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        i.gameObject.SetActive(false);
    }
}
