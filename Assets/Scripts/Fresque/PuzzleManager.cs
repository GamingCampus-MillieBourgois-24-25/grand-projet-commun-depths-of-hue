using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private Inventaire _inventaire;

    public static PuzzleManager Instance;
    public GameObject fresque;

    public Transform aled;

    public int rowAmount = 4;
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

        // Get the size of the object
        Collider collider = fresque.GetComponent<Collider>();

        if (collider != null)
        {
            // If it's a Collider, use bounds but ignore the scale effect
            longueur = collider.bounds.size.x / fresque.transform.lossyScale.x;
            largeur = collider.bounds.size.y / fresque.transform.lossyScale.y;
        }
        else
        {
            // If it's a RectTransform, use the rect size directly
            RectTransform fresqueRectTransform = fresque.GetComponent<RectTransform>();
            if (fresqueRectTransform != null)
            {
                largeur = fresqueRectTransform.rect.height;
                longueur = fresqueRectTransform.rect.width;
            }
        }

        col.Clear();
        row.Clear();

        // This divides the width and length by the amount of wanted row/column.
        for (int i = 0; i < 4; i++)
        {
            col.Add((longueur / columnAmount) * i);
            row.Add((largeur / rowAmount) * i);
        }
    }

    public void OStart()
    {
        Debug.Log("sl");
        StartCoroutine(InitializePuzzleWithDelay());
    }

    private IEnumerator InitializePuzzleWithDelay()
    {
        _inventaire = FindObjectOfType<Inventaire>();
        if (_inventaire == null)
        {
            Debug.LogError("Inventaire non trouvé dans la scène!");
            yield break;
        }


        // Tant qu'il reste des fragments dans l'inventaire
        while (_inventaire.HasFragments())
        {
            Debug.Log("ooso");
            // Instancie le prochain fragment
            GameObject fragmentObj = _inventaire.InstantiateNextFragment();
            PuzzleFragment fragment = fragmentObj.GetComponent<PuzzleFragment>();

            if (fragment != null)
            {
                AddFragmentToFresque(fragment);

                // Supprime le fragment instancié de la liste
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
        // Calcul de la taille d'une cellule (case de la grille)
        float fragmentWidth = longueur / columnAmount;  // Largeur d'une cellule
        float fragmentHeight = largeur / rowAmount;    // Hauteur d'une cellule

        // Récupérer la taille actuelle du fragment (avant redimensionnement)
        Vector3 currentScale = fragment.transform.localScale;

        // Inverser la position en Y pour que (0,0) soit en haut à gauche
        float posX = (fragment.col + 0.5f) * fragmentWidth - (longueur / 2f);
        float posY = -(fragment.row + 0.5f) * fragmentHeight + (largeur / 2f);  // Inversion de Y ici
        Vector3 fragmentPosition = new Vector3(posX, posY, 0);

        // Ajouter le fragment comme enfant de la fresque
        fragment.transform.SetParent(fresque.transform);

        // Appliquer la position

        Vector3 scale = new Vector3(2.90287423f, 5.0263319f, 2);
        
        fragment.MoveFragment(fragmentPosition, scale);
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