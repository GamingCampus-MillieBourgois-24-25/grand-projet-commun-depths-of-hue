using UnityEngine;

public class MapNavigateCadre : MonoBehaviour
{
    [SerializeField] private string referenceCadre;

    #region Event

    public delegate void OnHideMap();
    public static event OnHideMap OnHide;
    
    public delegate void OnMovePlayer(string _referenceCadre);
    public static event OnMovePlayer OnMove;

    #endregion

    public void ClickMapNavigate()
    {
        Debug.Log("Click Map Navigate");
        OnHide?.Invoke();
        if (referenceCadre.Length > 0) OnMove?.Invoke(referenceCadre);
    }
}
