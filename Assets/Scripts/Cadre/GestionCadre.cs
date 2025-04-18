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
    public Transform centerMiddle;
    private float angleZStart = 0f;
    private float angleZStartSide = 0f;
    private bool isRotating = false;
    private Vector3 direction;
    private GameObject targetCadre = null;
    
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
    private List<GestionCadre> allCadres = new();

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

    #region Event

    public delegate void SendNewStatus(GestionCadre _cadre);
    public static event SendNewStatus OnSendNewStatus;

    public delegate void ShowUIGame(bool _isShow);
    public static event ShowUIGame OnShowUI;

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
        if (gameObject.CompareTag("ActualCadre")) MapNavigateCadre.OnMove += MapNavigateCadreFunc;
        if (gameObject.CompareTag("ActualCadre")) GetMiddleCadre.OnCalculNewRotation += CalculNewRotation;
        BackgroundGridGenerator.OnSendCadre += AddToAllCadre;
        GetMiddleCadre.OnGetMiddleCadre += SetCenterMiddle;
    }
    
    private void OnDisable()
    {
        BackgroundGridGenerator.OnSpawnCadre -= FoundTargetCadre;
        if (gameObject.CompareTag("ActualCadre")) MapNavigateCadre.OnMove -= MapNavigateCadreFunc;
        if (gameObject.CompareTag("ActualCadre")) GetMiddleCadre.OnCalculNewRotation -= CalculNewRotation;
        BackgroundGridGenerator.OnSendCadre -= AddToAllCadre;
        GetMiddleCadre.OnGetMiddleCadre -= SetCenterMiddle;
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

    private void AddToAllCadre(GestionCadre obj)
    {
        if (!allCadres.Contains(obj)) allCadres.Add(obj);
    }

    private void SetCenterMiddle(Transform _center)
    {
        centerMiddle = _center;
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
        
        Debug.Log("reset all arrows from : " + gameObject.name);
    }
    #endregion

    // permet de naviguer au prochain cadre ciblé
    public void NavigateCadre(GameObject _original)
    {
        OnShowUI?.Invoke(false);
        gameObject.tag = "Untagged";
        ResetArrows();

        if (!arrowsVisibilities.ContainsKey(_original) || !arrowsVisibilities[_original]) return;
        if (!arrowToCadre.TryGetValue(_original, out var cadre)) return;
        
        player.SetPlayerDestination(cadre.transform.TransformPoint(cadre.GetComponent<GestionCadre>().center.localPosition), cadre.GetComponent<GestionCadre>());
        player.MovePlayer();
        cadre.gameObject.tag = "ActualCadre";
        OnSendNewStatus?.Invoke(cadre.GetComponent<GestionCadre>());

        ManageRotationMovement(_original);
    }
    
    private void MapNavigateCadreFunc(string _targetCadre)
    {
        ResetArrows();
        ResetAllArrows();

        GameObject cadre = CompareName(_targetCadre);

        if (!cadre)
        {
            Debug.LogError("cadre null");
        }

        // le middle cadre - l'actual cadre (universel pour toute direction)
        ManageRotation(cadre);
        Debug.Log(direction);
        
        player.SetPlayerDestination(cadre.transform.TransformPoint(cadre.GetComponent<GestionCadre>().center.localPosition), cadre.GetComponent<GestionCadre>());
        player.MovePlayer();
        OnSendNewStatus?.Invoke(cadre.GetComponent<GestionCadre>());

        ManageRotationMovement(cadre, cadre.name != "CadreMidTemple(Clone)");
    }

    private void ManageRotation(GameObject _cadre)
    {
        if (_cadre.name == "CadreMidTemple(Clone)") return;
        if (!centerMiddle) FindCenterMiddle();

        direction = Vector3.Normalize(centerMiddle.transform.position - transform.position);
        targetCadre = _cadre;
    }

    private void FindCenterMiddle()
    {
        GameObject centerMiddleFound = GameObject.FindGameObjectWithTag("centerMidCadre");
        centerMiddle = centerMiddleFound.transform;
    }

    private void CalculNewRotation()
    {
        // le target cadre - l'actual cadre
        if (!targetCadre) return;
        if (targetCadre.name == "CadreMidTemple(Clone)") return;
        
        var heading = targetCadre.transform.position - transform.position;
        var distance = heading.magnitude;
        Vector3 directionTemp = heading / distance;
        direction = new Vector3(
            Mathf.RoundToInt(directionTemp.x),
            Mathf.RoundToInt(directionTemp.y),
            Mathf.RoundToInt(directionTemp.z)
        );
        
        ManageRotationMovement(targetCadre, true);
    }

    private void ResetAllArrows()
    {
        foreach (var cadre in allCadres)
        {
            if (cadre.arrowDown) cadre.arrowDown.SetActive(false);
            if (cadre.arrowLeft) cadre.arrowLeft.SetActive(false);
            if (cadre.arrowRight) cadre.arrowRight.SetActive(false);
            if (cadre.arrowUp) cadre.arrowUp.SetActive(false);
        }
    }
    
    #region Gestion Rotation par Arrows
    private void ManageRotationMovement(GameObject _original, bool isForMap = false)
    {
        float angleZ = 0;
        Vector3 _direction = isForMap ? direction : GetDirection(_original.name);
        Vector3Int _dirInt = Vector3Int.RoundToInt(_direction);

        if (_dirInt == Vector3.down)
        {
            angleZ = 180f;
            RotationBottom(angleZ);
        }
        else if (_dirInt == Vector3.left || _dirInt == Vector3.left + Vector3.down)
        {
            angleZ = 0;
            RotationLeft(angleZ);
        }
        else if (_dirInt == Vector3.right || _dirInt == Vector3.right + Vector3.down)
        {
            angleZ = 0;
            RotationRight(angleZ);
        }
        else if (_dirInt == Vector3.up)
        {
            angleZ = 0;
            RotationUp(angleZ);
        }
    }

    private Vector3 GetDirection(string _arrows)
    {
        return _arrows switch
        {
            "ArrowDown" => Vector3.down,
            "ArrowUp" => Vector3.up,
            "ArrowLeft" => Vector3.left,
            "ArrowRight" => Vector3.right,
            _ => Vector3.zero
        };
    }
    #endregion

    #region RotationBottom

    private void RotationBottom(float _angleZ)
    {
        float angleZ = _angleZ;
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
    }

    #endregion
    #region RotationUp

    private void RotationUp(float _angleZ)
    {
        float angleZ = _angleZ;
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
    }

    #endregion
    #region RotationLeft

    private void RotationLeft(float _angleZ)
    {
        float angleZ = _angleZ;
        ResetBoolAnimation("IsLeft");
        angleZStartSide = -90f;
        player.PlayerPressLeftArrow = true;
        player.PlayerPressRightArrow = false;
        player.transform.rotation = Quaternion.Euler(0, 0, angleZ);
        ResetBoolUpArrowInPlayer();
    }

    #endregion
    #region RotationRight

    private void RotationRight(float _angleZ)
    {
        float angleZ = _angleZ;
        ResetBoolAnimation("IsRight");
        angleZStartSide = 90f;
        player.PlayerPressRightArrow = true;
        player.PlayerPressLeftArrow = false;
        player.transform.rotation = Quaternion.Euler(0, 0, angleZ);
        ResetBoolUpArrowInPlayer();
    }

    #endregion
    
    private GameObject CompareName(string _target)
    {
        if (allCadres.Count == 0) Debug.LogError("allCadres est null");
        foreach (var kvp in allCadres)
        {
            if (kvp.name != _target) continue;
            return kvp.gameObject;
        }
        return null;
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