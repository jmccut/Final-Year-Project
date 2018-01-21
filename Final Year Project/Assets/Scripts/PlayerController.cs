using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Boundary boundary; //boundary of ship y-axis
    public float speed; //speed of the ship movement
    public float Health; //ship health
    public float MaxHealth;
    public Transform shotSpawn; //where the bullet will spawn
    public GameObject shot; //reference to the bullet object
    private SpriteRenderer spriteRenderer; //reference to the ship sprite
    public Button restart; //reference to the restart button
    private Rigidbody2D rb;
    public MoveZoneScript MZ;
    public FireZoneScript FZ;
    private float nextFire;
    public float fireRate;
    public Slider healthBar;
    public GameObject zap;
    public GameObject explosion;

    private void Start()
    {
        //sets the starting health of the player
        MaxHealth = 500;
        Health = MaxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        healthBar.value = 1;
    }

    private void Update()
    {
        //shoots when the game is running and the fire area is being touched
        if (GameController.IsRunning && FZ.canFire && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
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
        Health -= damage;
        healthBar.value = Health/MaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { //if the wall collides with the player, kill player
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
            StartCoroutine(wait(part));
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    { //sets the colour of the ship back to white once the collider is empty
        spriteRenderer.color = Color.white;
    }
    
    void Shoot()
    { //makes shot
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
    }

    public void Dead()
    {
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        //when the ship dies, stop rendering it and show the restart button
        gameObject.GetComponent<Renderer>().enabled = false;
        restart.gameObject.SetActive(true);
        GameController.IsRunning = false;
        Health = MaxHealth;
        healthBar.value = 1;
    }

    IEnumerator wait(GameObject zap)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(zap.gameObject);
    }
}

[System.Serializable]
public class Boundary
{
    public float yMin, yMax; //sets boundary values in the editor
}
