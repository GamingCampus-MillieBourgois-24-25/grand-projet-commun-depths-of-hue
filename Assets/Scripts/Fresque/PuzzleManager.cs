using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private Inventaire _inventaire;

    public static PuzzleManager Instance;
    public GameObject fresque;

    public int rowAmount =4;
    public int columnAmount = 4;

    public float largeur;
    public float longueur;
    public List<float> col;
    public List<float> row;

    public PuzzleFragment[,] puzzleGrid;

   
    private void Awake()
    {
        puzzleGrid = new PuzzleFragment[rowAmount, columnAmount];
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Gets the size of the object
        Collider collider = fresque.GetComponent<Collider>();

        if (collider != null)
        {
            longueur = collider.bounds.size.x;  
            largeur = collider.bounds.size.y;  
        }
        else
        {
    
            RectTransform fresqueRectTransform = fresque.GetComponent<RectTransform>();
            if (fresqueRectTransform != null)
            {
                largeur = fresqueRectTransform.rect.height;
                longueur = fresqueRectTransform.rect.width;
            }
        }


        col.Clear();
        row.Clear();

        // This divides the width and length by the amount of wanted row/column. These values depend on the image splitter values.
        for (int i = 0; i < 4; i++)
        {
            col.Add((longueur / columnAmount) * i);  
            row.Add((largeur / rowAmount) * i); 
        }
    }

    private void Start()
    {
        StartCoroutine(InitializePuzzleWithDelay());
    }

    private IEnumerator InitializePuzzleWithDelay()
    {
        _inventaire = FindObjectOfType<Inventaire>();
        if (_inventaire == null)
        {
            Debug.LogError("Inventaire non trouv� dans la sc�ne!");
            yield break;
        }

        Debug.Log("ooso");
        // Tant qu'il reste des fragments dans l'inventaire
        while (_inventaire.HasFragments())
        {
            // Instancie le prochain fragment
            GameObject fragmentObj = _inventaire.InstantiateNextFragment();
            PuzzleFragment fragment = fragmentObj.GetComponent<PuzzleFragment>();

            if (fragment != null)
            {
                AddFragmentToFresque(fragment);

                // Supprime le fragment instanci� de la liste
                _inventaire.RemoveFirstFragment();
            }

            // Attend 2 secondes avant le prochain
            yield return new WaitForSeconds(2f);
        }
    }

    /// <summary>
    /// Add a fragment to the puzzle. This function places the fragment to his right position depending on the object holding this script.
    /// The parameter excpects a puzzle fragment with a row and collumn values.
    /// </summary>
    /// <param name="fragment"></param>
    public void AddFragmentToFresque(PuzzleFragment fragment)
    {
    
        float normalizedX = (col[fragment.col] / longueur) * fresque.transform.localScale.x;

        float normalizedY = (row[fragment.row] / largeur) * fresque.transform.localScale.y;


        float offsetXFresque = fresque.transform.localScale.x / 2f;

        float offsetYFresque = fresque.transform.localScale.y / 2f;
        
        float offsetXFragment = fragment.transform.localScale.x*2;
        
        float offsetYFragment = fragment.transform.localScale.y*2;

        Vector3 position = new Vector3(normalizedX - offsetXFresque + offsetXFragment, offsetYFresque - normalizedY - offsetYFragment, this.transform.position.z);

        Vector3 fragmentPosition = position;
        Vector3 fragmentScale = Vector3.one;


        float fragmentWidth = longueur / columnAmount;  
        float fragmentHeight = largeur / rowAmount;  

        //Adjusts the scale of the fragment to the fresque

        BoxCollider fragCollider = fragment.GetComponent<BoxCollider>();
        if (fragCollider != null)
        {
            Vector3 currentSize = fragCollider.size;
            Vector3 newScale = new Vector3(
                fragmentWidth / currentSize.x,
                fragmentHeight / currentSize.y,
                1f); // Conserve l'�paisseur Z

            fragmentScale = newScale;
        }
        else 
        {
            fragment.transform.localScale = new Vector3(
                fragmentWidth,
                fragmentHeight,
                1f);
        }


        if (puzzleGrid[fragment.row, fragment.col] != null)
        {
            Debug.LogWarning($"Un fragment est d�j� en {fragment.row}, {fragment.col}");
            return;
        }

        puzzleGrid[fragment.row, fragment.col] = fragment;

        MoveFragment(fragment,fragmentPosition, fragmentScale);
       
        Debug.Log(IsPuzzleComplete());

    }
    public void MoveFragment(PuzzleFragment fragment,Vector3 targetPosition, Vector3 finalScale)
    {
        Vector3 start = fragment.transform.position;
        float duration = 1.5f;

        float radius = 2.5f; // plus grand = spirale plus large
        int spinCount = 2; // nombre de tours
        float angle = 0f;

        DOTween.To(() => angle, x => {
            angle = x;
            float t = angle / (360f * spinCount);

            // Interpolation lin�aire de la position de base � finale
            Vector3 center = Vector3.Lerp(start, targetPosition, t);

            // Cr�ation d�un offset circulaire
            float radians = Mathf.Deg2Rad * angle;
            Vector3 offset = new Vector3(
                Mathf.Cos(radians),
                Mathf.Sin(radians),
                0f
            ) * radius * (1 - t);

            // Applique la position finale
            fragment.transform.position = center + offset;

        }, 360f * spinCount, duration).SetEase(Ease.InOutSine);

        // Scale
        fragment.transform.DOScale(finalScale, duration).SetEase(Ease.OutBack);

        // Rotation sur lui-m�me
        fragment.transform.DORotate(new Vector3(360f, 360f, 360f), duration, RotateMode.FastBeyond360)
                 .SetEase(Ease.InOutSine);

      
    }
    public bool IsPuzzleComplete()
    {
        foreach (var fragment in puzzleGrid)
        {
            if (fragment == null)
                return false;
        }
        return true;
    }
}