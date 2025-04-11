using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clic souris (PC)
        {
            DetectAndExecute(Input.mousePosition);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // Tactile (mobile)
        {
            DetectAndExecute(Input.GetTouch(0).position);
        }
    }

    void DetectAndExecute(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            MonoBehaviour script = hit.collider.GetComponent<MonoBehaviour>(); // Récupère le premier script attaché

            if (script != null)
            {

                script.Invoke("OnObjectClicked", 0f);
            }
            else
            {
                print("il n'y a rien");
            }
        }
    }
}
