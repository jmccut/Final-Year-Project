using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedLaser : MonoBehaviour {
    public static bool Rotate { get; set; } //flag to indicate if collider has rotated
    private LineRenderer lr;
    //references to laser destination objects
    public Transform destination;
    public Transform destination2;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //when not rotated
        if (!Rotate)
        {
            //sets line renderer position
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + new Vector3(200f, 0, 0));
        }
        //when rotating
        else
        {
            //rotate laser by setting end position to the laser destination object position
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
