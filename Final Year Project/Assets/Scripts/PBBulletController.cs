using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBBulletController : MonoBehaviour
{
    public float playerSpeed; //speed of the bullet for the player
    public static float Speed { get; set; } //holds speed of the bullets
    Rigidbody2D rb;
    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
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
            Speed = 25;
            transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            rb.velocity = Vector3.left * Speed;
        }

    }

    private void Update()
    {
        //kills the bullet once it leaves the game area
        if (transform.position.x > 65)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Laser") && gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
        if (!collision.CompareTag("Alien1") && gameObject.CompareTag("Alien Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
