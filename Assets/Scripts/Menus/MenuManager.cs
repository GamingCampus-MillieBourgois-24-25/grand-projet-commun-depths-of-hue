using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvasGameObject;

    private void Awake()
    {
        _settingsCanvasGameObject.SetActive(false);
    }

    public void StartGame()
    {
        TransitionManager.Instance.StartGame();
    }

    public void OpenSettings()
    {
        _settingsCanvasGameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsCanvasGameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
