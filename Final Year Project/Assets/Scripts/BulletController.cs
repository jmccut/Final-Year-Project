using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public GameObject explosion;
    public float speed; //holds speed of the bullets

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        //rotate the bullet to face forwards
        transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);

        //shoot the bullet left
        rb.velocity = Vector3.left * speed;
    }

    private void Update()
    {
        //kills the bullet once it leaves the game area
        if (transform.position.x < -130)
        {
            Destroy(gameObject);
        }
    }
}
