using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreenController : MonoBehaviour {
    public bool mouseOnImage;
    public bool exit;
	// Use this for initialization
	void Start () {
        mouseOnImage = false;
        exit = false;
	}
    //HACK: This will have to be changed for mobile
    private void OnMouseEnter()
    {
        mouseOnImage = true;
        exit = false;
    }

    public void exitFalse()
    {
        exit = false;
    }
    
    private void OnMouseExit()
    {
        mouseOnImage = false;
        exit = true;
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetButtonDown("Fire1") && !mouseOnImage && exit)
        { 
            gameObject.SetActive(false);
            exit = false;
        }
    }
}
