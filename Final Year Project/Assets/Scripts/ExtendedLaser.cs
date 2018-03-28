using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedLaser : MonoBehaviour {
    public static bool Rotate { get; set; } //flag to indicate if collider has rotated
    private LineRenderer lr;
    //gameobjects that move the way in which the lasers rotate
    public Transform destination;
    public Transform destination2;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!Rotate)
        {
            //sets line renderer position
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + new Vector3(200f, 0, 0));
        }
        else
        {
            //rotate laser by following gameobjects
            if(transform.parent.GetComponent<AlienController>().SequenceNumber == 1)
            {
                lr.SetPosition(0, transform.position + new Vector3(-5f, 4.5f, 0));
                lr.SetPosition(1, destination.position);
            }
            else
            {
                lr.SetPosition(0, transform.position + new Vector3(-5f, -3f, 0));
                lr.SetPosition(1, destination2.position);
            }
        }
    }
}
