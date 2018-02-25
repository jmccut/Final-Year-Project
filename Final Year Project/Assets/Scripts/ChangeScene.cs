using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour {
    public void ExitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void Change(int x)
    {
        PlayerPrefs.Save();
        StartCoroutine(ChangeLevel(x));
    }

    IEnumerator ChangeLevel(int levelNum)
    {
        FaderScript fdrs = GetComponent<FaderScript>();
        float result = fdrs.BeginFade(1);
        yield return new WaitForSeconds(result);
        SceneManager.LoadScene(levelNum);
    }
}
