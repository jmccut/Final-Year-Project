﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour {
    Rigidbody2D rb;
    public int speed; //set in editor
    public Vector3 spawnPos;
    private ParticleSystem ps;
    public GameObject explosion;
    GameObject nearest;
    float min;
    bool dropped; //flag to indicate if missile has fallen from the ship

    void Start () {
        //saves position it spawned at so can see how far it travelled
        //this is so the dropped flag can be set
        spawnPos = transform.position;

        rb = GetComponent<Rigidbody2D>();
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        nearest = null;
    }
	
	// Update is called once per frame
	void Update () {
        //starts particle effect
        ps.Play();
        //if the nearest has not yet been found
        if (nearest == null)
        {
            min = 0;
            //for every alive alien
            foreach (GameObject i in GameController.AliveAliens)
            {
                if (i != null)
                { 
                    //if this is the first loop
                    if (min == 0)
                    {
                        //set nearest and min to the distance of the first alien
                        min = Vector3.Distance(i.transform.position, gameObject.transform.position);
                        nearest = i;
                    }
                    //otherwise if not first alien and the distance from the gameobject to this alien is less than it is from the nearest
                    //and its alive; then set the min and nearest to this one
                    else if ((Vector3.Distance(gameObject.transform.position, i.transform.position) <
                        Vector3.Distance(nearest.transform.position, gameObject.transform.position)) && i.activeSelf)
                    {
                        min = Vector3.Distance(gameObject.transform.position, i.transform.position);
                        nearest = i;
                    }
                }
            }
        }
            
        //kills the missile once it leaves the game area
        if (transform.position.x < -130)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        //if the missile hasn't dropped out of the ship yet
        if (Mathf.Abs(rb.position.y - spawnPos.y) < 8 && !dropped)
        {
            //move it downwards
            rb.velocity = -Vector3.up * speed;
        }
        //if it has reached certain distance from spawn
        else
        {
            //the missile has fully dropped
            dropped = true;
            //if the nearest has been determined
            if (nearest != null)
            {
                //move the missile towards its position
                rb.MovePosition(Vector3.MoveTowards(transform.position, nearest.transform.position, (speed * 5) * Time.deltaTime));
                
            }
            //otherwise just fire it left
            else if (nearest == null)
            {
                rb.velocity = -Vector3.right * (speed * 5);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if it hits something other than the player
        if (!collision.gameObject.CompareTag("Player"))
        {
            //make explosion particle effect before destroying it and the missile
            GameObject bang = Instantiate(explosion, transform.position, Quaternion.identity);
            ParticleSystem parts = bang.GetComponent<ParticleSystem>();
            float totalDuration = parts.main.duration + parts.main.startLifetimeMultiplier;
            Destroy(bang, totalDuration);
            SoundController.GetSound(7).Play();
            Destroy(gameObject);
        }
    }
}
