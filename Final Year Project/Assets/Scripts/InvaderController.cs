using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InvaderController : MonoBehaviour {
    public GameObject player;
    NavMeshAgent agent;
    Animator anim;
    public GameObject bullet;
    public Transform shotSpawn;
    private float nextFire;
    public float fireRate;
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Slider healthBar;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        Health = 100;
        //healthBar.value = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        //agent.SetDestination(player.transform.position);
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Fire();
        }
        anim.SetBool("Shooting", true);
    }

    private void Fire()
    {
        Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            //takes 3 shots to kill an alien
            Health -= 40;
            if(Health < 0)
            {
                Destroy(gameObject);
            }
            healthBar.value = Health / 100;
        }
    }
}
