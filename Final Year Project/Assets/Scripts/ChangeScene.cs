using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        StartCoroutine(ChangeLevel(1));
    }

    IEnumerator ChangeLevel(int levelNum)
    {
        FaderScript fdrs = GetComponent<FaderScript>();
        float result = fdrs.BeginFade(1);
        yield return new WaitForSeconds(result);
        SceneManager.LoadScene(levelNum);
    }
}
