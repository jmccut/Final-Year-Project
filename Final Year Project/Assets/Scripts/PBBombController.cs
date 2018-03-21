using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBBombController : MonoBehaviour {
    Vector3 spawnPos;
    Rigidbody2D rb;
    int speed;
    bool dropped;
    Transform player;
    public Transform[] shotSpawns;
    private float nextFire;
    public float fireRate;
    public GameObject bullet;
    public GameObject explosion;
    private void Start()
    {
        //saves position it spawned at so can see how far it travelled
        spawnPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        speed = 14;
        //gets player position
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch (System.NullReferenceException)
        {
        }
    }
    private void FixedUpdate()
    {
        //once the missile has dropped out of the ship, shoot it left
        if (Mathf.Abs(rb.position.y - spawnPos.y) < 20 && !dropped)
        {
            rb.velocity = -Vector3.up * speed;
        }
        else
        {
            //the missile has fully dropped
            dropped = true;
            rb.MovePosition(Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime));
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, 10) * 10 * Time.deltaTime);
        if (Time.time > nextFire)
        {
            nextFire = Time.time + (fireRate);
            Shoot();
        }
    }

    void Shoot()
    {
        foreach(Transform t in shotSpawns)
        {
            Instantiate(bullet, t.position, t.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")|| collision.gameObject.CompareTag("Missile"))
        {
            //make enemy explosion and then kill the particle effect after its duration
            GameObject bang = Instantiate(explosion, transform.position, transform.rotation);
            ParticleSystem parts = bang.GetComponent<ParticleSystem>();
            float totalDuration = parts.main.duration + parts.main.startLifetimeMultiplier;
            Destroy(bang, totalDuration);
            GameManagerS.Money += 5;
            SoundController.GetSound(2).Play();
            if (collision.gameObject.CompareTag("Bullet")){
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
