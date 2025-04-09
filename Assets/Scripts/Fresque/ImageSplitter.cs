using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class DynamicPuzzleGenerator : MonoBehaviour
{
    [Header("Source Settings")]
    public Texture2D sourceImage;
    [Range(1, 16)] public int rows = 4;
    [Range(1, 16)] public int columns = 4;

    [Header("3D Settings")]
    public float thickness = 0.1f;
    public float scale = 0.1f;
    public float pixelsPerUnit = 100f;
    private float uiOffset;

    [Header("Output Settings")]
    public string outputFolder = "Assets/Prefabs/PuzzlePieces";

    [ContextMenu("Generate Puzzle Pieces")]
    public void GeneratePuzzlePieces()
    {
        uiOffset = -(thickness * 51f) / 100.0f;
#if UNITY_EDITOR
        if (!ValidateInputs()) return;

        PrepareOutputFolders();

        int pieceWidth = sourceImage.width / columns;
        int pieceHeight = sourceImage.height / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GeneratePuzzlePiece(x, y, pieceWidth, pieceHeight);
            }
        }

        FinalizeGeneration();
#endif
    }

    bool ValidateInputs()
    {
        if (sourceImage == null)
        {
            Debug.LogError("Source image is not assigned!");
            return false;
        }

        if (!sourceImage.isReadable)
        {
            Debug.LogError("Enable 'Read/Write' in texture import settings!");
            return false;
        }

        return true;
    }

    void PrepareOutputFolders()
    {
        Directory.CreateDirectory(outputFolder);
        Directory.CreateDirectory($"{outputFolder}/Textures");
        AssetDatabase.Refresh();
    }

    void GeneratePuzzlePiece(int x, int y, int width, int height)
    {
        string pieceName = $"Piece_{x}_{y}";
        EditorUtility.DisplayProgressBar("Generating Puzzle", pieceName, (float)(y * columns + x) / (rows * columns));

        try
        {
            // 1. Create and save texture
            string texturePath = CreateAndSaveTexture(x, y, width, height);

            // 2. Create puzzle piece GameObject
            GameObject piece = new GameObject(pieceName);

            // 3. Add 3D cube
            CreateCube(piece, width, height);

            // 4. Create and setup Canvas
            CreateDynamicCanvas(piece, texturePath, width, height);

            // 5. Add components
            AddPuzzleComponents(piece, texturePath, x, y);

            // 6. Save as prefab
            SaveAsPrefab(piece, pieceName);

            // Cleanup
            DestroyImmediate(piece);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error creating {pieceName}: {e.Message}");
        }
    }

    string CreateAndSaveTexture(int x, int y, int width, int height)
    {
        Texture2D pieceTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        pieceTexture.SetPixels(sourceImage.GetPixels(x * width, (rows - 1 - y) * height, width, height));
        pieceTexture.Apply();

        string texturePath = $"{outputFolder}/Textures/Piece_{x}_{y}.png";
        File.WriteAllBytes(texturePath, pieceTexture.EncodeToPNG());
        AssetDatabase.ImportAsset(texturePath);

        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(texturePath);
        importer.textureType = TextureImporterType.Sprite;
        importer.spritePixelsPerUnit = pixelsPerUnit;
        importer.isReadable = true;
        importer.SaveAndReimport();

        return texturePath;
    }

    void CreateCube(GameObject parent, int width, int height)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(parent.transform);

        float aspect = (float)width / height;
        cube.transform.localPosition = Vector3.zero;
        cube.transform.localScale = new Vector3(aspect * scale, scale, thickness);

        DestroyImmediate(cube.GetComponent<BoxCollider>());
    }

    void CreateDynamicCanvas(GameObject parent, string texturePath, int width, int height)
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("PieceCanvas");
        canvasObj.transform.SetParent(parent.transform);
        canvasObj.layer = LayerMask.NameToLayer("UI");

        // Setup Canvas
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        // Calculate dimensions
        float aspect = (float)width / height;
        float canvasWidth = aspect * scale;
        float canvasHeight = scale;

        // Configure Canvas RectTransform
        RectTransform canvasRT = canvasObj.GetComponent<RectTransform>();
        canvasRT.sizeDelta = new Vector2(canvasWidth, canvasHeight);
        canvasRT.localPosition = new Vector3(0, 0, 0 + uiOffset);
        canvasRT.localRotation = Quaternion.identity;
        canvasRT.localScale = Vector3.one;

        // Create Image
        GameObject imageObj = new GameObject("PieceImage");
        imageObj.transform.SetParent(canvasObj.transform);
        imageObj.transform.localPosition = new Vector3(0, 0, 0);
        imageObj.layer = LayerMask.NameToLayer("UI");

        // Setup Image
        Image image = imageObj.AddComponent<Image>();
        image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
        image.preserveAspect = true;
        image.raycastTarget = false;

        // Configure Image RectTransform
        RectTransform imageRT = imageObj.GetComponent<RectTransform>();
        imageRT.anchorMin = Vector2.zero;
        imageRT.anchorMax = Vector2.one;
        imageRT.offsetMin = Vector2.zero;
        imageRT.offsetMax = Vector2.zero;
    }

    void AddPuzzleComponents(GameObject piece, string texturePath, int x, int y)
    {
        // Add collider
        float aspect = (float)(sourceImage.width / columns) / (sourceImage.height / rows);
        BoxCollider collider = piece.AddComponent<BoxCollider>();
        collider.size = new Vector3(aspect * scale, scale, thickness);

        // Add puzzle components
        PuzzleFragment fragment = piece.AddComponent<PuzzleFragment>();
        fragment.row = y;
        fragment.col = x;
        fragment.id = $"Piece_{x}_{y}";
        fragment.icon = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);

        piece.AddComponent<FragmentPickup>();
    }

    void SaveAsPrefab(GameObject obj, string name)
    {
        string prefabPath = $"{outputFolder}/{name}.prefab";
        PrefabUtility.SaveAsPrefabAsset(obj, prefabPath);
    }

    void FinalizeGeneration()
    {
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        Debug.Log("Puzzle generation completed!");
    }
}