using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlanetTouch : MonoBehaviour, IPointerClickHandler
{ 
    public GameController gameController;
    public bool selected;
    public Text message;

    void Update () {
        //if the planet has been selected
        if (selected)
        {
            //make it flash green
            GetComponent<Image>().color = Color.Lerp(Color.white, Color.green, 
                Mathf.PingPong(Time.time, 1));
        }
        //otherwise leave it normal
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }

    //when player touches planet
    public void OnPointerClick(PointerEventData eventData)
    {
       //if the planet is the stage planet
        if (selected)
        {
            //parses base damage list to see if there are any bases that need repairing first
            bool temp = false;
            foreach(int i in GameManagerS.BaseDamage)
            {
                if(i == 100)
                {
                    temp = true;
                    StartCoroutine(FadeTextToZeroAlpha(6,message));
                }
            }
            //if there are no bases that need repairing, start the game
            if(!temp){
                gameController.StartGame();
            }
            
        }
    }
    //method used to fade the alpha value of the font colour of text so it is less abrupt
    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.gameObject.SetActive(true);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        i.gameObject.SetActive(false);
    }
}