using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlanetTouch : MonoBehaviour, IPointerClickHandler
{
    public GameController gameController;
    public bool selected;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (selected)
        {
            GetComponent<Image>().color = Color.green;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
    //if the planet is the current stage planet then clicking it will start the game
    public void OnPointerClick(PointerEventData eventData)
    {
        if (selected)
        {
            gameController.StartGame();
        }
    }
}
