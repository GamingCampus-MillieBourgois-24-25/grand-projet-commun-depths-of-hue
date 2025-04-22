using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundEnigme : Enigme
{
    public List<Corail> corals;  
    public List<int> correctSequence; 
    private List<int> playerSequence;  
    public GameObject statue;

    public GameObject panel;
    public CanvasGroup panelImage;
    public Image panelBackground;

    public GameObject fragment;

    private bool sequenced =false;

    public static SoundEnigme Instance;

    public AudioClip lose;

    public override void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        base.Initialize();

        for (int i = 0; i < corals.Count; i++)
        {
            int index = i;
            corals[i].OnCorailClicked += () => OnCoralClicked(index, corals[index]);
        }
        
    }

    public void StartRound()
    {
        panel.SetActive(true);
        Debug.Log("started");
        sequenced = true;
        StartCoroutine(FadeOverlay(1));
        GenerateMelody();
        playerSequence = new List<int>();
    }

    IEnumerator FadeOverlay(float targetAlpha)
    {
        float duration = 0.5f;
        float startAlpha = panelImage.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            panelImage.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            yield return null;
        }


        panelImage.alpha = targetAlpha;

    }

    void GenerateMelody()
    {
        correctSequence = new List<int>();
        for (int i = 0; i < corals.Count; i++)
        {
            correctSequence.Add(UnityEngine.Random.Range(0, corals.Count));
            corals[i].PlaySound();
            
        }
    }

    public void OnCoralClicked(int coralIndex, Corail coral)
    {
        if (sequenced == true)
        {

        
        Debug.Log("coral : " +coralIndex);

        playerSequence.Add(coralIndex);


        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                coral.SwapSound(lose);
                coral.PlaySound();
                ResetPuzzle();
                return;
            }
        }
        coral.PlaySound();

        if (playerSequence.Count == correctSequence.Count)
        {
            SolvePuzzle();
        }
        }
        else
        {
            coral.PlaySound();
        }
    }

    void ResetPuzzle()
    {
        sequenced = false;
        playerSequence.Clear();
        Debug.Log("ono");

        StartCoroutine(FlashRedThenFadeOut());
    }

    void SolvePuzzle()
    {
        StartCoroutine(RotateStatueTowardsCamera());
    }

    IEnumerator RotateStatueTowardsCamera()
    {
        float duration = 5f;
        float timer = 0f;

        Quaternion startRotation = statue.transform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 90f, 0f); 

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            statue.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        statue.transform.localRotation = endRotation;
        fragment.SetActive(true);

    }


    IEnumerator FlashRedThenFadeOut()
    {
        // Flash rouge
        Color originalColor = panelBackground.color;
        panelBackground.color = new Color(1f, 0f, 0f, originalColor.a);
    

        yield return new WaitForSeconds(0.7f); // durée du flash

        

        // Fade out ensuite
        yield return StartCoroutine(FadeOverlay(0));

        // Retour à la couleur d'origine (noir ou autre)
        panelBackground.color = originalColor;
    }

    

}
