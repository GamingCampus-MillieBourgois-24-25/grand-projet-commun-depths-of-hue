using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeplacementPlayer : MonoBehaviour
{
    private static readonly int IsWalk = Animator.StringToHash("IsWalk");

    [Header("Property")]
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator compagnon;
    [SerializeField] private Transform targetCompagnon;
    
    [Header("Animations List")]
    [SerializeField] private List<string> listAnimations;
    public List<string> ListAnimations { get => listAnimations; set => listAnimations = value; }
    
    [SerializeField] private Vector3 playerDestination;
    private GestionCadre actualCadre;

    private Camera _camera;
    private bool uniqueSendEvent;

    #region Gestion Bool Gestion Cadre For Animation

    private bool playerPressRightArrow = false;
    private bool playerPressLeftArrow = false;
    private bool playerPressUpArrow = false;
    private bool playerPressDownArrow = false;
    public bool PlayerPressLeftArrow { get => playerPressLeftArrow; set => playerPressLeftArrow = value; }
    public bool PlayerPressRightArrow { get => playerPressRightArrow; set => playerPressRightArrow = value; }
    public bool PlayerPressUpArrow { get => playerPressUpArrow; set => playerPressUpArrow = value; }
    public bool PlayerPressDownArrow { get => playerPressDownArrow; set => playerPressDownArrow = value; }
    
    public Animator Compagnon { get => compagnon; set => compagnon = value; }
    public Transform TargetCompagnon { get => targetCompagnon; set => targetCompagnon = value; }

    #endregion

    #region Event

    public delegate void ShowUIGame(bool _isShow);
    public static event ShowUIGame OnShowUI;

    #endregion
    
    private void Start()
    {
        _camera = Camera.main;
        player.freezeRotation = true;
        uniqueSendEvent = false;
        
        Animator animator = GetComponent<Animator>();
        animator.SetBool(IsWalk, true);
        compagnon.SetBool(IsWalk, true);
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
        Vector3 rotation = transform.eulerAngles;
        rotation.x = 0;
        rotation.y = 0;
        rotation.z = 0;
        transform.eulerAngles = rotation;

        if (!navMeshAgent.isOnNavMesh) return;
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!actualCadre) return;
            actualCadre.SetArrowsVisibilities();
            if (!uniqueSendEvent)
            {
                OnShowUI?.Invoke(true);
                uniqueSendEvent = true;
            }
        }
    }
    
    public void SetPlayerDestination(Vector3 _playerDestination, GestionCadre _cadre)
    {
        playerDestination = _playerDestination;
        actualCadre = _cadre;
        uniqueSendEvent = false;
    }
    
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
