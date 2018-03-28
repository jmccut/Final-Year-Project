using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    public static AudioSource[] Sounds;
    private void Awake()
    {
        //makes gameobject persistent
        DontDestroyOnLoad(gameObject);
    }
    void Start () {
        Sounds = GetComponents<AudioSource>();
	}
	//static method to get and play sounds
    public static AudioSource GetSound(int index)
    {
        return Sounds[index];
    }
    //method to play selection sound commonly used
    public void playSelectSound()
    {
        Sounds[5].Play();
    }
}
