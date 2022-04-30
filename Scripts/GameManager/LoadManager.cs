using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("Lobby");
    }
}
