using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {

    public float scrollSpeed;

    // Update is called once per frame
    void Update()
    {
        //changed the amount by which the background moves depending on if the game is running or not
        if (!(GameController.IsRunning))
        {
            if (!PlanetController.atPlanet) { 
                scrollSpeed += 0.0004f;
            }
        }
        else if(GameController.IsRunning)
        {
            scrollSpeed += 0.00175f;
        }
        //changes the position of the background manually
        Vector2 offset = new Vector2(0, scrollSpeed);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetTextureOffset("_MainTex", offset);
    }
}
