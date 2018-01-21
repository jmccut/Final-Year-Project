using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public float playerSpeed;
    public static float Speed { get; set; } //holds speed of the bullets
    Rigidbody2D rb;
    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        if (gameObject.CompareTag("Bullet"))
        {
            playerSpeed = 100;
            //rotate the bullet to face forwards
            transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            //shoot the bullet left
            rb.velocity = Vector3.left * playerSpeed;
        }
        else
        {
            Speed = 25;
            rb.velocity = -Vector3.left * Speed;
        }

    }

    private void Update()
    {
        //kills the bullet once it leaves the game area
        if (transform.position.x < -130)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
