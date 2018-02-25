using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestination : MonoBehaviour {
    private GameObject copy;
    public Transform part;
    public Transform part2;
    public GameObject alien;
    public bool go;
    // Use this for initialization
    private void Start()
    {
        go = false;
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
    // Update is called once per frame
    void Update () {
        if (alien != null)
        {
            if (alien.transform.position.y > 29f || alien.transform.position.y < -29f)
            {
                go = true;
            }
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
