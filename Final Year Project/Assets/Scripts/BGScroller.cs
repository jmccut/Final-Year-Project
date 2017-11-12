using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {

    //holds the amount the background moves (larger increments move faster)
    public float scrollSpeed;

    void Update()
    {
        //changes the amount by which the background moves depending on if the game is running or not
        if (!(GameController.IsRunning))
        {
            //if we're not approaching a planet (don't want the background to move whilst at a planet)
            if (!PlanetController.atPlanet) { 
                scrollSpeed += 0.0004f;
            }
        }
        //if we are in a game
        else if(GameController.IsRunning)
        {
            scrollSpeed += 0.00175f;
        }

        //changes the position of the background
        Vector2 offset = new Vector2(0, scrollSpeed);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetTextureOffset("_MainTex", offset);
    }
}
