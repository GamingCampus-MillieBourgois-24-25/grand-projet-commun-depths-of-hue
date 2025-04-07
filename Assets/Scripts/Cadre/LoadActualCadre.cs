using UnityEngine;

public class LoadActualCadre : MonoBehaviour
{
    private GestionCadre gestionCadre;

    private void Start()
    {
        GameObject foundActualCadre = GameObject.FindWithTag("ActualCadre");
        gestionCadre = foundActualCadre.GetComponent<GestionCadre>();
        gestionCadre.StockVisiblities();
    }
}
