using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien3Hit : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            transform.parent.GetComponent<AlienController>().health -= PlayerController.Damage;
        }
        if (collision.CompareTag("Missile"))
        {
            transform.parent.GetComponent<AlienController>().Dead();
        }
    }
}
