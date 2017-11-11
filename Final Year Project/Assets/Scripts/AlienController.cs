using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
    private Transform playerTransform; //reference to the player transform to chase
    public float speed; //sets the speed of the alien
    public int damage; //sets the damage the alien does on contact with the ship
	// Use this for initialization
	void Start () {
        //gets player position
        try
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
#pragma warning disable CS0168 // Variable is declared but never used
        catch (System.NullReferenceException e)
        {
        #pragma warning restore CS0168 // Variable is declared but never used
        }
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //moves enemy towards the player transform if ship is not already dead
        if (playerTransform != null)
        {
            rb.transform.position = Vector3.MoveTowards(rb.position, playerTransform.transform.position, Time.deltaTime * speed);
        }
    }
}
