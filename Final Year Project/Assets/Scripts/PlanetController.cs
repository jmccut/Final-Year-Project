﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

    //the following hold the sprites for the planets
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
    
    //target for the planet movement
    public Vector3 target = new Vector3(-87f, -25f, -0.5f);

    //flag to show the planet has stopped moving
    public static bool atPlanet;

    private void Start()
    { //set the initial planet position before movement and flag
        transform.position = new Vector3(-200f, -25f, -0.5f);
        atPlanet = false;
    }

    private void Update()
    {
        //if the stage has been cleared, begin planet approach
        if (GameController.stageCleared && !GameController.IsRunning)
        {
            //changes sprite of planet to relevant one
            ChangeSprite(GameController.Stage-1);

            //moves the planet to come into view when instantiated
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 10);

            //if the planet has reached its target, stop movement and set flag
            if (transform.position == target)
            {
                GameController.stageCleared = false;
                atPlanet = true;
            }
        }

        //if the game starts running again, move the planet back at twice the speed and reset flag
        else if (GameController.IsRunning)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-200f, -25f, -0.5f), Time.deltaTime * 25);
            atPlanet = false;
        }
    }

    public void ChangeSprite(int stageNum)
    { //changes sprite and scale of planet based on what planet we're approaching
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