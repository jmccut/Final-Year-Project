using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondAlienCollider : MonoBehaviour {
    private AlienController parentScript;
    void Start()
    {
        //fetches reference to alien controller (script of parent game object)
        parentScript = transform.parent.GetComponent<AlienController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the alien is not dead
        if (parentScript != null)
        {
            //call method from parent script and pass the name of the collider and the collision object
            StartCoroutine(parentScript.recieveTriggerEnter(name, collision));
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //call exit method from parent script with collider name and collision object
        parentScript.recieveTriggerExit(name, collision);
    }





}
