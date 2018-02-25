using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PBPlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public PBBoundary boundary;
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public TouchPad touchPad;
    public FireZoneScript FZ;
    private float nextFire;
    private AudioSource sfx;
    public Slider healthBar;
    public float Health; //ship health
    public float MaxHealth;
    public GameObject zap; //damage particle effect
    public GameObject explosion;
    public Button restart; //reference to the restart button
    public static int Damage { get; set; }
    private float bulletCount;
    private float maxBullets;
    public Slider bulletBar;
    public bool dead { get; set; }
    public GameObject missile;
    private bool MissileMode;
    private void Start()
    {
        Damage = 25; //* GameManagerS.ShipWepLevel;
        MaxHealth = 500;
        Health = MaxHealth;
        healthBar.value = 1;
        maxBullets = 35;
        bulletCount = maxBullets;
        bulletBar.value = 1;
        rb = GetComponent<Rigidbody2D>();
        sfx = GetComponent<AudioSource>();
        StartCoroutine(RegenBullets());
        if (GameManagerS.PowerUps[0])
        {
            MissileMode = true;
        }
        else
        {
            MissileMode = false;
        }
    }

    [System.Serializable]
    public class PBBoundary
    {
        public float yMin, yMax;
    }

    void Update()
    {
        if (FZ.canFire && Time.time > nextFire && bulletCount!=0)
        {
            nextFire = Time.time + fireRate;
            Shoot();
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

    //Get the 'calibrated' value from the Input

    void FixedUpdate()
    {
        Vector2 direction = touchPad.GetDirection();
        
        Vector3 movement = new Vector3(0.0f, direction.y, 0.0f);
        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
            -75f,
            Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax),
            0.0f
        );
    }

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
    { //decrease health by amount specified if not dead
        if (!dead)
        {
            Health -= damage;
            healthBar.value = Health / MaxHealth;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Alien Bullet"))
        {
            DecrementHealth(PBAlienController.alien1Dam);
            Destroy(collision.gameObject);
            //make zap particle effect and attach it to ship
            GameObject part = Instantiate(zap, gameObject.transform.position, Quaternion.identity);
            (part).transform.parent = (gameObject).transform;
            part.transform.Rotate(0f, 0f, -90f);
            part.transform.localScale = new Vector3(10, 10, 10);
            StartCoroutine(wait(part, 0.5f));
        }
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

    IEnumerator wait(GameObject zap, float secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(zap.gameObject);
    }
}
