using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {
    void Update () {
        //make key spin
        transform.Rotate(Vector3.up * Time.deltaTime * 50f, Space.World);
	}

    private void OnTriggerEnter(Collider other)
    {
        //if player walks over key, set flag to true, play sound and destroy key
        if (other.CompareTag("Player"))
        {
            CharController.GotKey = true;
            SoundController.GetSound(1).Play();
            Destroy(gameObject);
        }
    }
}
