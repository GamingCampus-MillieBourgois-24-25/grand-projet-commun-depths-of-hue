using System;
using System.Collections.Generic;
using UnityEngine;

public class GestionCadre : MonoBehaviour
{
    private static readonly int IsWalk = Animator.StringToHash("IsWalk");
    private static readonly int IsLeft = Animator.StringToHash("IsLeft");
    [SerializeField] private DeplacementPlayer player;
    private Animator playerAnimator;
    public Transform center;
    
    [Header("Manage Arrows")]
    [SerializeField] private bool ArrowLeft;
    [SerializeField, HideInInspector] private GameObject targetCadreLeft;
    [SerializeField, HideInInspector] private GameObject arrowLeft;
    [SerializeField, HideInInspector] private string tagToFoundCadreLeft;
    [SerializeField, HideInInspector] private float angleTargetLeft;
    
    [SerializeField] private bool ArrowRight;
    [SerializeField, HideInInspector] private GameObject targetCadreRight;
    [SerializeField, HideInInspector] private GameObject arrowRight;
    [SerializeField, HideInInspector] private string tagToFoundCadreRight;
    [SerializeField, HideInInspector] private float angleTargetRight;
    
    [SerializeField] private bool ArrowUp;
    [SerializeField, HideInInspector] private GameObject targetCadreUp;
    [SerializeField, HideInInspector] private GameObject arrowUp;
    [SerializeField, HideInInspector] private string tagToFoundCadreUp;
    [SerializeField, HideInInspector] private float angleTargetUp;
    
    [SerializeField] private bool ArrowDown;
    [SerializeField, HideInInspector] private GameObject targetCadreDown;
    [SerializeField, HideInInspector] private GameObject arrowDown;
    [SerializeField, HideInInspector] private string tagToFoundCadreDown;
    [SerializeField, HideInInspector] private float angleTargetDown;
    
    private readonly Dictionary<GameObject, bool> arrowsVisibilities = new Dictionary<GameObject, bool>();
    private readonly Dictionary<GameObject, bool> stockCadreTarget = new Dictionary<GameObject, bool>();
    private readonly Dictionary<GameObject, GameObject> arrowToCadre = new();

    #region Getter Bool
    public bool ArrowLeftBool => ArrowLeft;
    public bool ArrowRightBool => ArrowRight;
    public bool ArrowUpBool => ArrowUp;
    public bool ArrowDownBool => ArrowDown;
    #endregion
    
    #region Setter GameObject
    public GameObject TargetCadreLeftGO { set => targetCadreLeft = value; }
    public GameObject TargetCadreRightGO { set => targetCadreRight = value; }
    public GameObject TargetCadreUpGO { set => targetCadreUp = value; }
    public GameObject TargetCadreDownGO { set => targetCadreDown = value; }
    #endregion


    private void Awake()
    {
        FoundPlayer();
        StockTargetCadre();
        StockVisiblities();
    }

    private void Start()
    {
        if (!player) return;
        playerAnimator = player.GetComponent<Animator>();
    }

    #region Gestion d'événements
    private void OnEnable()
    {
        BackgroundGridGenerator.OnSpawnCadre += FoundTargetCadre;
    }
    
    private void OnDisable()
    {
        BackgroundGridGenerator.OnSpawnCadre -= FoundTargetCadre;
    }

    private void FoundTargetCadre()
    {
        // assignation des arrows à un cadre target (si y'en a), une fois que je les ai trouvé juste en haut
        if (arrowLeft && targetCadreLeft) arrowToCadre[arrowLeft] = targetCadreLeft;
        if (arrowRight && targetCadreRight) arrowToCadre[arrowRight] = targetCadreRight;
        if (arrowUp && targetCadreUp) arrowToCadre[arrowUp] = targetCadreUp;
        if (arrowDown && targetCadreDown) arrowToCadre[arrowDown] = targetCadreDown;
    }
    #endregion

    #region Attribution des différentes varaibles
    private void FoundPlayer()
    {
        // permet de trouver le joueur
        GameObject playerToFound = GameObject.FindGameObjectWithTag("Player");
        player = playerToFound.GetComponent<DeplacementPlayer>();
    }

