using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void GoToScene()
    {
        SceneManager.LoadScene("Main Scene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
