using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public Animator transition;
    public Animator transitionToMainMenu;
    [SerializeField]
    private float transitionTime = 1f;

    [SerializeField] private bool skipEndTransition = false;

    public static TransitionManager Instance;
    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        if (skipEndTransition)
        {
            transition.SetBool("SkipEndTransition", true);
        }
    }

    public void StartGame()
    {
        StartCoroutine(LoadScene("Pathfinding_1.0"));
    }

    public void BackToMainMenu()
    {
        StartCoroutine(MainMenu("MainMenu"));
    }



    IEnumerator LoadScene(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator MainMenu(string sceneName)
    {
        transitionToMainMenu.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

}
