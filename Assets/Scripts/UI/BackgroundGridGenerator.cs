using UnityEngine;

public class BackgroundGridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] imagePrefab;
    [SerializeField] private Sprite[] backgroundSprites;
    [SerializeField] private int columns = 3;
    [SerializeField] private int rows = 3;
    private Camera mainCamera;
    
    public delegate void OnFinishSpawnCadres();
    public static event OnFinishSpawnCadres OnSpawnCadre;

    private void Start()
    {
        mainCamera = Camera.main;
        Vector2 screenSize = GetScreenSizeInUnits();

        for (int i = 0; i < backgroundSprites.Length; i++)
        {
            int x = i % columns;
            int y = i / columns;

            Vector3 position = new Vector3(x * screenSize.x, -y * screenSize.y, 0);

            GameObject go = Instantiate(imagePrefab[i], position, Quaternion.identity, transform);

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.sprite = backgroundSprites[i];

            Vector3 scale = go.transform.localScale;
            scale.x = screenSize.x / sr.sprite.bounds.size.x;
            scale.y = screenSize.y / sr.sprite.bounds.size.y;
            go.transform.localScale = scale;
        }
        
        OnSpawnCadre?.Invoke();
    }

    private Vector2 GetScreenSizeInUnits()
    {
        float height = mainCamera.orthographicSize * 2f;
        float width = height * mainCamera.aspect;
        return new Vector2(width, height);
    }
}