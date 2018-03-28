using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthPackS : MonoBehaviour {
    public CharController player;
    private void Start()
    {
        //spawn in random location on navmesh
        Vector3 randomDirection = Random.insideUnitSphere * 25f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 25f, 1);
        transform.position = hit.position;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //if player hits health pack, increment player health and play sound
        if (other.gameObject.CompareTag("Player"))
        {
            player.IncrementHealth(25);
            SoundController.GetSound(1).Play();
            Destroy(gameObject);
        }
    }
}
