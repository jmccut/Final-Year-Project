using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    public static AudioSource[] Sounds;
    private void Awake()
    {
        //makes gameobject persistent
        //this is so it can be used from any scene in the game
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        //fetch reference to sounds list
        Sounds = GetComponents<AudioSource>();
	}

	//static method to get and play sounds from sound list
    //static so it can be used from any script
    public static AudioSource GetSound(int index)
    {
        return Sounds[index];
    }

    //method to play selection sound as it is commonly used
    public void playSelectSound()
    {
        Sounds[5].Play();
    }
}
