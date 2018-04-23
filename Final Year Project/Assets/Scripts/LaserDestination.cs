using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script to control the movements of the laser destination objects
public class LaserDestination : MonoBehaviour {
    private GameObject copy;
    //references to each alien in partner
    public Transform part; 
    public Transform part2;

    public GameObject alien;
    public bool go; //flag to indicate when objects can start moving

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
            //when the aliens have reached the top of the screen, the laser begins rotation
            if (alien.transform.position.y > 29f || alien.transform.position.y < -29f)
            {
                go = true;
            }
            //begin moving to destination
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
