using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
    public bool done;
    private int count;
	// Use this for initialization
	void Start () {
        count = 0;
        done = false;
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
        if (GameController.IsRunning && !done && GameManagerS.Level != 2)
        {
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
            if (Input.touchCount > 0)
            {
                count++;
                if (count >= 2)
                {
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
        else if (GameManagerS.Level == 2 && GameController.IsRunning)
        {
            foreach (Transform t in transform)
            {

                t.gameObject.SetActive(false);
            }
        }
	}

}
