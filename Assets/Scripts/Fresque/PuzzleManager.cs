using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    public GameObject fresque;

    public float largeur;
    public float longueur;
    public List<float> col;
    public List<float> row;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Récupérer la taille de la fresque en utilisant le Renderer si c'est un objet 3D
        Renderer fresqueRenderer = fresque.GetComponent<Renderer>();
        if (fresqueRenderer != null)
        {
            longueur = fresqueRenderer.bounds.size.x;  // Largeur de l'objet
            largeur = fresqueRenderer.bounds.size.y;  // Hauteur de l'objet
        }
        else
        {
            // Si fresque n'a pas de Renderer (pas un objet 3D), vérifier si c'est un UI (RectTransform)
            RectTransform fresqueRectTransform = fresque.GetComponent<RectTransform>();
            if (fresqueRectTransform != null)
            {
                largeur = fresqueRectTransform.rect.height;
                longueur = fresqueRectTransform.rect.width;
            }
        }

        // Remplir les listes col et row pour le découpage de la fresque (divisé en 4 par exemple)
        col.Clear();
        row.Clear();
        for (int i = 0; i < 4; i++)
        {
            col.Add((longueur / 4) * i);  // Positions en X
            row.Add((largeur / 4) * i);   // Positions en Y
        }
    }

    // Ajouter un fragment à la fresque
    public void AddFragmentToFresque(PuzzleFragment fragment)
    {
        // Calculer la position correcte du fragment sur la fresque
        float normalizedX = (col[fragment.col] / longueur) * fresque.transform.localScale.x;
        // Pour avoir l'origine en haut à gauche, inverser la position en Y
        float normalizedY = (row[fragment.row] / largeur) * fresque.transform.localScale.y;

        // Décalage pour que l'origine (0,0) soit en haut à gauche
        float offsetX = fresque.transform.localScale.x / 2f;
        // Inverser l'axe Y pour amener l'origine en haut à gauche
        float offsetY = fresque.transform.localScale.y / 2f;

        // Calculer la position locale en tenant compte du décalage
        Vector3 position = new Vector3(normalizedX - offsetX, offsetY - normalizedY, this.transform.position.z);

        // Appliquer la position locale au fragment
        fragment.transform.localPosition = position;

        // Calculer la taille (scale) de chaque fragment pour qu'il occupe toute la cellule
        // Diviser la taille totale de la fresque par le nombre de colonnes et de lignes
        float fragmentWidth = longueur / 4f;  // Taille en X de chaque fragment
        float fragmentHeight = largeur / 4f;  // Taille en Y de chaque fragment

        // Appliquer la scale aux fragments pour qu'ils remplissent leur cellule
        fragment.transform.localScale = new Vector3(fragmentWidth / fragment.GetComponent<Renderer>().bounds.size.x,
                                                     fragmentHeight / fragment.GetComponent<Renderer>().bounds.size.y,
                                                     1f);
    }
}
