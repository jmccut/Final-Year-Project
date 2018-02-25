using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    public static AudioSource[] Sounds;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start () {
        Sounds = GetComponents<AudioSource>();
	}
	
    public static AudioSource GetSound(int index)
    {
        return Sounds[index];
    }

    public void playSelectSound()
    {
        Sounds[5].Play();
    }
}
