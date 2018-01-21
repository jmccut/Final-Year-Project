﻿using System.Collections;
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
    public float MaxHealth { get; set; }
    public Slider healthBar;
    public Slider invulBar;
    public static bool Dead { get; set; }
    public static bool Invul { get; set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        invulBar.value = 1;
        StartCoroutine(StartInvul());
        Dead = false;
        MaxHealth = 100f;
        if (GameManagerS.Health <= 0)
        {
            GameManagerS.Health = MaxHealth;
        }
        anim = GetComponent<Animator>();
        healthBar.value = GameManagerS.Health / MaxHealth;
    }
    void Update()
    {
        //if the player hits the fire button, fire bullet
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

    public void IncrementHealth(int bonus)
    { //decrease health by amount specified
        GameManagerS.Health += bonus;
        if(GameManagerS.Health > MaxHealth)
        {
            GameManagerS.Health = MaxHealth;
        }
        healthBar.value = GameManagerS.Health / MaxHealth;
    }

    public void DecrementHealth(int damage)
    { //decrease health by amount specified
        GameManagerS.Health -= damage;
        if (GameManagerS.Health <= 0)
        {
            Dead = true;
            Destroy(gameObject);
        }
        healthBar.value = GameManagerS.Health / MaxHealth;
    }

    private void Fire()
    {
        if (!Invul)
        {
            //make bullet
            Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the player is hit by an alien bullet
        if (other.gameObject.CompareTag("Alien Bullet"))
        {
            //dec health and health bar, check if dead
            DecrementHealth(10);
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        //if the player touches the boss, lose health while touching
        if (collision.gameObject.CompareTag("Boss") && !Invul)
        {
            DecrementHealth(1);
        }
    }

    public IEnumerator StartInvul()
    {
        //player is invulnerable until invul bar is empty
        Invul = true;
        while (invulBar.value >= 0)
        {
            //decrement bar value by 0.01 every 0.05 seconds
            invulBar.value -= 0.01f;
            yield return new WaitForSeconds(0.05f);
            //once bar is empty, player is no longer invulnerable
            if (invulBar.value <= 0)
            {
                Invul = false;
                invulBar.gameObject.SetActive(false);
                break;
            }
        }
        
    }
}
