using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementArea : MonoBehaviour{
    public bool touched;
    public bool canFire;
    private Rect bounds;
    // Use this for initialization
    void Start () {
        touched = false;
        canFire = false;
        bounds = new Rect(0, 0, Screen.width / 2, Screen.height);
        
    }

    // Update is called once per frame
    void Update()
    {

        //if the player is touching the screen
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if ((!bounds.Contains(touch.position)) && touch.phase != TouchPhase.Ended)
            {
                touched = true;
                canFire = false;
            }
            else if (bounds.Contains(touch.position) && touch.phase == TouchPhase.Began)
            {
                canFire = true;
                touched = false;
            }
        }
        //sets touched to false if there is no touch input
        /*
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(0);

            if (bounds.Contains(touch1.position) && bounds.Contains(touch2.position))
            {
                canFire = true;
                touched = false;
            }
            if (bounds.Contains(touch1.position) && !bounds.Contains(touch2.position))
            {
                canFire = true;
                touched = true;
            }
            if (!bounds.Contains(touch1.position) && !bounds.Contains(touch2.position))
            {
                canFire = false;
                touched = true;
            }
        }
        */
        else if(Input.touchCount == 0){
            canFire = false;
            touched = false;
        }
    }
}
