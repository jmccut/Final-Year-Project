using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBBulletController : MonoBehaviour
{
    //this script is used by player and alien bullets and so require different speeds and movements
    public float playerSpeed; //player speed
    public static float Speed { get; set; } //alien bullet speed
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //if this is the player bullet
        if (gameObject.CompareTag("Bullet"))
        {
            playerSpeed = 100;
            //rotate the bullet to face forwards
            transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
            //shoot the bullet left
            rb.velocity = Vector3.right * playerSpeed;
        }
        //if enemy bullet
        else
        {
            //set rotation, speed and velocity
            Speed = 25;
            transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            rb.velocity = Vector3.left * Speed;
        }

    }

    private void Update()
    {
        //kills the bullet once it leaves the game area
        if (transform.position.x > 65
            || transform.position.x < -130)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision for player bullet
        if (!collision.CompareTag("Player") && !collision.CompareTag("Laser") && collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
        //collision for alien bullet
        if (!collision.CompareTag("Alien1") && !collision.CompareTag("Alien Bullet") && gameObject.CompareTag("Alien Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
