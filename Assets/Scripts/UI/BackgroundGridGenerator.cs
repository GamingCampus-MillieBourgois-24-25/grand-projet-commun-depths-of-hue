using UnityEngine;

public class BackgroundGridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] imagePrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Sprite[] backgroundSprites;
    [SerializeField] private int columns = 3;
    [SerializeField] private int rows = 3;
    private Camera mainCamera;
    private GameObject[,] gridCadres;
    [SerializeField] private bool isForBackgroundLayer;
    public delegate void OnFinishSpawnCadres();
    public static event OnFinishSpawnCadres OnSpawnCadre;

    private void Start()
    {
        mainCamera = Camera.main;
        Vector2 screenSize = GetScreenSizeInUnits();
        
        gridCadres = new GameObject[columns, rows];

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

            gridCadres[x, y] = go;
        }

        if (isForBackgroundLayer) return;
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject cadre = gridCadres[x, y];
                if (!cadre) continue;
                
                GestionCadre gestionCadre = cadre.GetComponent<GestionCadre>();
                if (!gestionCadre) continue;

                if (cadre.CompareTag("ActualCadre") && player)
                {
                    // set le player au bon endroit (là ou le joueur a quitter)
                    player.transform.position = gestionCadre.center.position;
                    gestionCadre.SetArrowsVisibilities();
                }

                // permet de set les target cadre selon leur position dans la grid
                if (gestionCadre.ArrowLeftBool && x > 0 || gestionCadre.ArrowLeftBool && x == columns - 1 && y == rows - 1 || gestionCadre.ArrowLeftBool && x == 2 && y == 0) gestionCadre.TargetCadreLeftGO = gridCadres[x - 1, y];
                if (gestionCadre.ArrowRightBool && x < columns - 1 || gestionCadre.ArrowRightBool && x == columns - 1 && y == rows - 1 || gestionCadre.ArrowRightBool && x == 2 && y == 0) gestionCadre.TargetCadreRightGO = gridCadres[x + 1, y];
                if ((gestionCadre.ArrowDownBool && y > 0) || gestionCadre.ArrowDownBool && x == columns - 1 && y == rows - 1 || gestionCadre.ArrowDownBool && x == 2 && y == 0) gestionCadre.TargetCadreDownGO = gridCadres[x, y + 1];
                if (gestionCadre.ArrowUpBool && y < rows - 1 || gestionCadre.ArrowUpBool && x == columns - 1 && y == rows - 1 || gestionCadre.ArrowUpBool && x == 2 && y == 0) gestionCadre.TargetCadreUpGO = gridCadres[x, y - 1];
                
                OnSpawnCadre?.Invoke();
            }
        }
    }

    private Vector2 GetScreenSizeInUnits()
    {
        float height = mainCamera.orthographicSize * 2f;
        float width = height * mainCamera.aspect;
        return new Vector2(width, height);
    }
}