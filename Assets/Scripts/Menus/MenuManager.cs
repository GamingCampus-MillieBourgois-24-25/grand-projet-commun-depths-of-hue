using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        TransitionManager.Instance.StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
