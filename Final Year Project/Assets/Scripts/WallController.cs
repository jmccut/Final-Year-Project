using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {
    public float speed; //speed of the wall
	
	void FixedUpdate ()
    { //moves the wall by right by its speed
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.right * speed;

        //destroys the wall if it has moved outside the game area
        if(transform.position.x > 54)
        {
            Destroy(gameObject);
        }
    }
}
