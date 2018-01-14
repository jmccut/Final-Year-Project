using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class CharController : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public Transform shotSpawn;
    public GameObject bullet;
    Animator anim;
    public static float Health { get; set; }
    public float MaxHealth { get; set; }
    public Slider healthBar;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        MaxHealth = 100f;
        Health = MaxHealth;
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(CrossPlatformInputManager.GetButtonUp("Jump"))
        {
            Fire();
        }
    }

    private void FixedUpdate()
    {
        //gets input from joysticks to get direction of player
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal") * speed;
        float vertical = CrossPlatformInputManager.GetAxis("Vertical") * speed;
        Vector3 direction = new Vector3(vertical, 0f, -horizontal);
        //changes velocity and look rotation according to the direction
        rb.velocity = direction;
        if (vertical != 0 || horizontal != 0)
        {   
            transform.rotation = Quaternion.LookRotation(direction);
            anim.SetFloat("Walk", 0.3f);
        }
        else
        {
            anim.SetFloat("Walk", 0f);
            
        }
        
    }

    private void Fire()
    {
        Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Alien Bullet"))
        {
            Health -= 10f;
            healthBar.value -= 0.1f;
            if (Health <= 0f)
            {
                Destroy(gameObject);
            }
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            Health--;
            healthBar.value -= 0.01f;
            if (Health <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
