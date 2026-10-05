using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelSelect = "Level Select";
    public void StartGame()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
