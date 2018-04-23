using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
    public bool done; //flag for when the tutorial is finished
    private int count; //count number of touches
	
	void Start () {
        count = 0;
        done = false;

        //sets everything that isn't the arrow to be inactive
		foreach(Transform t in transform)
        {
            if (!t.CompareTag("Arrow"))
            {
                t.gameObject.SetActive(false);
            }
            else
            {
                t.gameObject.SetActive(true);
            }
        }
	}

	void Update () {
        //if the game is running, the level is less than 2, it is the first stage and tutorial is not done yet
        if (GameController.IsRunning && !done && GameManagerS.Level < 2 && GameManagerS.Stage ==1)
        {
            //pause game
            Time.timeScale = 0f;
            //set everything which isn't the arrow to true
            //this sets the fire and lift area overlay to true
            foreach (Transform t in transform)
            {
                if (t.CompareTag("Arrow"))
                {
                    t.gameObject.SetActive(false);
                }
                else
                {
                    t.gameObject.SetActive(true);
                }
            }
            //if they touch the screen
            if (Input.touchCount > 0)
            {
                //if not for the first time
                count++;
                if (count >= 2)
                {
                    //disable all tutorial
                    foreach (Transform t in transform)
                    {
                        t.gameObject.SetActive(false);
                    }
                    //unpause the game and set flag
                    Time.timeScale = 1f;
                    gameObject.SetActive(false);
                    done = true;
                }
            }
        }
        //if the level is 2 show the arrow again for reinforcement
        else if (GameManagerS.Level == 2 && GameManagerS.Stage == 1 && GameController.IsRunning)
        {
            foreach (Transform t in transform)
            {

                t.gameObject.SetActive(false);
            }
        }
	}

}
