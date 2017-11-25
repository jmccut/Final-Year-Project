﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetHighlight : MonoBehaviour {
    public Image[] planetList;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //set the current stage planet to green and make it the selected planet
        for(int i = 0; i < planetList.Length; i++)
        {
            if(i == GameController.Stage)
            {
                planetList[i].GetComponent<PlanetTouch>().selected = true;
            }
            else
            {
                planetList[i].GetComponent<PlanetTouch>().selected = false;
            }
        }

    }
}