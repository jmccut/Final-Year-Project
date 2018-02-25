using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControl : MonoBehaviour {
    public float speed; //holds speed of the bullet
    Rigidbody rb;
    //private Vector3 velocity;
    //private Vector3 oldPosition;
    private Vector3 direction;
    private float min;
    private GameObject hitObject;
    public GameObject player;
    private bool isPlayerBullet;
    private bool isAlienBullet;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        //set flags so that compare tag does not have to be used every physics update
        if (gameObject.CompareTag("Bullet"))
        {
            isPlayerBullet = true;
        }
        else if (gameObject.CompareTag("Alien Bullet") )
        {
            isAlienBullet = true;
        }
    }

    private void FixedUpdate()
    {
        //if this is a player bullet and the game is on easy, bullets will be assisted towards aliens
        if (isPlayerBullet && PlayerPrefs.GetInt("HardMode") == 0 && player != null)
        {
            //find the alien closest to the player's forward transform vector
            min = 0;
            hitObject = null;
            foreach (GameObject i in InsideController.AliveInvaders)
            {
                //if this is the first alien, set the min to the angle and set it as the target
                if (min == 0 && hitObject == null && i.activeSelf)
                {
                    min = Vector3.Angle(player.transform.forward, i.transform.position);
                    hitObject = i;
                }
                //otherwise judge if the angle between the alien and the player transform is smaller than the previous
                if (Vector3.Angle(player.transform.forward, i.transform.position) < min && i.activeSelf)
                {
                    //if so, overwrite min and the target
                    min = Vector3.Angle(player.transform.forward, i.transform.position);
                    hitObject = i;
                }
            }
            //if the distance from the player's forward vector and the alien is less than 15
            if (min < 15f)
            {
                //search for the alien (has to be done in case the alien dies)
                foreach (GameObject i in InsideController.AliveInvaders)
                {
                    //if it matches the closest
                    if (i == hitObject)
                    {
                        //make the laser go towards the alien
                        transform.position = Vector3.MoveTowards(transform.position, 
                            new Vector3(i.transform.position.x, transform.position.y, i.transform.position.z), 
                            speed * Time.deltaTime);
                    }
                }
            }
            //otherwise if the alien is past the minimum then just shoot straight
            else
            {
                rb.velocity = transform.forward * speed;
            }
        }
        else 
        {
            //if this is an alien bullet and the game is in hard mode, alien bullets will be assisted towards the player
            if (isAlienBullet && PlayerPrefs.GetInt("HardMode") == 1 && player != null) {
                //find the angle between the bullet and the player
                min = Vector3.Angle(transform.forward, player.transform.position);
                hitObject = player;
                //if the angle is less than 15, move towards the player
                if (min < 20f)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(hitObject.transform.position.x, transform.position.y,
                        hitObject.transform.position.z), speed * Time.deltaTime);
                }
                else
                {
                    rb.velocity = transform.forward * speed;
                }
            }
            //else shoot the bullet straight
            else
            {
                rb.velocity = transform.forward * speed;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if the bullet didn't hit another bullet
        if (!collision.gameObject.CompareTag("Bullet") || !collision.gameObject.CompareTag("Alien Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
