using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
    private Transform playerTransform; //reference to the player transform
    private float speed; //sets the speed of the alien
    public static int Damage { get; set; } //sets the damage the alien does on contact with the ship
    public GameObject explosion;
    public Transform shotSpawn;
    private float nextFire;
    public float fireRate;
    public GameObject shot;
    public enum Type { TYPE1, TYPE2, TYPE3};
    public Type type;

    void Start () {
        //sets type of alien
        if(gameObject.CompareTag("Alien1"))
        {
            type = Type.TYPE1;
            Damage = 1;
            speed = 18 * GameManagerS.Level;
        }
        else if(gameObject.CompareTag("Alien2"))
        {
            
            type = Type.TYPE2;
            Damage = 50;
            speed = 400 * GameManagerS.Level;
        }
        else if(gameObject.CompareTag("Alien3"))
        {
            type = Type.TYPE3;
            Damage = 1;
            speed = 18 * GameManagerS.Level;
        }
        //gets player position
        try
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        #pragma warning disable CS0168 
        catch (System.NullReferenceException e)
        {
        #pragma warning restore CS0168 
        }
    }

    void FixedUpdate()
    { //moves enemy towards the player transform if ship is not already dead
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (playerTransform !=  null && type == Type.TYPE1)
        {
            rb.transform.position = Vector3.MoveTowards(rb.position, playerTransform.transform.position, Time.deltaTime * speed);
        }
        else if(type == Type.TYPE2)
        {
            if(rb.position.x < -110f)
            {
                rb.velocity = -Vector3.left * speed * Time.deltaTime;
            }
            else
            {
                rb.velocity = new Vector3();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if alien is hit by a bullet
        if (collision.CompareTag("Bullet"))
        {
            //make enemy explosion and then kill the particle effect after its duration
            GameObject bang = Instantiate(explosion, transform.position, transform.rotation);
            ParticleSystem parts = bang.GetComponent<ParticleSystem>();
            float totalDuration = parts.main.duration + parts.main.startLifetimeMultiplier;
            Destroy(bang, totalDuration);
            //spawn new enemy in its place
            Instantiate(gameObject, new Vector3(-125f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            //kill alien and bullet
            Destroy(collision.gameObject);
            Destroy(gameObject);
            GameController.numAliensToKill--; //decrements the number of aliens left to kill

        }

    }
    private void Update()
    {
        //shoots when the game is running and the fire area is being touched
        if (Time.time > nextFire && type == Type.TYPE2)
        {
            nextFire = Time.time + (fireRate - (0.35f * GameManagerS.Level));
            Shoot();
        }
    }

    void Shoot()
    { //makes shot
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
    }
}
