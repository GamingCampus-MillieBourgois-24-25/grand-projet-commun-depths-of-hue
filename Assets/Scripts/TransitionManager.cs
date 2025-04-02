using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public Animator transition;
    [SerializeField]
    private float transitionTime = 1f;

    [SerializeField] private bool skipEndTransition = false;

    public static TransitionManager instance;
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
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

    
    IEnumerator LoadScene(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
