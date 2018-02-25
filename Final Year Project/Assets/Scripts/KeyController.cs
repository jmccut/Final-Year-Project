using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {
    void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * 50f, Space.World);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharController.GotKey = true;
            SoundController.GetSound(1).Play();
            Destroy(gameObject);
        }
    }
}
