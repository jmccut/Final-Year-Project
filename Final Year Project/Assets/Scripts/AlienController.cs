using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
    private Transform playerTransform; //reference to the player transform
    public float speed; //sets the speed of the alien
    public int damage; //sets the damage the alien does on contact with the ship
    public GameObject explosion;

    void Start () {
        //gets player position
        try
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        #pragma warning disable CS0168 
        catch (System.NullReferenceException e)
        {
        #pragma warning restore CS0168 
        }
    }

    void FixedUpdate()
    { //moves enemy towards the player transform if ship is not already dead
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (playerTransform !=  null && GameController.IsRunning)
        {
            rb.transform.position = Vector3.MoveTowards(rb.position, playerTransform.transform.position, Time.deltaTime * speed);
        }
    }
}
