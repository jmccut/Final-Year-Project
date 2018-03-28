using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
    public bool done;
    private int count;
	
	void Start () {
        count = 0;
        done = false;
        //sets everything that isn't the arrow to false
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
        //if the game is running and tutorial is not done yet
        if (GameController.IsRunning && !done && GameManagerS.Level < 2 && GameManagerS.Stage ==1)
        {
            //pause game and show fire and lift button tutorials
            Time.timeScale = 0f;
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
                    Time.timeScale = 1f;
                    gameObject.SetActive(false);
                    done = true;
                }
            }
        }
        //if the level is 2 show the arrow
        else if (GameManagerS.Level == 2 && GameManagerS.Stage == 1 && GameController.IsRunning)
        {
            foreach (Transform t in transform)
            {

                t.gameObject.SetActive(false);
            }
        }
	}

}
