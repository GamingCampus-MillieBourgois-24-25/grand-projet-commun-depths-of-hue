using UnityEngine;
//
public class AnimScale : MonoBehaviour
{
    private static readonly int Open = Animator.StringToHash("Open");

    [Header("Property")]
    [SerializeField] private ShowMap showMap;
    [SerializeField] private Animator animator;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    public void StartMap()
    {
        animator.SetTrigger(Open);
        PlaySoundBtn();
    }
    
    public void ResetTrigger()
    {
        animator.ResetTrigger(Open);
        showMap.ClickMapIcon();
    }

    private void PlaySoundBtn()
    {
        audioSource.Play();
    }
}
