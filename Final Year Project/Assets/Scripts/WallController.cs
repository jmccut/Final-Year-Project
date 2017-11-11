using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {
    public float speed; //speed of the wall
	// Use this for initialization
	void Start () {
  
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.right * speed;
        //kills the wall once it leaves the game area
        if(transform.position.x > 54)
        {
            Destroy(gameObject);
        }
    }
}
