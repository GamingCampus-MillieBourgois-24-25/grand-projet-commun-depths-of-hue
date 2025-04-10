using UnityEngine;
using System.IO;
using UnityEditor;

public class ImageSplitter : MonoBehaviour
{
    public Texture2D sourceImage;
    public int rows = 4;
    public int columns = 4;
    public float pieceScale = 1f;
    public float spacing = 0.1f;
    public Material pieceMaterial;

    [ContextMenu("Split Image")]
    public void SplitImage()
    {
#if UNITY_EDITOR

        if (sourceImage == null)
        {
            Debug.LogError("Aucune image source assignée !");
            return;
        }

        Debug.Log($"Texture Info - Taille: {sourceImage.width}x{sourceImage.height} | Format: {sourceImage.format} | Lisible: {sourceImage.isReadable}");

        // Création des dossiers
        string baseFolder = "Assets/Prefabs/ImagePieces/";
        string textureFolder = baseFolder + "Textures/";
        Directory.CreateDirectory(baseFolder);
        Directory.CreateDirectory(textureFolder);

        int pieceWidth = sourceImage.width / columns;
        int pieceHeight = sourceImage.height / rows;

        Debug.Log($"Taille des fragments: {pieceWidth}x{pieceHeight}");

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Debug.Log($"Traitement fragment ({x},{y})...");

                try
                {
                    // Inversion verticale (Unity lit depuis bas gauche)
                    int realY = (rows - 1) - y;

                    // 1. Création de la texture du fragment
                    Texture2D pieceTexture = new Texture2D(pieceWidth, pieceHeight, TextureFormat.RGBA32, false);
                    Color[] pixels = sourceImage.GetPixels(
                        x * pieceWidth,
                        realY * pieceHeight,
                        pieceWidth,
                        pieceHeight);

                    pieceTexture.SetPixels(pixels);
                    pieceTexture.Apply();

                    // 2. Sauvegarde de la texture en .png
                    byte[] bytes = pieceTexture.EncodeToPNG();
                    string imageAssetPath = $"{textureFolder}Piece_{x}_{y}.png";
                    File.WriteAllBytes(imageAssetPath, bytes);
                    AssetDatabase.ImportAsset(imageAssetPath);

                    // 3. Configuration pour que ce soit bien un sprite
                    TextureImporter importer = AssetImporter.GetAtPath(imageAssetPath) as TextureImporter;
                    if (importer != null)
                    {
                        importer.textureType = TextureImporterType.Sprite;
                        importer.spritePixelsPerUnit = 100;
                        importer.isReadable = true;
                        importer.alphaIsTransparency = true;
                        importer.SaveAndReimport();
                    }

                    // 4. Rechargement de la texture comme asset sprite
                    Texture2D importedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
                    Sprite pieceSprite = Sprite.Create(
                        importedTexture,
                        new Rect(0, 0, pieceWidth, pieceHeight),
                        new Vector2(0.5f, 0.5f),
                        100f);

                    // 5. Création du GameObject
                    GameObject piece = new GameObject($"Piece_{x}_{y}");
                    SpriteRenderer renderer = piece.AddComponent<SpriteRenderer>();
                    PuzzleFragment fragmentPiece = piece.AddComponent<PuzzleFragment>();
                    FragmentPickup pieceFragmentPickup = piece.AddComponent<FragmentPickup>();
                    BoxCollider box = piece.AddComponent<BoxCollider>();

                    fragmentPiece.row = y; fragmentPiece.col = x;
                    fragmentPiece.id = ($"Piece_{x}_{y}");
                    

                    // Chargement du vrai sprite asset Unity généré automatiquement
                    Sprite spriteAsset = AssetDatabase.LoadAssetAtPath<Sprite>(imageAssetPath);
                    if (spriteAsset != null)
                    {
                        renderer.sprite = spriteAsset;

                        fragmentPiece.icon = spriteAsset;
                    }
                    else
                    {
                        Debug.LogError($"Sprite introuvable pour {imageAssetPath}");
                    }


                    if (pieceMaterial != null)
                    {
                        renderer.material = pieceMaterial;
                        Debug.Log($"Matériau assigné: {pieceMaterial.name}");
                    }

                    // 6. Sauvegarde du prefab
                    string prefabPath = $"{baseFolder}Piece_{x}_{y}.prefab";
                    PrefabUtility.SaveAsPrefabAsset(piece, prefabPath);
                    Debug.Log($"Préfab sauvegardé: {prefabPath}");

                    // 7. Nettoyage
                    DestroyImmediate(piece);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Erreur sur le fragment ({x},{y}): {e.Message}");
                }
            }
        }

        Debug.Log("Découpage terminé !");
#else
        Debug.LogError("Ce script ne peut être exécuté qu'en mode éditeur !");
#endif
    }
}
