using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBBombController : MonoBehaviour {
    //stats
    int speed;
    private float nextFire;
    public float fireRate;
    Vector3 spawnPos;
    //flags
    bool dropped;
    //references
    Transform player;
    public Transform[] shotSpawns;
    Rigidbody2D rb;
    public GameObject bullet;
    public GameObject explosion;

    private void Start()
    {
        //saves position it spawned at so can see how far it travelled
        //used to evaluate if dropped
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
        //if the bomb has not dropped yet, keep moving it down
        if (Mathf.Abs(rb.position.y - spawnPos.y) < 20 && !dropped)
        {
            rb.velocity = -Vector3.up * speed;
        }
        //if it has reached necessary distance from spawn
        else
        {
            //set flag and move it towards player
            dropped = true;
            rb.MovePosition(Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime));
        }
    }

    private void Update()
    {
        //rotate the bomb
        transform.Rotate(new Vector3(0, 0, 10) * 10 * Time.deltaTime);
        //if the time till the next fire has elapsed
        if (Time.time > nextFire)
        {
            //reset it according to the fire rate and shoot
            nextFire = Time.time + (fireRate);
            Shoot();
        }
    }

    void Shoot()
    {
        //shoot a bullet for each shot position (4)
        foreach(Transform t in shotSpawns)
        {
            Instantiate(bullet, t.position, t.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the bomb is hit with a bullet or missile
        if (collision.gameObject.CompareTag("Bullet")|| collision.gameObject.CompareTag("Missile"))
        {
            //make enemy explosion and then kill the particle effect after its duration
            GameObject bang = Instantiate(explosion, transform.position, transform.rotation);
            ParticleSystem parts = bang.GetComponent<ParticleSystem>();
            float totalDuration = parts.main.duration + parts.main.startLifetimeMultiplier;
            Destroy(bang, totalDuration);

            GameManagerS.Money += 5;
            SoundController.GetSound(2).Play();
            //if it was a bullet, kill gameobject
            if (collision.gameObject.CompareTag("Bullet")){
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
