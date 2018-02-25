using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBLaserControl : MonoBehaviour {
    Rigidbody2D rb;
    public int speed;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 250;
        Destroy(gameObject, 10f);
    }
    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed * Time.deltaTime;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
