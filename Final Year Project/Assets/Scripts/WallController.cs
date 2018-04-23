using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {
    public float speed; //speed of the wall (set in editor)
    
    void FixedUpdate ()
    { //fetches the rigidbody component of the object to control movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        //changes the velocity of the wall to move right by a speed scaled by the game level
        rb.velocity = Vector3.right * (speed + (5*GameManagerS.Level));

        //destroys the wall if it has moved outside the game area
        if(transform.position.x > 54)
        {
            Destroy(gameObject);
        }
    }
}
