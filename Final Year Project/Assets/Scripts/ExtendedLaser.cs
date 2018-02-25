using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedLaser : MonoBehaviour {
    public static bool Rotate { get; set; }
    private LineRenderer lr;
    public Transform destination;
    public Transform destination2;

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
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
