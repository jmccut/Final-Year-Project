using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the wep parts in the crates and interaction with them
public class CrateController : MonoBehaviour {
    private bool near;
    private int parts;

    private void Start()
    {
        if (GameManagerS.PowerUps[2])
        {
            //crate has double the number of random parts
            parts = Random.Range(5, 10);
        }
        else
        {
            if (PlayerPrefs.GetInt("HardMode") == 0)
            {
                //crate has random number of parts
                parts = Random.Range(1, 5);
            }
            else
            {
                parts = Random.Range(3, 7);
            }
        }

    }

    void Update () {
        //if the player touches the screen whilst near this crate
        if (Input.touchCount == 1 && near)
        {
            //casts ray from touch point
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            //if the ray hits a collider and is this gameobject
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.Equals(gameObject))
            {
                //add crate parts to game manager parts
                GameManagerS.Parts += parts;
                GameManagerS.TotalPartsCollected += parts;
                parts = 0;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            near = true;
        }
    }
}
