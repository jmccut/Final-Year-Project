using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetHighlight : MonoBehaviour {
    public Image[] planetList; //list of planet images
	
	// Update is called once per frame
	void Update () {
        //highlight the current stage planet to green and make it the selected planet it other script
        for(int i = 0; i < planetList.Length; i++)
        {
            //if the index of the planet is equal to the stage number, this planet is the stage
            if(i == GameManagerS.Stage)
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
