using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        TransitionManager.instance.StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
