using UnityEngine;

public class VariantAnimCompagnon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool anim;
    
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int WalkVariant = Animator.StringToHash("WalkVariant");

    private void Start()
    {
        animator.SetTrigger(Walk);
        anim = false;
    }

    public void SwitchAnimation()
    {
        anim = !anim;
        animator.ResetTrigger(anim ? Walk : WalkVariant);
        animator.SetTrigger(anim ? WalkVariant : Walk);
    }
}
