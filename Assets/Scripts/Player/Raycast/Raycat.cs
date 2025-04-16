using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Raycat : MonoBehaviour
{
    public static event Action OnClickOnNothing;

    private Vector3 positionObj;
    private GameObject Obj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0)) // Clic souris (PC)
        //{
        //    DetectAndExecute(Input.mousePosition);
        //}

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
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
            MonoBehaviour script = hit.collider.GetComponent<MonoBehaviour>();

            Collider collider = hit.collider;

            Obj = collider.gameObject;

            positionObj = collider.bounds.center + new Vector3(0, collider.bounds.extents.y, 0);

            if (script != null)
            {
                script.Invoke("OnObjectClicked", 0f);
                
            }
        }
        else
        {
            OnClickOnNothing?.Invoke();
        }
    }

    public Vector3 GetPosition() { return  positionObj; }
    public GameObject GetObj() { return Obj; }
}
