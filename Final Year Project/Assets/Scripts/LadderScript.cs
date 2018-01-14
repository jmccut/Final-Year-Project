using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LadderScript : MonoBehaviour {
    public ChangeScene change;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                change.Change(3);
            }
            else if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                change.Change(4);
            }
        } 
    }
}
