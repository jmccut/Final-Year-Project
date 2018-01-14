using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideController : MonoBehaviour {
    public GameObject enemy;

	void Start () {
        //instantiates enemies
        for(int i = 0; i <5; i++)
        {
            Instantiate(enemy);
        }
	}
}
