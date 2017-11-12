using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {

    public void DisableCanvas()
    { //disables each child of the GUI except health which is needed during the game
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("Health"))
            {
                child.gameObject.SetActive(false);
            }
        }
        
    }
    public IEnumerator EnableCanvas()
    { //waits until all wall objects have left the scene before showing GUI
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Wall") == null);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
