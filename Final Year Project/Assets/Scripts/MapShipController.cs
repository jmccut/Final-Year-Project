using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapShipController : MonoBehaviour {

    //HACK: This might need to be changed later to actually take into account their positions

    //holds all the valid positions the ship can be in
    //In order of: EARTH, MOON, MARS, JUPITER, SATURN, URANUS, NEPTUNE, PLUTO, MERCURY
    public float[] validPositions = { 18.9f, 10.45f, -9.3f, -30.2f, -59.7f, -81.2f, -101.9f, -110.3f, 38f };
    private float currentPos; //holds the current position of the map ship

	void Update () {
        //Updates the position of the ship on the mini-map as the levels progress
        currentPos = validPositions[GameController.Stage-1];
        transform.position = new Vector3(currentPos, 44.4f, 0f);
    }
}
