using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBAlienController : MonoBehaviour {
    public static int alien1Dam;
    public static int alien2Dam;
    public static int alien3Dam;
    private Transform playerTransform; //reference to the player transform
    private float speed; //sets the speed of the alien
    public GameObject explosion;
    public Transform shotSpawn;
    private float nextFire;
    public float fireRate;
    public GameObject shot;
    public enum Type { TYPE1, TYPE2, TYPE3 };
    public Type type;
    public float health;
    Rigidbody2D rb;
    private AudioSource shootingSound;
    SpriteRenderer rend;
    private bool touching;

    // Use this for initialization
    void Start () {
        rend = GetComponent<SpriteRenderer>();
        alien1Dam = 25;
        alien2Dam = 150;
        alien3Dam = 15;
        PBGameController.AliveAliens.Add(gameObject);
        rb = GetComponent<Rigidbody2D>();
        //sets type of alien
        if (gameObject.CompareTag("Alien1"))
        {
            type = Type.TYPE1;
            speed = 20;
            health = 75;
        }
        else if (gameObject.CompareTag("Alien2"))
        {
            type = Type.TYPE2;
            speed = 20;
            health = 5;
        }
        else if (gameObject.CompareTag("Alien3"))
        {
            type = Type.TYPE3;
            speed = 10;
            health = 350;
            fireRate = 4;
        }
        //gets player position
        try
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch (System.NullReferenceException)
        {
        }
        shootingSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        //shoots when the game is running and the fire area is being touched
        if (Time.time > nextFire && (type == Type.TYPE1 || type == Type.TYPE3))
        {
            nextFire = Time.time + (fireRate - (0.35f * GameManagerS.Level));
            Shoot();
        }
        if (health <= 0)
        {
            Dead();
        }
    }
    void flee(Collider2D collision)
    {
        touching = true;
        Vector2 desiredVelocity = (rb.transform.position - collision.gameObject.transform.position);
        desiredVelocity.Normalize();
        desiredVelocity *= 25f;
        Vector2 direction = desiredVelocity - rb.velocity;
        direction = new Vector2(0f, direction.y);
        if (rb.position.y < 40 && rb.position.y > -40)
        {
            rb.velocity = direction;
        }
    }
    void FixedUpdate()
    { //moves enemy towards the player transform if ship is not already dead
        if (playerTransform != null && type == Type.TYPE1)
        {
            if (!touching)
            {
                rb.velocity = new Vector3();
                rb.MovePosition(Vector3.MoveTowards(rb.position,
                    new Vector3(0f, playerTransform.position.y, 0f),
                    Time.deltaTime * speed));
            }
        }
        else if (playerTransform != null && type == Type.TYPE2)
        {
            rb.velocity = new Vector3();
            rb.MovePosition(Vector3.MoveTowards(rb.position,
                new Vector3(playerTransform.position.x + 10, playerTransform.position.y, playerTransform.position.z),
                Time.deltaTime * speed));
        }
        else if (playerTransform != null && type == Type.TYPE3)
        {
            rb.velocity = new Vector3();
            rb.MovePosition(Vector3.MoveTowards(rb.position,
                new Vector3(20, transform.position.y, transform.position.z),
                Time.deltaTime * speed));
        }
    }

    public void Shoot()
    {
        //makes shot
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        if (type == Type.TYPE1)
        {
            shootingSound.Play();
        }
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        //if alien is hit by a bullet
        if (collision.CompareTag("Bullet"))
        {
            //flash red
            rend.material.color = Color.white;
            yield return new WaitForSeconds(.1f);
            rend.material.color = Color.red;
            yield return new WaitForSeconds(.1f);
            rend.material.color = Color.white;
            health -= PBPlayerController.Damage;
            if (health <= 0)
            {
                Dead();
            }
        }
        //if alien is hit by a missile
        else if (collision.CompareTag("Missile"))
        {
            Dead();
        }
        else if (type==Type.TYPE2 && collision.CompareTag("Player") 
            && !collision.gameObject.GetComponent<PBPlayerController>().dead)
        {
            Dead();
        }
        else if(type == Type.TYPE1 && collision.CompareTag("Alien1"))
        {
            flee(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (type == Type.TYPE1 && collision.CompareTag("Alien1"))
        {
            touching = false;
        }
    }

    public void Dead()
    {
        //make enemy explosion and then kill the particle effect after its duration
        GameObject bang = Instantiate(explosion, transform.position, transform.rotation);
        ParticleSystem parts = bang.GetComponent<ParticleSystem>();
        float totalDuration = parts.main.duration + parts.main.startLifetimeMultiplier;
        Destroy(bang, totalDuration);
        PBGameController.numAliensToKill--; //decrements the number of aliens left to kill
        GameManagerS.TotalAliensKilled++; //adds to the total number of aliens killed
        //gives more money to player if hard mode is enabled
        if (PlayerPrefs.GetInt("HardMode") == 0)
        {
            GameManagerS.Money += 10;
        }
        else
        {
            GameManagerS.Money += 15;
        }
        //if it was a type 2 alien that died and its fairly close to the player
        if (type == Type.TYPE2 && Vector3.Distance(transform.position, playerTransform.position) < 30f)
        {
            //do damage to player by exploding
            playerTransform.GetComponent<PBPlayerController>().DecrementHealth(alien2Dam);
        }
        //kill alien and remove it from the list of alive ones
        PBGameController.AliveAliens.Remove(gameObject);
        SoundController.GetSound(2).Play();
        Destroy(gameObject);
    }
}
