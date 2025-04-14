using UnityEngine;

public class MapBackgroundUI : MonoBehaviour
{
    [SerializeField] private GameObject[] imagePrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Sprite[] backgroundSprites;
    [SerializeField] private int columns = 3;
    [SerializeField] private int rows = 3;
    private Camera mainCamera;
    private GameObject[,] gridCadres;
    [SerializeField] private bool isForBackgroundLayer;

    private void Start()
    {
        mainCamera = Camera.main;

        Vector2 screenSize = GetScreenSizeInUnits(); // taille de l’écran en unités monde
        Vector2 cellSize = new Vector2(screenSize.x / columns, screenSize.y / rows);
        Vector2 origin = new Vector2(-screenSize.x / 2f, -screenSize.y / 2f); // coin bas gauche **relatif au centre de caméra**

        Vector3 cameraCenter = mainCamera.transform.position;
        cameraCenter.z = 0f; // on reste en 2D

        gridCadres = new GameObject[columns, rows];

        for (int i = 0; i < backgroundSprites.Length; i++)
        {
            int x = i % columns;
            int y = i / columns;
            if (y >= rows) break;

            // position dans la cellule, en partant du coin inférieur gauche du champ de vision caméra
            Vector3 localPosition = new Vector3(
                origin.x + cellSize.x * (x + 0.5f),
                origin.y + cellSize.y * (y + 0.5f),
                0f);

            Vector3 worldPosition = cameraCenter + localPosition;

            GameObject go = Instantiate(imagePrefab[i], worldPosition, Quaternion.identity, transform);

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.sprite = backgroundSprites[i];

            float scaleX = cellSize.x / sr.sprite.bounds.size.x;
            float scaleY = cellSize.y / sr.sprite.bounds.size.y;
            go.transform.localScale = new Vector3(scaleX, scaleY, 1f);

            gridCadres[x, y] = go;
        }
    }

    private Vector2 GetScreenSizeInUnits()
    {
        float height = mainCamera.orthographicSize * 2f;
        float width = height * mainCamera.aspect;
        return new Vector2(width, height);
    }
}