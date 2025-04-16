using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class GestionInputs : MonoBehaviour
{
    private Controls controls;
    private Camera _camera;
    
    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnEnable()
    {
        TouchSimulation.Enable();
    }

    private void Start()
    {
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        _camera = Camera.main;
    }

    private void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.isTap)
            {
                Vector3 touchPosition = touch.screenPosition;
                Ray ray = _camera.ScreenPointToRay(touchPosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
                if (hit.collider)
                {
                    Debug.Log("Objet touché : " + hit.collider.gameObject.name);
                    // MonoBehaviour script = hit.collider.GetComponent<MonoBehaviour>();
                    //
                    // if (script)
                    // {
                    //     script.Invoke("OnObjectClicked", 0f);
                    // }
                    // else
                    // {
                    //     print("il n'y a rien");
                    // }

                    // if (hit.collider.CompareTag("Ancre"))
                    // {
                    //     MapNavigateCadre hitMapNavigate = hit.collider.GetComponent<MapNavigateCadre>();
                    //     if (hitMapNavigate) hitMapNavigate.ClickMapNavigate();
                    // }
                }
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPosition = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(touchPosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (!hit.collider) return;
            if (hit.collider.CompareTag("Ancre"))
            {
                MapNavigateCadre hitMapNavigate = hit.collider.GetComponent<MapNavigateCadre>();
                if (hitMapNavigate) hitMapNavigate.ClickMapNavigate();
            }
        }
        else
        {
            Debug.Log("rien n'est touché");
        }
    }
}
