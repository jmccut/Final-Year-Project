using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {

    //holds the amount the background moves (larger increments move faster)
    private float scroll;
    public static float scrollSpeed; //40 = stationary

    void Update()
    {
        //changes the amount by which the background moves depending on if the game is running or not
        if (!(GameController.IsRunning))
        {
            //if we're not approaching a planet (don't want the background to move whilst at a planet)
            if (!PlanetController.atPlanet) {
                scrollSpeed = 40f;
            }
        }
        //if we are in a game
        else if(GameController.IsRunning)
        {
            //scroll background by speed scaled by game level (since walls move faster, BG needs to keep up)
            scrollSpeed = 163f + (GameManagerS.Level - 1) * 50;
        }

        //changes the position of the background
        scroll += scrollSpeed / 100000;
        Vector2 offset = new Vector2(0, scroll);
        //sets the necessary offset to create scrolling background effect
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetTextureOffset("_MainTex", offset);
    }
}
