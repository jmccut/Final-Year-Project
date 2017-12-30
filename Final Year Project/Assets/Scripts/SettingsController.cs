using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {
    private bool lastTouched; //holds the flag so the menu button either creates or destroys buttons

    void Start () {
        lastTouched = true;
        foreach (Transform b in transform)
        {
            b.gameObject.SetActive(false);
        }

    }
	
    public void SpawnMenu()
    {
        //if the button hasn't been touched before or last destroyed buttons-
        //-then instantiate buttons
        if (lastTouched)
        {
            foreach (Transform b in transform)
            {
                b.gameObject.SetActive(true);
            }
            lastTouched = false;
        }
        //otherwise the menu is currently up so destroy the buttons
        else
        {
            foreach (Transform b in transform)
            {
                b.gameObject.SetActive(false);
            }
            lastTouched = true;
        }

    }
}
