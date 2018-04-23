using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBLaserControl : MonoBehaviour {
    Rigidbody2D rb;
    public int speed;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //set speed and destroy object after 10 seconds
        //this is since they are spawned in mass so they need to auto-destruct to reduce lag
        speed = 250;
        Destroy(gameObject, 10f);
    }
    private void FixedUpdate()
    {
        //move bullet to the right by the speed scaled by the frame rate
        rb.velocity = transform.right * speed * Time.deltaTime;
    }
}
