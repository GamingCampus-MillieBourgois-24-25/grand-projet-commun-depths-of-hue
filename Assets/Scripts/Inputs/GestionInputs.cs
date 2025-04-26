using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class GestionInputs : MonoBehaviour
{


    private Controls controls;
    private Camera _camera;

    public static event Action OnClickOnNothing;
    public static event Action<GameObject> OnClickOnGameObject;

    private Vector3 positionObj;
    private GameObject Obj;

    #region Event

    public delegate void PlayerGoFrontEnigme(Vector3 _position, DoorController _doorController, int _direction);
    public static event PlayerGoFrontEnigme OnPlayerGoFront;

    #endregion

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
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {

                    GameObject go = hit.transform.gameObject;

                    if (go != null)
                    {
                        OnClickOnGameObject?.Invoke(go);
            

                        return;
                    }
                    //MonoBehaviour script = hit.collider.GetComponent<MonoBehaviour>();

                    //Collider collider = hit.collider;

                    //Obj = collider.gameObject;

                    //positionObj = collider.bounds.center + new Vector3(0, collider.bounds.extents.y, 0);

                    //if (script != null)
                    //{
                    //    script.Invoke("OnObjectClicked", 0f);

                    //}

                    // if (hit.collider.CompareTag("Ancre"))
                    // {
                    //     MapNavigateCadre hitMapNavigate = hit.collider.GetComponent<MapNavigateCadre>();
                    //     if (hitMapNavigate) hitMapNavigate.ClickMapNavigate();
                    // }
                }
                else
                {
                    OnClickOnNothing?.Invoke();
                    return;
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPosition = Input.mousePosition;
            MapNavigateCadre();
            StartEnigme(touchPosition);
        }
    }

    public Vector3 GetPosition() { return positionObj; }
    public GameObject GetObj() { return Obj; }

    private void MapNavigateCadre()
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
    
    private void StartEnigme(Vector3 _touchPosition)
    {
        Ray ray = _camera.ScreenPointToRay(_touchPosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (!hit.collider) return;
        if (hit.collider.CompareTag("Doors"))
        {
            DoorController hitDoorController = hit.collider.GetComponent<DoorController>();
            OnPlayerGoFront?.Invoke(hitDoorController.gameObject.transform.position, hitDoorController, hitDoorController.Direction);
        }
    }

    /*public Vector3 GetPosition() { return positionObj; }
    public GameObject GetObj() { return Obj; }*/
}
