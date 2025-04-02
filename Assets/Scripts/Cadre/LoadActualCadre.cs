using UnityEngine;

public class LoadActualCadre : MonoBehaviour
{
    [SerializeField] private GestionCadre gestionCadre;

    private void Start()
    {
        gestionCadre.StockVisiblities();
    }
}
