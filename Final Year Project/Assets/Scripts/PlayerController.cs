using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private AudioSource sfx;
    public Boundary boundary; //boundary of ship y-axis
    //player stats
    public float speed;
    public float Health;
    public float MaxHealth;
    private float nextFire;
    public float fireRate;
    public static int Damage { get; set; }
    //references 
    public Transform shotSpawn; 
    private SpriteRenderer sprite; 
    public Button restart;
    private Rigidbody2D rb;
    public MoveZoneScript MZ;
    public FireZoneScript FZ;
    public Slider healthBar;
    public Slider bulletBar;
    public Material reward;
    //particle effects
    public GameObject zap;
    public GameObject fire;
    public GameObject explosion;
    //projectiles
    public GameObject shot; 
    public GameObject missile;
    //flags for upgrade modes
    public static bool Invul { get; set; }
    public Slider invulBar;
    private float invulCount;
    public static bool MissileMode { get; set; }
    private float bulletCount;
    private float maxBullets;
    
    private void Start()
    {
        //if all objectives have been completed then set ship sprite to gold
        if (GameManagerS.AllObjCompleted)
        {
            GetComponent<SpriteRenderer>().material = reward;
        }
        //set damage according to ship weapon level
        Damage = 25 * GameManagerS.ShipWepLevel;
        //Initialises player stats
        MaxHealth = 500;
        invulCount = 400;
        maxBullets = 35;
        bulletCount = maxBullets;
        bulletBar.value = 1;
        Health = MaxHealth;
        healthBar.value = 1;
        invulBar.value = 1;
        //sets references
        sfx = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        //begin regeneration of bullets
        StartCoroutine(RegenBullets());
        //if power-ups are active on load, set flags
        if (GameManagerS.PowerUps[0])
        {
            MissileMode = true;
        }
        if (GameManagerS.PowerUps[1])
        {
            Invul = true;
        }
    }
    //regenerates the bullet count for weapon exhaustion (stamina) feature
    IEnumerator RegenBullets()
    {
        //while this script is running
        while (true)
        {
            //wait .3 seconds
            yield return new WaitForSeconds(0.3f);
            //if the number of bullets is less than max and player is not holding down fire area
            if (bulletCount < maxBullets && !FZ.canFire)
            {
                //add a bullet to the count
                bulletCount++;
                //update the stamina bar
                bulletBar.value = bulletCount / maxBullets;
            }

        }
    }
    //Update is called once per frame
    private void Update()
    {
        //shoots when the game is running, the fire area is being touched, the player has enough bullet regenerated
        //and the time till the next bullet can be fired, according to the fire rate, has elapsed
        if (GameController.IsRunning && FZ.canFire && Time.time > nextFire && bulletCount != 0)
        {
            //reset the time till the player can fire again and shoot
            nextFire = Time.time + fireRate;
            Shoot();
        }
        //if this flag becomes true, set the missile flag
        if (GameManagerS.PowerUps[0])
        {
            MissileMode = true;
        }
        //otherwise turn it off
        else
        {
            MissileMode = false;
        }
        //ditto for invulnerable flag
        if (GameManagerS.PowerUps[1])
        {
            Invul = true;
            invulBar.gameObject.SetActive(true);
        }
        else
        {
            Invul = false;
            invulBar.gameObject.SetActive(false);
        }
        //if the player's health reaches 0 or below, they die
        if (Health <= 0)
        {
            Dead();
        }
    }

    void FixedUpdate () {
        //if the game is running them perform movement
        if (GameController.IsRunning)
        {
            //scales the ship down slighly when game starts
            if (transform.localScale.y > 18f){
                transform.localScale -= new Vector3(0.4f, 0.5f, 0f);
            }
            //if the player is touching the movement area
            if (MZ.canMove)
            {
                //move ship upwards at set speed
                rb.velocity = Vector3.up * speed;
            }
            else
            {
                //move ship down slower to simulate gravity
                rb.velocity = Vector3.down * ((speed / 2) + 5);
            }
            //clamp the ship movement between the boundary valyes
            rb.transform.position = new Vector3(0.0f, Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax), 0.0f);
        }
        else
        {
            //the game has stopped running and moves to the centre of the screen
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, new Vector3(0.0f, 0.0f, 0.0f),speed*Time.deltaTime);
        }
    }

    public void DecrementHealth(int damage)
    { //decrease health by amount specified
        //if invulnerable, defer damage to shield
        if (Invul)
        {
            //dec shield health
            invulCount -= damage;
            //update shield bar
            invulBar.value = invulCount / 400;
            //if shield is destroyed, set flags to false and destroy shield bar object
            if(invulCount <= 0)
            {
                Invul = false;
                invulBar.gameObject.SetActive(false);
                GameManagerS.PowerUps[1] = false;
                //reset values for next time
                invulBar.value = 1;
                invulCount = 400;
            }
        }
        //if no shield is up
        else
        {
            //dec health and update health bar value
            Health -= damage;
            healthBar.value = Health / MaxHealth;
        }
    }
    //called once when trigger is collided with
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        //if a laser collides with a player
        if (collision.gameObject.CompareTag("Laser")  && GameController.IsRunning)
        {
            //do damage based on the amount of alien damage
            DecrementHealth(AlienController.Damage);
            //make zap particle effect, scale and rotate properly and attach it to ship
            GameObject part = Instantiate(fire, gameObject.transform.position, Quaternion.identity);
            (part).transform.parent = (gameObject).transform;
            part.transform.Rotate(0f, 0f, -90f);
            part.transform.localScale = new Vector3(10, 10, 10);
            //destroy particle effect after 2 seconds
            StartCoroutine(wait(part,2f));
        }
        //if player collides with wall, destroy ship straight away
        if (collision.gameObject.CompareTag("Wall") && GameController.IsRunning)
        {
            Dead();
        }
        //if player collides with alien bullet
        else if(collision.gameObject.CompareTag("Alien Bullet") && GameController.IsRunning)
        {
            //do damage based on the amount of alien damage
            DecrementHealth(AlienController.Damage);
            //destroy bullet object
            Destroy(collision.gameObject);
            //make zap particle effect, scale and rotate properly and attach it to ship
            GameObject part = Instantiate(zap, gameObject.transform.position, Quaternion.identity);
            (part).transform.parent = (gameObject).transform;
            part.transform.Rotate(0f, 0f, -90f);
            part.transform.localScale = new Vector3(10, 10, 10);
            //destroy particle after 2 seconds
            StartCoroutine(wait(part, 0.5f));
        }
    }
    //called every frame while collision is in trigger
    private void OnTriggerStay2D(Collider2D collision)
    { //if an enemy collides with the player, do damage while it touches the ship
        if (collision.gameObject.CompareTag("Alien1") && GameController.IsRunning)
        {
            //change the sprite colour to flash red to show visual feedback of taking damage
            sprite.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 1));
            //decrements the ship health by the amount of damage the alien does
            DecrementHealth(AlienController.Damage);
        }
        //while the laser is colliding with the player
        else if (collision.gameObject.CompareTag("Laser") && GameController.IsRunning)
        {
            //dec player health
            DecrementHealth(AlienController.Damage);
        }
    }
    //called once when trigger collision stops
    private void OnTriggerExit2D(Collider2D collision)
    { //sets the colour of the ship back to white once the collider is empty
        sprite.color = Color.white;
    }
    
    void Shoot()
    { 
        //make normal bullet if not in missle mode
        if (!MissileMode)
        {
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            sfx.Play(); //play sfx
            bulletCount--; //dec bullet count for weapon stamina
            //update stamina bar
            bulletBar.value = bulletCount / maxBullets;
        }
        //make missile
        else
        {
            Instantiate(missile, shotSpawn.position, missile.transform.rotation);
            SoundController.GetSound(3).Play(); //play sfx
        }
    }

    public void Dead()
    {
        //make explosion and then kill the particle effect after its duration
        GameObject bang = Instantiate(explosion, transform.position, transform.rotation);
        ParticleSystem parts = bang.GetComponent<ParticleSystem>();
        float totalDuration = parts.main.duration + parts.main.startLifetimeMultiplier;
        Destroy(bang, totalDuration);
        //when the ship dies, stop rendering it and show the restart button
        gameObject.GetComponent<Renderer>().enabled = false;
        restart.gameObject.SetActive(true);
        //set the game to not running
        GameController.IsRunning = false;
        //if the player died because too much damage, set number of health to 1 point
        if (Health <= 0)
        {
            Health = 1;
            healthBar.value = Health / MaxHealth;
        }
        //play death sound
        SoundController.GetSound(4).Play();
        //money goes down by 25 if they have more
        GameManagerS.Money -= 25;
        //if this makes the money go below 0, reset it
        if(GameManagerS.Money<0)
        {
            GameManagerS.Money = 0;
        }
        
    }
    //method used to destroy particle effect
    IEnumerator wait(GameObject zap, float secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(zap.gameObject);
    }

    //method called when player purchases health restoration
    public void RestoreHealth()
    {
        //if the player has damage and enough money to pay for it
        if (Health<MaxHealth && GameManagerS.Money > Mathf.Abs(Health - MaxHealth))
        {
            //dec money and reset health
            GameManagerS.Money -= (int)MaxHealth - (int)Health;
            Health = MaxHealth;
            healthBar.value = 1;  
        }
        //otherwise, if they have damage and not enough money, reset health and take all money
        else if(Health<MaxHealth)
        {
            GameManagerS.Money = 0;
            Health = MaxHealth;
            healthBar.value = 1;
        }
    }
}

//class used to store boundary values on the y-axis of the player
[System.Serializable]
public class Boundary
{
    public float yMin, yMax; //sets boundary values in the editor
}
