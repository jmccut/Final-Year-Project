using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

    //the following holds the sprites for the planets
    public Sprite mercury;
    public Sprite venus;
    public Sprite earth;
    public Sprite moon;
    public Sprite mars;
    public Sprite jupiter;
    public Sprite saturn;
    public Sprite uranus;
    public Sprite neptune;
    public Sprite pluto;
    public ChangeScene change;
    //target for the planet movement
    public Vector3 target; 

    //flag to show the planet has stopped moving
    public static bool atPlanet;

    private void Start()
    {
        atPlanet = false;
        //since this script is used by 2 different objects in different scene, i need to differentiate:

        //if this is a planet that appears before the boss level
        if (!gameObject.CompareTag("MapPlanet"))
        {
            //set position and target position to move to
            transform.position = new Vector3(-200f, -25f, -0.5f);
            target = new Vector3(-87f, -25f, -0.5f);
            //changes sprite of planet to relevant one
            ChangeSprite(GameManagerS.Stage - 1);
        }
        //if this is a planet object that appears in the planet invasion scene
        else
        {
            //set current and target positions
            transform.position = new Vector3(115f, -25f, -0.5f);
            target = new Vector3(40f, -25f, 0f);
            //change to the correct sprite based on which planet has been invaded
            if (GameManagerS.CurrentPlanet > 1)
            {
                ChangeSprite(GameManagerS.CurrentPlanet);
            }
        }
    }

    private void Update()
    {
        //if the stage has been cleared, begin moving planet into scene
        if (GameController.stageCleared && !GameController.IsRunning && !gameObject.CompareTag("MapPlanet"))
        {
            //moves the planet to come into view when instantiated
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 10);

            //if the planet has reached its target, stop movement and set flag
            if (transform.position == target)
            {
                GameController.stageCleared = false;
                atPlanet = true;
                GameManagerS.OnBossLevel = true;
                change.Change(2); //change to boss level
            }
        }
        //if the game starts running again, move the planet back the original position
        else if (GameController.IsRunning && !gameObject.CompareTag("MapPlanet"))
        {
            transform.position = new Vector3(-200f, -25f, -0.5f);
            atPlanet = false;
        }
        //move towards target if planet invasion planet
        if (gameObject.CompareTag("MapPlanet"))
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 2);
        }
    }

    public void ChangeSprite(int stageNum)
    { //changes sprite and scale of planet based on index given as argument
        switch (stageNum)
        {
            case 1:
                GetComponent<SpriteRenderer>().sprite = moon;
                transform.localScale = new Vector3(10f, 10f, 1f);
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = mars;
                transform.localScale = new Vector3(7f, 7f, 1f);
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = jupiter;
                transform.localScale = new Vector3(17f, 17f, 1f);
                break;
            case 4:
                GetComponent<SpriteRenderer>().sprite = saturn;
                transform.localScale = new Vector3(15.5f, 15.5f, 1f);
                break;
            case 5:
                GetComponent<SpriteRenderer>().sprite = uranus;
                transform.localScale = new Vector3(13f, 13f, 1f);
                break;
            case 6:
                GetComponent<SpriteRenderer>().sprite = neptune;
                transform.localScale = new Vector3(13f, 13f, 1f);
                break;
            case 7:
                GetComponent<SpriteRenderer>().sprite = pluto;
                transform.localScale = new Vector3(10f, 10f, 1f);
                break;
            case 8:
                GetComponent<SpriteRenderer>().sprite = venus;
                transform.localScale = new Vector3(5f, 5f, 1f);
                break;
            case 9:
                GetComponent<SpriteRenderer>().sprite = mercury;
                break;
        }
    }
}
