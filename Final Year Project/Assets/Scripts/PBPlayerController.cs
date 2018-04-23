using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PBPlayerController : MonoBehaviour
{
    //player stats
    public float speed;
    public float fireRate;
    private float nextFire;
    private float bulletCount;
    private float maxBullets;
    public static int Damage { get; set; }
    private float invulCount;
    public float Health;
    public float MaxHealth;
    //flags
    public static bool Invul { get; set; }
    private bool MissileMode;
    public bool dead { get; set; }
    //references
    Rigidbody2D rb;
    public PBBoundary boundary;
    public GameObject shot;
    public Transform shotSpawn;
    public TouchPad touchPad;
    public FireZoneScript FZ;
    private AudioSource sfx;
    public Slider healthBar;
    public GameObject zap; 
    public GameObject explosion;
    public Button restart; 
    public Slider bulletBar;
    public Slider invulBar;
    public GameObject missile;

    private void Start()
    {
        //set player stats
        Damage = 25 * GameManagerS.ShipWepLevel;
        MaxHealth = 500;
        Health = MaxHealth;
        healthBar.value = 1;
        maxBullets = 35;
        invulCount = 400;
        bulletCount = maxBullets;
        bulletBar.value = 1;
        invulBar.value = 1;
        //set references
        rb = GetComponent<Rigidbody2D>();
        sfx = GetComponent<AudioSource>();
        //start regenerating bullets
        StartCoroutine(RegenBullets());
        //set flags based on power-up states
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
    }

    void Update()
    {
        //if the fire area is touched, the time before the next shot has elapsed, the player has bullet regenerated and the game isn't paused
        if (FZ.canFire && Time.time > nextFire && bulletCount!=0 && !dead && !PBGameController.Paused)
        {
            //reset fire rate and shoot
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    //method being called every frame to regernate bullets
    //same as in player controller
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

    void FixedUpdate()
    {
        //get the direction of the touch as a vector
        Vector2 direction = touchPad.GetDirection();
        //take the y value of that direction
        Vector3 movement = new Vector3(0.0f, direction.y, 0.0f);
        //change the velocity by that movement by speed
        rb.velocity = movement * speed;
        //clamp position between y boundaries of the screen
        rb.position = new Vector3
        (
            -75f,
            Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax),
            0.0f
        );
    }
    //same as in player controller
    public void Shoot()
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

    public void DecrementHealth(int damage)
    {
        if (!dead)
        {
            if (Invul)
            {
                //defer damage to shield
                invulCount -= damage;
                invulBar.value = invulCount / 400;
                //destroy shield if health below or equal to 0
                if (invulCount <= 0)
                {
                    //turn off flags and destroy object
                    Invul = false;
                    invulBar.gameObject.SetActive(false);
                    GameManagerS.PowerUps[1] = false;
                    PlayerController.Invul = false;
                    invulBar.value = 1;
                    invulCount = 400;
                }
            }
            //dec health and update health bar
            else
            {
                Health -= damage;
                healthBar.value = Health / MaxHealth;
            }
        }
    }
    //called once collider is triggered
    //similar to player controller
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the player hits an alien bullet
        if (collision.gameObject.CompareTag("Alien Bullet"))
        {
            //dec health and destroy bullet
            DecrementHealth(PBAlienController.alien1Dam);
            Destroy(collision.gameObject);
            //make zap particle effect and attach it to ship
            GameObject part = Instantiate(zap, gameObject.transform.position, Quaternion.identity);
            (part).transform.parent = (gameObject).transform;
            part.transform.Rotate(0f, 0f, -90f);
            part.transform.localScale = new Vector3(10, 10, 10);
            StartCoroutine(wait(part, 0.5f));
        }
        //if alien hits a laser
        else if (collision.gameObject.CompareTag("Laser"))
        {
            DecrementHealth(PBAlienController.alien3Dam);
            Destroy(collision.gameObject);
            //make zap particle effect and attach it to ship
            GameObject part = Instantiate(zap, gameObject.transform.position, Quaternion.identity);
            (part).transform.parent = (gameObject).transform;
            part.transform.Rotate(0f, 0f, -90f);
            part.transform.localScale = new Vector3(10, 10, 10);
            StartCoroutine(wait(part, 0.5f));
        }
        
        //kills the ship if the amount of damage was enough to kill it
        if (Health <= 0)
        {
            Dead();
        }
        
    }

    public void Dead()
    {
        dead = true;
        //make explosion and then kill the particle effect after its duration
        GameObject bang = Instantiate(explosion, transform.position, transform.rotation);
        ParticleSystem parts = bang.GetComponent<ParticleSystem>();
        float totalDuration = parts.main.duration + parts.main.startLifetimeMultiplier;
        Destroy(bang, totalDuration);
        //when the ship dies, stop rendering it and show the restart button
        gameObject.GetComponent<Renderer>().enabled = false;
        restart.gameObject.SetActive(true);
        GameController.IsRunning = false;
        Health = MaxHealth;
        healthBar.value = 1;
        SoundController.GetSound(4).Play();
    }
    //destroys particle effect
    IEnumerator wait(GameObject zap, float secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(zap.gameObject);
    }
}
//class sets boundaries of y-axis for player movement
[System.Serializable]
public class PBBoundary
{
    public float yMin, yMax;
}