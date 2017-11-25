using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        StartCoroutine(ChangeLevel());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator ChangeLevel()
    {
        FaderScript fdrs = GetComponent<FaderScript>();
        float result = fdrs.BeginFade(1);
        yield return new WaitForSeconds(result);
        SceneManager.LoadScene(1);
    }
}
