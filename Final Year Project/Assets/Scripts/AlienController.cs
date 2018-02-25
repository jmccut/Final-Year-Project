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
    public GameObject partnerAlien;
    public enum Type { TYPE1, TYPE2, TYPE3};
    public int SequenceNumber; //set in editor
    public Type type;
    public float health;
    Rigidbody2D rb;
    private bool chase;
    private AudioSource shootingSound;
    private SpriteRenderer rend;
    void Start () {
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
                health = 125;
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
                health = 75;
            }
            else
            {
                type = Type.TYPE3;
                Damage = 2;
                speed = 25 * GameManagerS.Level;
                health = 100;
            }
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
        shootingSound = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    { //moves enemy towards the player transform if ship is not already dead
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (playerTransform != null && type == Type.TYPE1 && chase && GameController.IsRunning)
        {
            rb.velocity = new Vector3();
            rb.MovePosition(Vector3.MoveTowards(rb.position, playerTransform.position, Time.deltaTime * speed));
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
                if (transform.position.x < -110)
                {
                    transform.position = Vector3.MoveTowards(rb.position, new Vector3(-110f, 5f, 0f), Time.deltaTime * speed / 2);
                    ExtendedLaser.Rotate = false;
                }
                else
                {


                    if (transform.position.y < 30f)
                    {
                        transform.position = Vector3.MoveTowards(rb.position, new Vector3(-110f, 30f, 0f), Time.deltaTime * speed / 2);
                        ExtendedLaser.Rotate = false;
                    }
                    else
                    {
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
            //if they are the second of the pair
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
            nextFire = Time.time + (fireRate - (0.35f * GameManagerS.Level));
            Shoot();
        }
        if(health <= 0)
        {
            Dead();
        }
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
        //if alien is hit by a bullet
        if (collision.CompareTag("Bullet") && type != Type.TYPE1)
        {
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
            if (type == Type.TYPE3)
            {
                partnerAlien.GetComponent<AlienController>().Dead();
            }
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
        SoundController.GetSound(2).Play();
        Destroy(gameObject);
    }

    //avoids gameobject called with
    public void flee(GameObject target)
    {
        if (!chase)
        {
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
        if(name == "Large Collider")
        {
            if (other.CompareTag("Bullet") && type == Type.TYPE1)
            {
                chase = false;
                flee(other.gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Bullet"))
            {
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
                if (playerTransform != null)
                {
                    chase = true;
                }
                //destroys bullet
                if(other!= null)
                {
                    Destroy(other.gameObject);
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
        if(name == "Large Collider")
        {
            if (other.CompareTag("Bullet") && type == Type.TYPE1)
            {
                if (playerTransform != null)
                {
                    chase = true;
                }
            }
        }
    }
}
