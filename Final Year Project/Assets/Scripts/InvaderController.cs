using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InvaderController : MonoBehaviour {
    //alien states
    public enum State { CHASE, WANDER }; 
    public State state; 
    //stats
    public float nextWander; //time until next wander position
    private float timer; //timer used to check if it is time to find next wander pos
    private float nextFire;
    public float fireRate;
    public int Health { get; set; }
    private Vector3 destination; //holds the destination of the wander
    public float pointRadius; //radius in which enemy can find a point to travel to
    //references
    Animator anim;
    private Vector3 startPos;
    public GameObject bullet;
    public Transform shotSpawn;
    public Transform target;
    NavMeshAgent agent;
    public GameObject key;
    private AudioSource shootingSound;

    void Start () {
        //add alien to alive list
        InsideController.AliveInvaders.Add(gameObject);
        //set references
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        shootingSound = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //set health for boss
        if (gameObject.CompareTag("Boss")){
            //more health on hard mode
            if (PlayerPrefs.GetInt("HardMode") == 0)
            {
                Health = 250;
            }
            else
            {
                Health = 400;
            }
        }
        //normal alien
        else
        {
            //aliens have more health in hard mode
            if (PlayerPrefs.GetInt("HardMode") == 0)
            {
                Health = 100;
            }
            else
            {
                Health = 150;
            }
        }
        //random pause between next wander directions
        nextWander = Random.Range(2f, 6f);
        timer = nextWander;
        state = State.WANDER; //enemies start in wander state

        //get random point in a 25 point radius
        Vector3 randomDirection = Random.insideUnitSphere * 25f;
        //add it to the default position
        randomDirection += transform.position;
        //find where on the navmesh it is closest to
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 25f, 1);
        //set position to this position
        transform.position = hit.position;
        startPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime; //adds time since last update to timer to keep track of when next wander is
        //if in chase state
        if (state.Equals(State.CHASE) && target != null)
        {
            //enemy follows player
            agent.SetDestination(target.position);
            //if the enemy is within a distance of the player, they can see them and this is an invader enemy; shoot them
            if (Vector3.Distance(transform.position, target.position) < 4f && 
                Physics.Linecast(shotSpawn.transform.forward, target.transform.position) && 
                gameObject.CompareTag("Invader"))
            {
                //if enough time has passed since last fire
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
                //this is because the animation looks like a melee attack when there is no gun
                if (gameObject.CompareTag("Boss") && Vector3.Distance(transform.position, target.position) < 1.5f)
                {
                    anim.SetBool("Shooting", true);
                }
                else
                {
                    anim.SetBool("Shooting", false);
                }
            }
            //do not slow down near player when chasing
            agent.autoBraking = false;
        }
        //if the enemy is wandering and the time before the next wander has elapsed
        else if (state.Equals(State.WANDER) && nextWander <= timer)
        {
            //wander and reset timer
            Wander();
            timer = 0f;
            //random time till next wander
            nextWander = Random.Range(2f, 6f);
            agent.autoBraking = true;
        }
        //if player dies, go back to wandering
        else if(target == null)
        {
            anim.SetBool("Shooting", false);
            state = State.WANDER;
        }
    }

    private void Fire()
    {
        //make bullet
        shootingSound.Play();
        Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if hit by bullet
        if (collision.transform.gameObject.CompareTag("Bullet"))
        {
            //takes 3 shots to kill an alien
            Health -= CharController.Damage;
            if (Health < 0)
            {
                Dead();
            }
            //chase
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
            //if in boss level, chase if near even if not in line of sight
            else if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                state = State.CHASE;
            }
        }
    }

    void Dead()
    {
        //increment kill count, if all enemies are dead and not boss level
        InsideController.KilledCount++;
        GameManagerS.TotalAliensKilled++; //adds to the total number of aliens killed
        //if this was the last alien to die
        if (InsideController.KilledCount == InsideController.numberOfEnemies
            && SceneManager.GetActiveScene().buildIndex != 4)
        {
            //spawn door key
            Instantiate(key, gameObject.transform.position, Quaternion.identity);
        }
        //gives more money to player when hard mode enabled
        if (PlayerPrefs.GetInt("HardMode") == 0)
        {
            GameManagerS.Money += 10;
        }
        else
        {
            GameManagerS.Money += 15;
        }
        //play death sound and remove from alive list
        SoundController.GetSound(0).Play();
        InsideController.AliveInvaders.Remove(gameObject);
        Destroy(gameObject);
    }
}
