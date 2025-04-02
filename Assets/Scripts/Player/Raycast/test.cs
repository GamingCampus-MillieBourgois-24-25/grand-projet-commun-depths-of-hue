using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private bool launchTimer;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (launchTimer)
        {
            timer += Time.deltaTime;
            if (timer >= 3) 
            {
                print("fini");
            }
        }
        
    }

    public void OnObjectClicked()
    {
        Debug.Log(gameObject.name + " a été cliqué !");
        launchTimer = true;
        // Ajoutez ici l'action que l'objet doit effectuer
    }
}
