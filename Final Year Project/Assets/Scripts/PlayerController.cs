using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Boundary boundary; //boundary of ship y-axis
    public float speed; //speed of the ship movement
    public static float health; //ship health
    public Transform shotSpawn; //where the bullet will spawn
    public float firerate; //how fast to shoot
    public GameObject shot; //reference to the bullet object
    private SpriteRenderer spriteRenderer; //reference to the ship sprite
    public Button restart; //reference to the restart button
    private Rigidbody2D rb;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //sets the starting health of the player
        health = 500;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //HACK: This needs to be changed when switching to mobile
        //shoots when the left mouse button is clicked and the game is running
        if (Input.GetMouseButtonDown(0) && GameController.IsRunning)
        {
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

            if (Input.GetKey(KeyCode.Space))
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
        health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { //if the wall collides with the player, kill player
        if (collision.gameObject.CompareTag("Wall") && GameController.IsRunning)
        {
            Dead();

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    { //if an enemy collides with the player, do damage while it touches the ship
        if (collision.gameObject.CompareTag("Alien1") && GameController.IsRunning)
        {
            //change the sprite colour to red to show visual feedback
            spriteRenderer.color = Color.red;

            //decrements the ship health by the amount of damage the alien does
            DecrementHealth(collision.gameObject.GetComponent<AlienController>().damage);

            //kills the ship if the amount of damage was enough to kill it
            if (health <= 0)
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
    { //when the ship dies, stop rendering it and show the restart button
        gameObject.GetComponent<Renderer>().enabled = false;
        restart.gameObject.SetActive(true);
        GameController.IsRunning = false;
    }
}

[System.Serializable]
public class Boundary
{
    public float yMin, yMax; //sets boundary values in the editor
}
