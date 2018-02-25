using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {
    private bool lastTouched; //holds the flag so the menu button either creates or destroys buttons
    public Toggle hardMode;
    public Toggle muted;
    private GameObject gc;

    void Start () {
        gc = GameObject.FindGameObjectWithTag("GameController");
        lastTouched = true;
        foreach (Transform b in transform)
        {
            b.gameObject.SetActive(false);
        }

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
