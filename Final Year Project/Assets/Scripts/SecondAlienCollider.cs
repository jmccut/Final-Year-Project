using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondAlienCollider : MonoBehaviour {
    private AlienController parentScript;
    void Start()
    {
        parentScript = transform.parent.GetComponent<AlienController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       StartCoroutine(parentScript.recieveTriggerEnter(name, collision));

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        parentScript.recieveTriggerExit(name, collision);
    }





}
