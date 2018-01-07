using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CharController : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public Transform shotSpawn;
    public GameObject bullet;
    Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
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
}
