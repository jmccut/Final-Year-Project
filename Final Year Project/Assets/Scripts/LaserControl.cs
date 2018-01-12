using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControl : MonoBehaviour {
    public float speed; //holds speed of the bullet
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //shoot the bullet left
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the bullet hit something other than the player
        if (!other.CompareTag("Bullet"))
        {
            Destroy(gameObject);    
        }
    }
}
