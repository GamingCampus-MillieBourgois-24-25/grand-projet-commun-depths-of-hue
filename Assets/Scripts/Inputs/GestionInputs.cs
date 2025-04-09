using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GestionInputs : MonoBehaviour
{
    private Controls controls;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        //controls = new Controls();
        //controls.Enable();
        //controls.Mouse.Clic.performed += HandleClicMouse;
    }

    private void OnDisable()
    {
        //controls.Mouse.Clic.performed -= HandleClicMouse;
    }

    private void HandleClicMouse(InputAction.CallbackContext context)
    {
        Vector3 mousePosition = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(mousePosition), Vector2.zero);
        if (hit.transform.CompareTag("Arrows"))
        {
            Debug.Log("Mouse clicked : ");
        }
        
    }
}
