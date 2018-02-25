using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingArrow : MonoBehaviour {
    //script to make tutorial arrow bounce up and down

    float origin;
    private void Start()
    {
        //saves the y position so it does not change
        origin = transform.position.y;
    }
    void Update () {
        //make y position change between 2 values over time
        transform.position = new Vector3(transform.position.x, origin + Mathf.PingPong(Time.time * 5, 5), transform.position.z);
    }
}
