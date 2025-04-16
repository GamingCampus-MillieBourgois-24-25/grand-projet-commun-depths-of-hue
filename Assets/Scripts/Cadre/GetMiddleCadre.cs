using UnityEngine;

public class GetMiddleCadre : MonoBehaviour
{
    [SerializeField] private Transform center;
    
    public delegate void GetMiddleCadreFunc(Transform middle);
    public static event GetMiddleCadreFunc OnGetMiddleCadre;
    
    public delegate void CalculNewRotation();
    public static event CalculNewRotation OnCalculNewRotation;

    private void OnEnable()
    {
        if (center) OnGetMiddleCadre?.Invoke(center);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        OnCalculNewRotation?.Invoke();
    }
}
