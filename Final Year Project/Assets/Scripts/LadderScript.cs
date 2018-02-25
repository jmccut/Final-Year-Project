using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LadderScript : MonoBehaviour {
    public ChangeScene change;
    public Material ladder;
    private AudioSource sound;
    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CharController.GotKey)
        {
            gameObject.GetComponent<MeshRenderer>().material = ladder;
            sound.Play();
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                change.Change(3);
            }
            else if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                change.Change(4);
            }

            CharController.GotKey = false;
        } 
    }
}
