using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InvaderController : MonoBehaviour {
    public GameObject player;
    NavMeshAgent agent;
    Animator anim;
    public GameObject bullet;
    public Transform shotSpawn;
    private float nextFire;
    public float fireRate;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
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
}
