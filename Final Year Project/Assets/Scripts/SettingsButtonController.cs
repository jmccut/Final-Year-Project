using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButtonController : MonoBehaviour {
    private bool lastTouched;
    private void Start()
    {
        foreach (Transform b in transform)
        {
            if (b.gameObject.CompareTag("SettingsImage"))
            {
                b.gameObject.SetActive(false);
            }
        }
    }

    public void spawnScreen()
    {
        if (lastTouched)
        {
            foreach (Transform b in transform)
            {
                if (b.gameObject.GetType() == typeof(Image))
                {
                    Debug.Log("im here");
                    b.gameObject.SetActive(true);
                }
                
            }
            lastTouched = false;
        }
        //otherwise the menu is currently up so destroy the buttons
        else
        {
            foreach (Transform b in transform)
            {
                if (b.gameObject.CompareTag("SettingsImage"))
                {
                    b.gameObject.SetActive(false);
                }
            }
            lastTouched = true;
        }
        


    }
}
