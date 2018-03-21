using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private AudioSource sfx;
    public Boundary boundary; //boundary of ship y-axis
    public float speed; //speed of the ship movement
    public float Health; //ship health
    public float MaxHealth;
    public Transform shotSpawn; //where the bullet will spawn
    public GameObject shot; //reference to the bullet object
    public GameObject missile;
    private SpriteRenderer spriteRenderer; //reference to the ship sprite
    public Button restart; //reference to the restart button
    private Rigidbody2D rb;
    public MoveZoneScript MZ;
    public FireZoneScript FZ;
    private float nextFire;
    public float fireRate;
    public Slider healthBar;
    public GameObject zap; //damage particle effect
    public GameObject fire;
    public GameObject explosion;

    public static int Damage { get; set; }
    //flags for upgrade modes
    public static bool Invul { get; set; }
    public Slider invulBar;
    private float invulCount;
    public static bool MissileMode { get; set; }
    private float bulletCount;
    private float maxBullets;
    public Slider bulletBar;
    public Material reward;
    private void Start()
    {
        //if all objectives have been completed then set material
        if (GameManagerS.AllObjCompleted)
        {
            GetComponent<SpriteRenderer>().material = reward;
        }
        //set damage according to game manager state
        Damage = 25 * GameManagerS.ShipWepLevel;
        //sets the starting health of the player
        MaxHealth = 500;
        invulCount = 400;
        maxBullets = 35;
        bulletCount = maxBullets;
        bulletBar.value = 1;
        Health = MaxHealth;
        sfx = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        healthBar.value = 1;
        invulBar.value = 1;
        StartCoroutine(RegenBullets());
        if (GameManagerS.PowerUps[0])
        {
            MissileMode = true;
        }
        if (GameManagerS.PowerUps[1])
        {
            Invul = true;
        }
    }
    IEnumerator RegenBullets()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (bulletCount < maxBullets && !FZ.canFire)
            {
                bulletCount++;
                bulletBar.value = bulletCount / maxBullets;
            }

        }
    }
    private void Update()
    {
        //shoots when the game is running and the fire area is being touched
        if (GameController.IsRunning && FZ.canFire && Time.time > nextFire && bulletCount != 0)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
        if (GameManagerS.PowerUps[0])
        {
            MissileMode = true;
        }
        else
        {
            MissileMode = false;
        }
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
                //move ship up at the full movement speed
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
        if (Invul)
        {
            invulCount -= damage;
            invulBar.value = invulCount / 400;
            if(invulCount <= 0)
            {
                Invul = false;
                invulBar.gameObject.SetActive(false);
                GameManagerS.PowerUps[1] = false;
                invulBar.value = 1;
                invulCount = 400;
            }
        }
        else
        {
            Health -= damage;
            healthBar.value = Health / MaxHealth;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { //if the wall collides with the player, kill player
        if (collision.gameObject.CompareTag("Laser")  && GameController.IsRunning)
        {
            DecrementHealth(AlienController.Damage);
            //make zap particle effect and attach it to ship
            GameObject part = Instantiate(fire, gameObject.transform.position, Quaternion.identity);
            (part).transform.parent = (gameObject).transform;
            part.transform.Rotate(0f, 0f, -90f);
            part.transform.localScale = new Vector3(10, 10, 10);
            StartCoroutine(wait(part,2f));
        }
        if (collision.gameObject.CompareTag("Wall") && GameController.IsRunning)
        {
            Dead();
        }
        else if(collision.gameObject.CompareTag("Alien Bullet") && GameController.IsRunning)
        {
            Destroy(collision.gameObject);
            //make zap particle effect and attach it to ship
            GameObject part = Instantiate(zap, gameObject.transform.position, Quaternion.identity);
            (part).transform.parent = (gameObject).transform;
            part.transform.Rotate(0f, 0f, -90f);
            part.transform.localScale = new Vector3(10, 10, 10);
            StartCoroutine(wait(part, 0.5f));
            DecrementHealth(AlienController.Damage);
            //kills the ship if the amount of damage was enough to kill it
            if (Health <= 0)
            {
                Dead();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    { //if an enemy collides with the player, do damage while it touches the ship
        if (collision.gameObject.CompareTag("Alien1") && GameController.IsRunning)
        {
            //change the sprite colour to flash red to show visual feedback of taking damage
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 1));
            //decrements the ship health by the amount of damage the alien does
            DecrementHealth(AlienController.Damage);

            //kills the ship if the amount of damage was enough to kill it
            if (Health <= 0)
            {
                Dead();
            }
        }
        else if (collision.gameObject.CompareTag("Laser") && GameController.IsRunning)
        {
            DecrementHealth(AlienController.Damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { //sets the colour of the ship back to white once the collider is empty
        spriteRenderer.color = Color.white;
    }
    
    void Shoot()
    { 
        //make normal bullet if not in missle mode
        if (!MissileMode)
        {
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            sfx.Play();
            bulletCount--;
            bulletBar.value = bulletCount / maxBullets;
        }
        //make missile
        else
        {
            Instantiate(missile, shotSpawn.position, missile.transform.rotation);
            SoundController.GetSound(3).Play();
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
        GameController.IsRunning = false;
        if (Health == 0)
        {
            Health = 1;
            healthBar.value = Health / MaxHealth;
        }
        SoundController.GetSound(4).Play();
        //money goes down
        if (GameManagerS.Money > 25)
        {
            GameManagerS.Money -= 25;
        }
        else
        {
            GameManagerS.Money = 0;
        }
        
    }

    IEnumerator wait(GameObject zap, float secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(zap.gameObject);
    }

    public void RestoreHealth()
    {
        if (Health<MaxHealth && GameManagerS.Money > Mathf.Abs(Health - MaxHealth))
        {
            GameManagerS.Money -= (int)MaxHealth - (int)Health;
            Health = MaxHealth;
            healthBar.value = 1;  
        }
        else if(Health<MaxHealth)
        {
            GameManagerS.Money = 0;
            Health = MaxHealth;
            healthBar.value = 1;
        }
    }
}

[System.Serializable]
public class Boundary
{
    public float yMin, yMax; //sets boundary values in the editor
}
