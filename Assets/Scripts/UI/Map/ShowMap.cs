using System;
using System.Collections.Generic;
using UnityEngine;

public class ShowMap : MonoBehaviour
{
    private static readonly int IsWalk = Animator.StringToHash("IsWalk");

    [Header("Show Map")]
    [SerializeField] List<GameObject> objToHide = new List<GameObject>();
    [SerializeField] List<GameObject> objToShow = new List<GameObject>();
    
    [Header("Generator Grids Gestion")]
    [SerializeField] private BackgroundGridGenerator gridGenerator;
    [SerializeField] private MapBackgroundUI mapBackgroundUI;
    [SerializeField] private Animator playerAnimator;

    private bool isOpen;

    private void Start()
    {
        isOpen = false;
    }

    private void OnEnable()
    {
        MapNavigateCadre.OnHide += ClickMapIcon;
    }

    private void OnDisable()
    {
        MapNavigateCadre.OnHide -= ClickMapIcon;
    }

    public void ClickMapIcon()
    {
        if (objToHide.Count <= 0 || objToShow.Count <= 0) return;
        foreach (var hide in objToHide) hide.SetActive(isOpen);
        foreach (var show in objToShow) show.SetActive(!isOpen);

        if (gridGenerator.backgrounds.Count > 0)
        {
            foreach (var mainGrid in gridGenerator.backgrounds) mainGrid.SetActive(isOpen);
        }
        
        if (mapBackgroundUI.backgrounds.Count > 0)
        {
            foreach (var mainGrid in mapBackgroundUI.backgrounds) mainGrid.SetActive(!isOpen);
        }

        if (isOpen && playerAnimator)
        {
            playerAnimator.SetBool(IsWalk, true);
        } 
        isOpen = !isOpen;
    }
}
