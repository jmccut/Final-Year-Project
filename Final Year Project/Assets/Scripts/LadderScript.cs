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
        //get sound
        sound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //if the player is on the ladder and has the key
        if (other.CompareTag("Player") && CharController.GotKey)
        {
            //set ladder sprite to open and make sound
            gameObject.GetComponent<MeshRenderer>().material = ladder;
            sound.Play();
            //change to different level depending on current level
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                change.Change(3);
            }
            else if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                change.Change(4);
            }
            //set flag to false
            CharController.GotKey = false;
        } 
    }
}
