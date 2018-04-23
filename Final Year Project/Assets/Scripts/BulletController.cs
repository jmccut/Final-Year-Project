using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script is shared between alien and player bullet
public class BulletController : MonoBehaviour {
    public float playerSpeed; //speed of the bullet for the player
    public static float Speed { get; set; } //holds speed of the bullets for alien
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //if player bullet
        if (gameObject.CompareTag("Bullet"))
        {
            //different speed for player bullets
            playerSpeed = 100;
            //rotate the bullet to face forwards
            transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            //shoot the bullet left
            rb.velocity = Vector3.left * playerSpeed;
        }
        //if enemy bullet
        else
        {
            //slower speed
            Speed = 25;
            //shot bullet right
            rb.velocity = -Vector3.left * Speed;
        }

    }

    private void Update()
    {
        //kills the bullet once it leaves the game area
        if (transform.position.x > 65 || transform.position.x < -130)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if it hits a wall or another bullet, kill it
        if (collision.CompareTag("Wall") || collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
