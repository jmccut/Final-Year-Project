using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBlink : MonoBehaviour {
    Renderer rend;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        foreach(Transform t in transform)
        {
            t.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time/2, 1));
        }
    }
}
