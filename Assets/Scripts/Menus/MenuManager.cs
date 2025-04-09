using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvasGameObject;
    [SerializeField] private GameObject _creditsCanvasGameObject;
    [SerializeField] private GameObject _pauseCanvasGameObject;
    [SerializeField] private float transitionTimeSettings = 0.5f;
    [SerializeField] private float transitionTimeCredits = 1f;
    public Animator transitionSettings;
    public Animator transitionCredits;

    private void Awake()
    {
        if(_settingsCanvasGameObject != null)
        {
            _settingsCanvasGameObject.SetActive(false);
        }
        if (_creditsCanvasGameObject != null)
        {
            _creditsCanvasGameObject.SetActive(false);
        }
        if (_pauseCanvasGameObject != null)
        {
            _pauseCanvasGameObject.SetActive(false);
        }
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
        StartCoroutine(CloseSettingsTransition());
    }
    
    public void OpenCredits()
    {
        _creditsCanvasGameObject.SetActive(true);
    }

    public void CloseCredits()
    {
        StartCoroutine(CloseCreditsTransition());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //Pause menus
    public void OpenPauseMenu()
    {
        _pauseCanvasGameObject.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        _pauseCanvasGameObject.SetActive(false);
    }

    public void SaveGame()
    {

    }

    public void QuitToMainMenu()
    {
        TransitionManager.Instance.BackToMainMenu();
    }

    IEnumerator CloseSettingsTransition()
    {
        transitionSettings.SetTrigger("End");
        yield return new WaitForSeconds(transitionTimeSettings);
        _settingsCanvasGameObject.SetActive(false);
    }

    IEnumerator CloseCreditsTransition()
    {
        transitionCredits.SetTrigger("End");
        yield return new WaitForSeconds(transitionTimeCredits);
        _creditsCanvasGameObject.SetActive(false);
    }

}
