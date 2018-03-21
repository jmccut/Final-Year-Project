using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien3Hit : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //only hits when collider is vertical
        if (collision.CompareTag("Bullet") && transform.eulerAngles.z > 75)
        {
            Debug.Log("here");
            transform.parent.GetComponent<AlienController>().health -= PlayerController.Damage;
        }
        if (collision.CompareTag("Missile") && transform.eulerAngles.z > 75)
        {
            transform.parent.GetComponent<AlienController>().Dead();
        }
    }
}
