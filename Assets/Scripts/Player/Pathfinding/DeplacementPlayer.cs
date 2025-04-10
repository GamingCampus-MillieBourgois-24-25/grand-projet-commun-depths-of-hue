using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class DeplacementPlayer : MonoBehaviour
{
    [Header("Property")]
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private NavMeshAgent navMeshAgent;
    
    [SerializeField] private Vector3 playerDestination;
    private GestionCadre actualCadre;

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
        player.freezeRotation = true;
        //GameObject foundActualCadre = GameObject.FindWithTag("ActualCadre");
        //actualCadre = foundActualCadre.GetComponent<GestionCadre>();
        //actualCadre.SetArrowsVisibilities();
        //player.transform.position = actualCadre.center.position;
    }

    public void MovePlayer()
    {
        if (!navMeshAgent.enabled) return;

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.SetDestination(playerDestination);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(playerDestination, Vector3.one * 1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(playerDestination, 0.3f);
    }

    private void Update()
    {
        // pas de rotation chelou sur ces axes
        var rotation = transform.eulerAngles;
        rotation.x = 0;
        rotation.y = 0;
        transform.eulerAngles = rotation;
        
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.isTap)
            {
                Vector2 touchPosition = touch.screenPosition;
                RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(touchPosition), Vector2.zero);
                if (hit.collider)
                {
                    Debug.Log("Objet touché : " + hit.collider.gameObject.name);
                    MonoBehaviour script = hit.collider.GetComponent<MonoBehaviour>();

                    if (script)
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
    }
    
    public void SetPlayerDestination(Vector3 _playerDestination)
    {
        playerDestination = _playerDestination;
    }
}
