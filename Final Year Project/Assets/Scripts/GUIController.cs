﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {
    //references
    public GameObject player;
    public GameObject arrow;
    public GameObject tutorial;

    public void DisableCanvas()
    { //disables each child of the GUI except health, shield and tutorial which is needed during the game
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("Health") && !child.CompareTag("Zone") && !child.CompareTag("Invul") && !child.CompareTag("Tutorial"))
            {
                child.gameObject.SetActive(false);
            }
        }
        
    }
    public IEnumerator EnableCanvas()
    { //waits until all wall objects have left the scene before showing GUI
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Wall") == null);
        //enable all gui components
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            //change ship back to bigger size once in menu
            player.transform.localScale = new Vector3(15f, 25f, 1f);
            player.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        }
        //set bouncing arrow to active if on level 1 or 2
        if (GameManagerS.Stage == 1 && GameManagerS.Level == 2)
        {
            tutorial.SetActive(true);
            arrow.SetActive(true);
        }
        //otherwise set it to false
        else
        {
            tutorial.SetActive(false);
            arrow.SetActive(false);
        }
    }
}
