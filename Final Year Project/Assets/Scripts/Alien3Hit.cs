using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien3Hit : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if laser is hit by bullet while collider is vertical
        if (collision.CompareTag("Bullet") && transform.eulerAngles.z > 75)
        {
            //decrement alien health by player damage
            transform.parent.GetComponent<AlienController>().health -= PlayerController.Damage;
        }
        //if collider is hit by missile while vertical
        if (collision.CompareTag("Missile") && transform.eulerAngles.z > 75)
        {
            //aliens die
            transform.parent.GetComponent<AlienController>().Dead();
        }
    }
}
