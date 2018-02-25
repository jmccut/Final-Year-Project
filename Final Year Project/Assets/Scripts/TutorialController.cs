using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
    public bool done;
	// Use this for initialization
	void Start () {
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
        if (GameController.IsRunning && !done)
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
        }
        if(Input.touchCount > 0)
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            done = true;
        }
	}

}
