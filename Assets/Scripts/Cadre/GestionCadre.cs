using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionCadre : MonoBehaviour
{
    private static readonly int IsWalk = Animator.StringToHash("IsWalk");
    private static readonly int IsLeft = Animator.StringToHash("IsLeft");
    [SerializeField] private DeplacementPlayer player;
    private Animator playerAnimator;
    public Transform center;
    private float angleZStart = 0f;
    private float angleZStartSide = 0f;
    private bool isRotating = false;
    
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
        arrowToCadre.Clear();
        // assignation des arrows à un cadre target (si y'en a), une fois que je les ai trouvé juste en haut
        if (arrowLeft && targetCadreLeft) arrowToCadre[arrowLeft] = targetCadreLeft;
        if (arrowRight && targetCadreRight) arrowToCadre[arrowRight] = targetCadreRight;
        if (arrowUp && targetCadreUp) arrowToCadre[arrowUp] = targetCadreUp;
        if (arrowDown && targetCadreDown) arrowToCadre[arrowDown] = targetCadreDown;
    }
    #endregion

    #region Attribution des différentes variables
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
        arrowsVisibilities.TryAdd(arrowLeft, ArrowLeft);
        arrowsVisibilities.TryAdd(arrowRight, ArrowRight);
        arrowsVisibilities.TryAdd(arrowUp, ArrowUp);
        arrowsVisibilities.TryAdd(arrowDown, ArrowDown);
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
        
        player.SetPlayerDestination(cadre.transform.TransformPoint(cadre.GetComponent<GestionCadre>().center.localPosition), cadre.GetComponent<GestionCadre>());
        player.MovePlayer();
        
        #region Gestion Rotation par Arrows
        float angleZ = 0;
        switch (_original.name)
        {
            case "ArrowDown":
                angleZ = 180f;
                if (player.PlayerPressLeftArrow || player.PlayerPressRightArrow)
                {
                    angleZStart = angleZStartSide;
                }
                else
                {
                    angleZStart = 0f;
                }
                ResetBoolAnimation("IsWalk");
                if (!player.PlayerPressDownArrow && !isRotating)
                {
                    StartCoroutine(RotateZOverTime(player.transform, angleZ, 2f));
                    isRotating = true;
                }
                player.PlayerPressDownArrow = true;
                player.PlayerPressUpArrow = false;
                ResetBoolSideArrowInPlayer();
                break;
            case "ArrowLeft":
                angleZ = 0;
                ResetBoolAnimation("IsLeft");
                angleZStartSide = -90f;
                player.PlayerPressLeftArrow = true;
                player.PlayerPressRightArrow = false;
                player.transform.rotation = Quaternion.Euler(0, 0, angleZ);
                ResetBoolUpArrowInPlayer();
                break;
            case "ArrowRight":
                angleZ = 0;
                ResetBoolAnimation("IsRight");
                angleZStartSide = 90f;
                player.PlayerPressRightArrow = true;
                player.PlayerPressLeftArrow = false;
                player.transform.rotation = Quaternion.Euler(0, 0, angleZ);
                ResetBoolUpArrowInPlayer();
                break;
            case "ArrowUp":
                angleZ = 0f;
                if (player.PlayerPressLeftArrow || player.PlayerPressRightArrow)
                {
                    angleZStart = angleZStartSide;
                }
                else
                {
                    angleZStart = 180f;
                }
                ResetBoolAnimation("IsWalk");
                if (!player.PlayerPressUpArrow && !isRotating)
                {
                    StartCoroutine(RotateZOverTime(player.transform, angleZ, 2f));
                    isRotating = true;
                }
                player.PlayerPressUpArrow = true;
                player.PlayerPressDownArrow = false;
                ResetBoolSideArrowInPlayer();
                break;
        }
        #endregion
    }

    private void ResetBoolAnimation(string _animToSkip)
    {
        if (!playerAnimator || !player) return;
        foreach (var t in player.ListAnimations)
        {
            if (t == _animToSkip)
            {
                playerAnimator.SetBool(Animator.StringToHash(t), true);
                continue;
            }
            playerAnimator.SetBool(Animator.StringToHash(t), false);
        }
    }

    private IEnumerator RotateZOverTime(Transform obj, float targetZAngle, float duration, float step = 0f)
    {
        isRotating = true;
        
        float startZ = angleZStart;
        float shortestAngle = Mathf.DeltaAngle(startZ, targetZAngle);

        float z = startZ + shortestAngle * (step / duration);
        obj.rotation = Quaternion.Euler(obj.eulerAngles.x, obj.eulerAngles.y, z);

        if (step < duration)
        {
            yield return new WaitForEndOfFrame(); // ou yield return null
            StartCoroutine(RotateZOverTime(obj, targetZAngle, duration, step + Time.deltaTime));
        }
        else
        {
            obj.rotation = Quaternion.Euler(obj.eulerAngles.x, obj.eulerAngles.y, targetZAngle);
            isRotating = false;
        }
    }

    private void ResetBoolSideArrowInPlayer()
    {
        player.PlayerPressLeftArrow = false;
        player.PlayerPressRightArrow = false;
    }
    
    private void ResetBoolUpArrowInPlayer()
    {
        player.PlayerPressUpArrow = false;
        player.PlayerPressDownArrow = false;
    }
}