using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InvaderController : MonoBehaviour {
    public Transform target;
    NavMeshAgent agent;
    public enum State { CHASE, WANDER }; //holds enemy states
    public State state; //current enemy state
    public float nextWander; //time until next wander position
    private float timer; //timer used to check if it is time to find next wander pos
    private Vector3 destination; //holds the destination of the wander
    public float pointRadius; //radius in which enemy can find a point to travel to
    Animator anim;
    private Vector3 startPos;
    public GameObject bullet;
    public Transform shotSpawn;
    private float nextFire;
    public float fireRate;
    public int Health { get; set; }

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (gameObject.CompareTag("Boss")){
            Health = 200;
        }
        else
        {
            Health = 100;
        }


        //random pause between next wander directions
        nextWander = Random.Range(2f, 6f);
        timer = nextWander;
        state = State.WANDER; //enemies start in wander state

        //sets a random start position on the nav mesh
        Vector3 randomDirection = Random.insideUnitSphere * 25f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 25f, 1);
        transform.position = hit.position;
        startPos = transform.position;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        //agent.SetDestination(player.transform.position);

        if (state.Equals(State.CHASE) && target != null)
        {

            //enemy follows player
            agent.SetDestination(target.position);
            //if the enemy is within a distance of the player, and they can see them and this is an invader enemy, shoot them
            if (Vector3.Distance(transform.position, target.position) < 4f && 
                Physics.Linecast(shotSpawn.transform.forward, target.transform.position) && 
                gameObject.CompareTag("Invader"))
            {
                if (Time.time > nextFire)
                {
                    anim.SetBool("Shooting", true);
                    nextFire = Time.time + fireRate;
                    Fire();
                }
            }
            else
            {
                //if this is the boss enemy and they are a certain distance away, do shoot animation but not shoot
                if (gameObject.CompareTag("Boss") && Vector3.Distance(transform.position, target.position) < 1.5f)
                {
                    anim.SetBool("Shooting", true);
                }
                else
                {
                    anim.SetBool("Shooting", false);
                }
            }
            agent.autoBraking = false;
        }
        //if the enemy is wandering and the time before the next wander has elapsed
        else if (state.Equals(State.WANDER) && nextWander <= timer)
        {
            Wander();
            timer = 0f;
            nextWander = Random.Range(2f, 6f);
            agent.autoBraking = true;
        }
        else if(target == null)
        {
            anim.SetBool("Shooting", false);
            state = State.WANDER;
        }
    }

    private void Fire()
    {
        //anim.SetBool("Shooting", true);
        Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.gameObject.CompareTag("Bullet"))
        {
            //takes 3 shots to kill an alien
            Health -= 40;
            if (Health < 0)
            {
                Destroy(gameObject);
                InsideController.KilledCount++;
            }
            state = State.CHASE;
        }
    }

    //wander to random points on the nav mesh
    void Wander()
    {
        //set slower speed for wandering
        agent.speed = 0.8f;

        //get random direction within sphere with a radius
        Vector3 randomPoint = Random.insideUnitSphere * pointRadius;

        //apply the direction to the starting point to give a position
        randomPoint += startPos;
        NavMeshHit hit;
        //find the closest point on the mesh for that position
        NavMesh.SamplePosition(randomPoint, out hit, pointRadius, 15);
        Vector3 finalPosition = hit.position;
        //normalised vector point from AI to target
        Vector3 direction = finalPosition - transform.position;
        //if the dot product is greater than 1, the position is directly ahead
        if (1f > Vector3.Dot(direction, transform.forward))
        {
            agent.SetDestination(finalPosition);
        }
        else
        {
            nextWander = 0f;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //if the player enters the collision radius and he isn't invulnerable, chase him
        if (other.transform.gameObject.CompareTag("Player") && !CharController.Invul)
        {
            //if the enemy is in the line of sight
            if (Physics.Linecast(transform.position, other.transform.position))
            {
                state = State.CHASE;
            }
            else if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                state = State.CHASE;
            }
        }
    }
}
