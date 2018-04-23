using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {
    private bool menuInActive; 
    public Toggle hardMode;
    public Toggle muted;
    private GameObject gc;

    void Start () {
        //gets reference to the game controller script
        gc = GameObject.FindGameObjectWithTag("GameController");
        
        menuInActive = true;

        //do not show settings buttons on start
        foreach (Transform b in transform)
        {
            b.gameObject.SetActive(false);
        }
        //sets toggles tick boxes based on what the current preferences are
        if (PlayerPrefs.GetInt("HardMode") == 1)
        {
            hardMode.isOn = true;
            PlayerPrefs.SetInt("HardMode", 1);
        }
        else if (PlayerPrefs.GetInt("HardMode") == 0)
        {
            hardMode.isOn = false;
        }

        if (PlayerPrefs.GetInt("IsMuted") == 1)
        {
            muted.isOn = true;
            PlayerPrefs.SetInt("IsMuted", 1);
            gc.GetComponent<AudioSource>().mute = true;
        }
        else if (PlayerPrefs.GetInt("IsMuted") == 0)
        {
            muted.isOn = false;
            gc.GetComponent<AudioSource>().mute = false;
        }

    }

    public void SpawnMenu()
    {
        //if the menu is currently inactive
        if (menuInActive)
        {
            //show menu
            foreach (Transform b in transform)
            {
                b.gameObject.SetActive(true);
            }
            //menu is active
            menuInActive = false;
        }
        //otherwise the menu is active so hide it
        else
        {
            //set menu components to inactive
            foreach (Transform b in transform)
            {
                b.gameObject.SetActive(false);
            }
            //menu is active
            menuInActive = true;
        }

    }
    //called when hard mode toggle is touched
    public void SetHardMode()
    {
        //if hard mode is currently off or has not been set, set it to true
        if (PlayerPrefs.GetInt("HardMode") == 0)
        {
            PlayerPrefs.SetInt("HardMode", 1);
        }
        //otherwise it must already be on so set it to false
        else if(PlayerPrefs.GetInt("HardMode") == 1)
        {
            PlayerPrefs.SetInt("HardMode", 0);
        }
    }

    //called when mute toggle is touched
    public void MuteGame()
    {
        //if the game was unmuted
        if (PlayerPrefs.GetInt("IsMuted") == 0)
        {
            //mute the game
            PlayerPrefs.SetInt("IsMuted", 1);
            gc.GetComponent<AudioSource>().mute = true;
        }
        //if the game was muted
        else if (PlayerPrefs.GetInt("IsMuted") == 1)
        {
            //unmute the game
            PlayerPrefs.SetInt("IsMuted", 0);
            gc.GetComponent<AudioSource>().mute = false;
        }
    }
}
