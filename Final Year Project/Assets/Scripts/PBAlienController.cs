using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBAlienController : MonoBehaviour {
    //stats
    public static int alien1Dam;
    public static int alien2Dam;
    public static int alien3Dam;
    private float speed;
    private float nextFire;
    public float fireRate;
    public float health;
    //references
    public Transform shotSpawn;
    private Transform playerTransform; //reference to the player transform
    public GameObject explosion;
    public GameObject shot;
    Rigidbody2D rb;
    private AudioSource shootingSound;
    SpriteRenderer rend;
    //types of alien
    public enum Type { TYPE1, TYPE2, TYPE3 };
    public Type type;
    //flags
    private bool touching;
    private bool touchingBoss;

    void Start () {
        //set stats and references
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        shootingSound = GetComponent<AudioSource>();
        alien1Dam = 25;
        alien2Dam = 150;
        alien3Dam = 15;
        //add alien to alive list in game controller
        PBGameController.AliveAliens.Add(gameObject);
     
        //sets type of alien and stats
        //IMPORTANT: in this implementation, type 1 in the medium alien ships and type 2 are the smaller bomber aliens
        //-contrary to the final report
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
            fireRate = 6;
        }
        //gets player position
        try
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch (System.NullReferenceException)
        {
        }
    }
	
	// Update is called once per frame
	void Update () {
        //shoots if enemy is type 1 or 3 (only types that shoot) and time till next fire is elapsed
        if (Time.time > nextFire && (type == Type.TYPE1 || type == Type.TYPE3))
        {
            nextFire = Time.time + (fireRate);
            Shoot();
        }

        if (health <= 0)
        {
            Dead();
        }
    }

    //similar to that described in alien controller
    void flee(Collider2D collision)
    {
        //if the alien is touching another
        touching = true;
        //find vector between current position and collision object
        Vector2 desiredVelocity = (rb.transform.position - collision.gameObject.transform.position);
        //find direction and scale it
        desiredVelocity.Normalize();
        desiredVelocity *= 25f;
        //get movement vector and take the y value
        Vector2 direction = desiredVelocity - rb.velocity;
        direction = new Vector2(0f, direction.y);
        //if the movement is within the screen boundaries
        if (rb.position.y < 40 && rb.position.y > -40)
        {
            rb.velocity = direction; //apply it
        }
    }
    void FixedUpdate()
    { //if player not dead and alien type 1
        if (playerTransform != null && type == Type.TYPE1)
        {
            //if not touching another alien
            if (!touching)
            {
                //move towards player y-axis position
                rb.velocity = new Vector3();
                rb.MovePosition(Vector3.MoveTowards(rb.position,
                    new Vector3(0f, playerTransform.position.y, 0f),
                    Time.deltaTime * speed));
            }
        }
        //if player not dead and type 2
        else if (playerTransform != null && type == Type.TYPE2)
        {
            //move towards player position
            rb.velocity = new Vector3();
            rb.MovePosition(Vector3.MoveTowards(rb.position,
                new Vector3(playerTransform.position.x + 10, playerTransform.position.y, playerTransform.position.z),
                Time.deltaTime * speed));
        }
        //if player not dead, alien is type 3 and not touching
        else if (playerTransform != null && type == Type.TYPE3 && !touchingBoss)
        {
            //move into game scene view
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
        //only plays sfx for type 1
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
            //run damage sequence
            Destroy(collision.gameObject);
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
            if (type != Type.TYPE3)
            {
                Dead();
            }
            else
            {
                health -= PBPlayerController.Damage*2;
                if(health<= 0)
                {
                    Dead();
                }
            }
        }
        //if bomber alien hits player, explode and die
        else if (type==Type.TYPE2 && collision.CompareTag("Player") 
            && !collision.gameObject.GetComponent<PBPlayerController>().dead)
        {
            Dead();
        }
        //if alien touches another of the same type, flee it to avoid overlapping
        else if(type == Type.TYPE1 && collision.CompareTag("Alien1"))
        {
            flee(collision);
        }
        //same for type 3 alien
        else if (type == Type.TYPE3 && collision.CompareTag("Alien3"))
        {
            flee(collision);
            touchingBoss = true;
        }
    }
    //on collider exit
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if this was same alien types, turn off flags

        if (type == Type.TYPE1 && collision.CompareTag("Alien1"))
        {
            touching = false;
        }
        if (type == Type.TYPE3 && collision.CompareTag("Alien3"))
        {
            touchingBoss = false;
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
        Destroy(gameObject);
        SoundController.GetSound(2).Play(); //play sfx
    }
}
