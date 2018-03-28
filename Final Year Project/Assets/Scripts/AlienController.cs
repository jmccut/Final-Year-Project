using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
    private Transform player;
    private float speed;
    public static int Damage { get; set; }
    public GameObject explosion;
    public Transform shotSpawn;
    private float nextFire; //time until next shot permitted
    public float fireRate; //set in editor
    public GameObject shot;
    public GameObject partnerAlien;
    public enum Type { TYPE1, TYPE2, TYPE3}; //alien types
    public Type type;
    public int SequenceNumber; //set in editor
    public float health;
    Rigidbody2D rb;
    private bool chase;
    private AudioSource shootingSound;
    private SpriteRenderer rend;
    void Start () {
        //adds alien to alive list when spawned
        GameController.AliveAliens.Add(gameObject);
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        chase = true;
        //sets type of alien
        if(gameObject.CompareTag("Alien1"))
        {
            type = Type.TYPE1;
            //aliens have more damage and speed if hard mode is enabled
            if(PlayerPrefs.GetInt("HardMode") == 0)
            {
                Damage = 1;
                speed = 18 * GameManagerS.Level;
                health = 75;
            }
            else
            {
                Damage = 3;
                speed = 25 * GameManagerS.Level;
                health = 75;
            }
        }
        else if(gameObject.CompareTag("Alien2"))
        {
            if (PlayerPrefs.GetInt("HardMode") == 0)
            {
                type = Type.TYPE2;
                Damage = 50;
                speed = 400 * GameManagerS.Level;
                health = 150;
            }
            else
            {
                type = Type.TYPE2;
                Damage = 50;
                speed = 600 * GameManagerS.Level;
                health = 125;
            }
        }
        else if(gameObject.CompareTag("Alien3"))
        {
            if (PlayerPrefs.GetInt("HardMode") == 0)
            {
                type = Type.TYPE3;
                Damage = 1;
                speed = 15 * GameManagerS.Level;
                health = 250;
            }
            else
            {
                type = Type.TYPE3;
                Damage = 2;
                speed = 25 * GameManagerS.Level;
                health = 200;
            }
        }
        //gets player position
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        #pragma warning disable CS0168 
        catch (System.NullReferenceException e)
        {
        #pragma warning restore CS0168 
        }
        shootingSound = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    { //moves enemy towards the player transform if ship is not already dead and in chase mode
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (player != null && type == Type.TYPE1 && chase && GameController.IsRunning)
        {
            rb.velocity = new Vector3();
            rb.MovePosition(Vector3.MoveTowards(rb.position, player.position, Time.deltaTime * speed));
        }
        else if (type == Type.TYPE2)
        {
            //reach the left of the screen
            if (rb.position.x < -110f)
            {
                rb.velocity = -Vector3.left * speed * Time.deltaTime;
            }
            //once there, stop
            else
            {
                rb.velocity = new Vector3();
            }
        }
        else if (type == Type.TYPE3)
        {
            //aliens are in pairs deliniated by a sequence number
            //if they are the first of the pair
            if (SequenceNumber == 0)
            {
                //move to the right
                if (transform.position.x < -110)
                {
                    transform.position = Vector3.MoveTowards(rb.position, new Vector3(-110f, 5f, 0f), Time.deltaTime * speed / 2);
                    ExtendedLaser.Rotate = false;
                }
                else
                {
                    //move upwards
                    if (transform.position.y < 30f)
                    {
                        transform.position = Vector3.MoveTowards(rb.position, new Vector3(-110f, 30f, 0f), Time.deltaTime * speed / 2);
                        ExtendedLaser.Rotate = false;
                    }
                    else
                    {
                        //rotate laser and move to the right
                        Transform part = transform.GetChild(0);
                        Transform col = transform.GetChild(2);
                        col.transform.rotation = Quaternion.Lerp(col.transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, -90f)), Time.deltaTime * 0.25f);
                        col.localPosition = new Vector3();
                        ExtendedLaser.Rotate = true;
                        transform.position = Vector3.MoveTowards(rb.position, new Vector3(60f, 30f, 0f), Time.deltaTime * speed);
                        part.localPosition = new Vector3(0f, -0.45f, -0.11f);
                    }
                }
            }
            //if they are the second of the pair, do the same but move downwards
            else
            {
                if (transform.position.x < -110)
                {
                    transform.position = Vector3.MoveTowards(rb.position, new Vector3(-110f, -5f, 0f), Time.deltaTime * speed / 2);
                    ExtendedLaser.Rotate = false;
                }
                else
                {
                    if (transform.position.y > -30f)
                    {
                        transform.position = Vector3.MoveTowards(rb.position, new Vector3(-110f, -30f, 0f), Time.deltaTime * speed / 2);
                        ExtendedLaser.Rotate = false;
                    }
                    else
                    {
                        Transform part = transform.GetChild(0);
                        Transform col = transform.GetChild(2);
                        col.transform.rotation = Quaternion.Lerp(col.transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, 90f)), Time.deltaTime * 0.25f);
                        col.localPosition = new Vector3();
                        ExtendedLaser.Rotate = true;
                        transform.position = Vector3.MoveTowards(rb.position, new Vector3(60f, -30f, 0f), Time.deltaTime * speed);
                        part.localPosition = new Vector3(0f, 0.6f, -0.11f);
                    }
                }
            }
        }
        
    }

    private void Update()
    {
        //shoots when the game is running and the fire area is being touched
        if (Time.time > nextFire && type == Type.TYPE2)
        {
            //fires faster according to game level
            nextFire = Time.time + (fireRate - (0.35f * GameManagerS.Level));
            Shoot();
        }
        if(health <= 0)
        {
            Dead();
        }
        //if type 3 alien and one of the aliens dies, they both die
        if (partnerAlien == null && type == Type.TYPE3)
        {
            Dead();
        }
    }

    void Shoot()
    { //makes shot
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        shootingSound.Play();
    }
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        //if alien is hit by a bullet and isn't type 1
        if (collision.CompareTag("Bullet") && type != Type.TYPE1)
        {
            //destroy bullet and make alien flash red
            Destroy(collision.gameObject);
            rend.material.color = Color.white;
            yield return new WaitForSeconds(.1f);
            rend.material.color = Color.red;
            yield return new WaitForSeconds(.1f);
            rend.material.color = Color.white;
            health -= PlayerController.Damage;
            if (health <= 0)
            {
                Dead();
            }
        }
        //if alien is hit by a missile
        else if (collision.CompareTag("Missile"))
        {
            //if type 3, kill partner
            if (type == Type.TYPE3)
            {
                partnerAlien.GetComponent<AlienController>().Dead();
            }
            //kill alien
            Destroy(collision.gameObject);
            Dead();
        }
    }

    public void Dead()
    { 
        //make enemy explosion and then kill the particle effect after its duration
        GameObject bang = Instantiate(explosion, transform.position, transform.rotation);
        ParticleSystem parts = bang.GetComponent<ParticleSystem>();
        float totalDuration = parts.main.duration + parts.main.startLifetimeMultiplier;
        Destroy(bang, totalDuration);
        if(type == Type.TYPE2)
        {
            //spawn new enemy in its place
            Instantiate(gameObject, new Vector3(-125f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
        }
        GameController.numAliensToKill--; //decrements the number of aliens left to kill
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
        //kill alien and remove it from the list of alive ones
        GameController.AliveAliens.Remove(gameObject);
        SoundController.GetSound(2).Play(); //plays death sound
        Destroy(gameObject);
    }

    public void flee(GameObject target)
    {
        //only flee if not chasing
        if (!chase)
        {
            //calculate and set velocity to avoid gameobject
            Vector2 desiredVelocity = (rb.transform.position - target.transform.position);
            desiredVelocity.Normalize();
            desiredVelocity *= 25f;
            Vector2 direction = desiredVelocity - rb.velocity;
            direction = new Vector2(0f, direction.y);
            rb.velocity = direction;
        }
    }

    public IEnumerator recieveTriggerEnter(string name, Collider2D other)
    {
        //if method called from large collider
        if(name == "Large Collider")
        {
            //if bullet hits large collider, make alien flee it
            if (other.CompareTag("Bullet") && type == Type.TYPE1)
            {
                chase = false;
                flee(other.gameObject);
            }
        }
        //if small collider
        else
        {
            //run hit sequence
            if (other.CompareTag("Bullet"))
            {
                Destroy(other.gameObject);
                //flash red
                rend.material.color = Color.white;
                yield return new WaitForSeconds(.1f);
                rend.material.color = Color.red;
                yield return new WaitForSeconds(.1f);
                rend.material.color = Color.white;

                health -= PlayerController.Damage;
                if (health <= 0)
                {
                    Dead();
                }
                if (player != null)
                {
                    chase = true;
                }
            }
            else if (other.CompareTag("Missile"))
            {
                Dead();
            }
        }
    }

    public void recieveTriggerExit(string name, Collider2D other)
    {
        //if bullet exits large collider, set alien to chase again
        if(name == "Large Collider")
        {
            if (other.CompareTag("Bullet") && type == Type.TYPE1)
            {
                if (player != null)
                {
                    chase = true;
                }
            }
        }
    }
}
