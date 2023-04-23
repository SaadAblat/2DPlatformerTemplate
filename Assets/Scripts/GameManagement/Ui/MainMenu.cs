using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string levelToLoad;

    public void StartTheGame()
    {
        SceneManager.LoadScene("CutScene1");
    }
    public void loadFirstLevel()
    {
        SceneManager.LoadScene(levelToLoad);

    }
    public void ExitTheGame()
    {
        Application.Quit();
    }
}

