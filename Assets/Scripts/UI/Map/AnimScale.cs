using System;
using System.Collections.Generic;
using System.Linq;
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
    
    [Header("Pause")]
    [SerializeField] private List<GameObject> objToHide;
    [SerializeField] private GameObject pauseCanvas;

    private bool isForMap;
    private bool show;

    #region Event

    public delegate void SendArrowsToArrowsTrigger(bool _canUpdate);
    public static event SendArrowsToArrowsTrigger OnSendArrowsToArrowsTrigger;
    
    public delegate void CanUpdateGestionInputs(bool _canUpdate);
    public static event CanUpdateGestionInputs OnCanUpdateGestionInputs;
    
    public delegate void ShowArrowsHideden();
    public static event ShowArrowsHideden OnShowArrowsHidden;

    #endregion

    private void Start()
    {
        show = false;
    }
    
    public void SetAnimator(Animator _animator)
    {
        animator = _animator;
    }

    public void StartMap(bool _isForMap)
    {
        isForMap = _isForMap;
        animator.SetTrigger(Open);
    }
    
    public void ResetTrigger()
    {
        animator.ResetTrigger(Open);
        if (isForMap)
        {
            showMap.ClickMapIcon();
        }
        else
        {
            if (show)
            {
                OnShowArrowsHidden?.Invoke();
            }
            else
            {
                OnSendArrowsToArrowsTrigger?.Invoke(show);
            }
            show = !show;
            OnCanUpdateGestionInputs?.Invoke(show);
            objToHide.ForEach(obj => obj.SetActive(!show));
            pauseCanvas.SetActive(show);
        }
    }

    public void PlaySoundBtn()
    {
        audioSource.Play();
    }
}
