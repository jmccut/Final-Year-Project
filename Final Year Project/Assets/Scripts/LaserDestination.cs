using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestination : MonoBehaviour {
    private GameObject copy;
    public Transform part;
    public Transform part2;
    public GameObject alien;
    public bool go;

    private void Start()
    {
        go = false;
        //set alien partners
        if(alien.GetComponent<AlienController>().SequenceNumber == 0)
        {
            part = alien.transform.GetChild(0);
            part2 = alien.GetComponent<AlienController>().partnerAlien.transform.GetChild(0);
        }
        else
        {
            part2 = alien.transform.GetChild(0);
            part = alien.GetComponent<AlienController>().partnerAlien.transform.GetChild(0);
        }
    }

    void Update () {
        if (alien != null)
        {
            //if they have reached top of the screen, they can move
            if (alien.transform.position.y > 29f || alien.transform.position.y < -29f)
            {
                go = true;
            }
            //move to the right
            if (gameObject.CompareTag("LaserDestination") && go)
            {
                transform.position = Vector3.MoveTowards(transform.position, part.position, 60f * Time.deltaTime);
            }
            else if (gameObject.CompareTag("LaserDestination2") && go)
            {
                transform.position = Vector3.MoveTowards(transform.position, part2.position, 60f * Time.deltaTime);

            }
        }
    }
}