    public void StockVisiblities()
    {
        // permet de stock les arrows key->value pour avoir l'info si ils sont visibles ou non
        // Gestion de l'anti duplication dans la map pour éviter les erreurs
        if (!arrowsVisibilities.ContainsKey(arrowLeft) || !arrowsVisibilities.ContainsValue(ArrowLeft)) arrowsVisibilities.Add(arrowLeft, ArrowLeft);
        if (!arrowsVisibilities.ContainsKey(arrowRight) || !arrowsVisibilities.ContainsValue(ArrowRight)) arrowsVisibilities.Add(arrowRight, ArrowRight);
        if (!arrowsVisibilities.ContainsKey(arrowUp) || !arrowsVisibilities.ContainsValue(ArrowUp)) arrowsVisibilities.Add(arrowUp, ArrowUp);
        if (!arrowsVisibilities.ContainsKey(arrowDown) || !arrowsVisibilities.ContainsValue(ArrowDown)) arrowsVisibilities.Add(arrowDown, ArrowDown);
    }
    
    private void StockTargetCadre()
    {
        // permet de stock les cadre key->value pour avoir l'info de quel flèches mènent à quel cadre
        if (targetCadreLeft) stockCadreTarget.Add(targetCadreLeft, ArrowLeft);
        if (targetCadreRight) stockCadreTarget.Add(targetCadreRight, ArrowRight);
        if (targetCadreUp) stockCadreTarget.Add(targetCadreUp, ArrowUp);
        if (targetCadreDown) stockCadreTarget.Add(targetCadreDown, ArrowDown);
    }
    #endregion

    #region Gestion des arrows
    // gére la visibilités des arrows quand j'arrive sur le cadre
    public void SetArrowsVisibilities()
    {
        foreach(KeyValuePair<GameObject, bool> visibility in arrowsVisibilities)
        {
            GameObject arrow = visibility.Key;
            bool isVisible = visibility.Value;

            if (!isVisible)
            {
                arrow.SetActive(false);
                continue;
            }
            arrow.SetActive(true);
        }
    }

    // permet de reset les arrows visible quand le joueur rejoint un autre cadre
    private void ResetArrows()
    {
        foreach(KeyValuePair<GameObject, bool> visibility in arrowsVisibilities)
        {
            GameObject arrow = visibility.Key;
            bool isVisible = visibility.Value;

            if (isVisible)
            {
                arrow.SetActive(false);
            }
        }
    }
    #endregion

    // permet de naviguer au prochain cadre ciblé
    public void NavigateCadre(GameObject _original)
    {
        ResetArrows();

        if (!arrowsVisibilities.ContainsKey(_original) || !arrowsVisibilities[_original]) return;
        if (!arrowToCadre.TryGetValue(_original, out var cadre)) return;
        
        player.SetPlayerDestination(cadre.transform.TransformPoint(cadre.GetComponent<GestionCadre>().center.localPosition));
        cadre.GetComponent<GestionCadre>().SetArrowsVisibilities();
        player.MovePlayer();
        
        float angleZ = 0;

        switch (_original.name)
        {
            case "ArrowDown":
                Debug.Log("Direction down");
                angleZ = cadre.GetComponent<GestionCadre>().angleTargetDown;
                playerAnimator.SetBool(IsWalk, true);
                playerAnimator.SetBool(IsLeft, false);
                break;
            case "ArrowLeft":
                Debug.Log("Direction left");
                angleZ = 180f;
                playerAnimator.SetBool(IsWalk, false);
                playerAnimator.SetBool(IsLeft, true);
                break;
            case "ArrowRight":
                Debug.Log("Direction right");
                angleZ = 0;
                playerAnimator.SetBool(IsWalk, false);
                playerAnimator.SetBool(IsLeft, true);
                break;
            case "ArrowUp":
                Debug.Log("Direction up");
                angleZ = 0f;
                playerAnimator.SetBool(IsWalk, true);
                playerAnimator.SetBool(IsLeft, false);
                break;
        }
        
        player.transform.rotation = Quaternion.Euler(0, 0, angleZ);
    }
}
