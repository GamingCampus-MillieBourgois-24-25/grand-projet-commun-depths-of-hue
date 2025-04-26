using UnityEngine;

public class AnimScale : MonoBehaviour
{
    private static readonly int Open = Animator.StringToHash("Open");

    [Header("Property")]
    [SerializeField] private ShowMap showMap;
    [SerializeField] private Animator animator;

    public void StartMap()
    {
        animator.SetTrigger(Open);
    }
    
    public void ResetTrigger()
    {
        animator.ResetTrigger(Open);
        showMap.ClickMapIcon();
    }
}
