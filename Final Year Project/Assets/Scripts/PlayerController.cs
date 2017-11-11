using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Boundary boundary; //boundary of ship y-axis
    public float speed; //speed of the ship movement
    public static float health; //ship health
    public Transform shotSpawn; //where the bullet will spawn
    public float firerate; //how fast to shoot
    public GameObject shot; //reference to the bullet object
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        health = 500; //sets the starting health
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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (GameController.IsRunning)
        {
            if (transform.localScale.y > 18f){
                transform.localScale -= new Vector3(0.4f, 0.5f, 0f);
            }
            /* HACK: This needs to be used when game goes mobile
            if (Input.touchCount > 0)
            {
                //finger on screen
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    rb.AddForce(Vector3.up * 1, ForceMode2D.Force);
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Debug.Log("touch moved");
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    Debug.Log("touch Ended");
                }

            }
            */

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

    private void OnDestroy()
    {
        //will need this eventually to say the player is dead
        
    }

    public void DecrementHealth(int damage)
    {
        health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the wall collides with the player, kill it
        if (collision.gameObject.CompareTag("Wall") && GameController.IsRunning)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if an enemy collides with the player, do damage while it touches the ship
        if (collision.gameObject.CompareTag("Alien1") && GameController.IsRunning)
        {
            spriteRenderer.color = Color.red;
            //decrements the ship health by the amount of damage the alien does
            DecrementHealth(collision.gameObject.GetComponent<AlienController>().damage);
            //kills the ship if the amount of damage was enough to kill it
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.color = Color.white;
    }
    
    void Shoot()
    {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
    }
}

[System.Serializable]
public class Boundary
{
    public float yMin, yMax; //sets boundary values in the editor
}
