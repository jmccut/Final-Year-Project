using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
    //stats
    private float nextFire;
    public float fireRate; //set in editor
    private float speed;
    public static int Damage { get; set; }
    public int SequenceNumber; //set in editor
    public float health;
    //references
    public GameObject explosion;
    public GameObject shot;
    public GameObject partnerAlien;
    public Transform shotSpawn;
    private Transform player;
    private AudioSource shootingSound;
    private SpriteRenderer rend;
    Rigidbody2D rb;
    //type of alien
    public enum Type { TYPE1, TYPE2, TYPE3}; //alien types
    public Type type;
    //flags
    private bool chase;

    void Start () {
        //adds alien to alive list when spawned
        GameController.AliveAliens.Add(gameObject);

        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        shootingSound = GetComponent<AudioSource>();

        //sets type of alien
        if (gameObject.CompareTag("Alien1"))
        {
            chase = true; //alien begins in chase state
            type = Type.TYPE1;

            //aliens stats change if game is on hard compared to normal mode

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
    }

    void FixedUpdate()
    { //if alien type 1 and chase mode set
        if (player != null && type == Type.TYPE1 && chase && GameController.IsRunning)
        {
            //move alien towards player position
            rb.velocity = new Vector3();
            rb.MovePosition(Vector3.MoveTowards(rb.position, player.position, Time.deltaTime * speed));
        }
        else if (type == Type.TYPE2)
        {
            //if not in the game scene yet
            if (rb.position.x < -110f)
            {
                //move to the right
                rb.velocity = -Vector3.left * speed * Time.deltaTime;
            }
            //once in the scene view, stay static
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
                //move to the right if not at position
                if (transform.position.x < -110)
                {
                    transform.position = Vector3.MoveTowards(rb.position, new Vector3(-110f, 5f, 0f), Time.deltaTime * speed / 2);
                    ExtendedLaser.Rotate = false;
                }
                else
                {
                    //move upwards if not at position
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
        //if type 2 and the time till the next shot, set by the fire rate, has elapsed
        if (Time.time > nextFire && type == Type.TYPE2)
        {
            //resets the time till next fire (shorter time with each game level)
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
    { //creates bullet object
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        shootingSound.Play();
    }
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        //if alien is hit by a bullet and isn't type 1
        //type 1 aliens use different trigger colliders
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

        GameController.numAliensToKill--; 
        GameManagerS.TotalAliensKilled++;

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
        Destroy(gameObject);
        SoundController.GetSound(2).Play(); //plays death sfx
    }

    public void flee(GameObject target)
    {
        //only flee if not chasing
        if (!chase)
        {
            //calculate vector between the object and current position
            Vector2 desiredVelocity = (rb.transform.position - target.transform.position);
            //get the direction of that vector
            desiredVelocity.Normalize();
            //scale it to desired length of movement
            desiredVelocity *= 25f;
            //get vector needed to apply that direction
            Vector2 direction = desiredVelocity - rb.velocity;
            //apply it in the y-axis
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
