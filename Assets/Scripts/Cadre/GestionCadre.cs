using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GestionCadre : MonoBehaviour
{
    [SerializeField] private DeplacementPlayer player;
    public Transform center;
    
    [Header("Manage Arrows")]
    [SerializeField] private bool ArrowLeft;
    [SerializeField, HideInInspector] private GameObject targetCadreLeft;
    [SerializeField, HideInInspector] private GameObject arrowLeft;
    [SerializeField, HideInInspector] private float angleTargetLeft;
    
    [SerializeField] private bool ArrowRight;
    [SerializeField, HideInInspector] private GameObject targetCadreRight;
    [SerializeField, HideInInspector] private GameObject arrowRight;
    [SerializeField, HideInInspector] private float angleTargetRight;
    
    [SerializeField] private bool ArrowUp;
    [SerializeField, HideInInspector] private GameObject targetCadreUp;
    [SerializeField, HideInInspector] private GameObject arrowUp;
    [SerializeField, HideInInspector] private float angleTargetUp;
    
    [SerializeField] private bool ArrowDown;
    [SerializeField, HideInInspector] private GameObject targetCadreDown;
    [SerializeField, HideInInspector] private GameObject arrowDown;
    [SerializeField, HideInInspector] private float angleTargetDown;
    
    Dictionary<GameObject, bool> arrowsVisibilities = new Dictionary<GameObject, bool>();
    Dictionary<GameObject, bool> stockCadreTarget = new Dictionary<GameObject, bool>();

    private void Start()
    {
        StockTargetCadre();
    }

    public void StockVisiblities()
    {
        if (!arrowsVisibilities.ContainsKey(arrowLeft) || !arrowsVisibilities.ContainsValue(ArrowLeft)) arrowsVisibilities.Add(arrowLeft, ArrowLeft);
        if (!arrowsVisibilities.ContainsKey(arrowRight) || !arrowsVisibilities.ContainsValue(ArrowRight)) arrowsVisibilities.Add(arrowRight, ArrowRight);
        if (!arrowsVisibilities.ContainsKey(arrowUp) || !arrowsVisibilities.ContainsValue(ArrowUp)) arrowsVisibilities.Add(arrowUp, ArrowUp);
        if (!arrowsVisibilities.ContainsKey(arrowDown) || !arrowsVisibilities.ContainsValue(ArrowDown)) arrowsVisibilities.Add(arrowDown, ArrowDown);

        foreach(KeyValuePair<GameObject, bool> visibility in arrowsVisibilities)
        {
            GameObject arrow = visibility.Key;
            bool isVisible = visibility.Value;

            if (!isVisible)
            {
                arrow.SetActive(false);
                continue;
            }
            arrow.SetActive(true);
        }
    }
    
    private void StockTargetCadre()
    {
        if (targetCadreLeft) stockCadreTarget.Add(targetCadreLeft, ArrowLeft);
        if (targetCadreRight) stockCadreTarget.Add(targetCadreRight, ArrowRight);
        if (targetCadreUp) stockCadreTarget.Add(targetCadreUp, ArrowUp);
        if (targetCadreDown) stockCadreTarget.Add(targetCadreDown, ArrowDown);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        Debug.Log("player enter in the cadre : " + gameObject.name);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        Debug.Log("player exit in the cadre : " + gameObject.name);
    }

    private void ResetArrows()
    {
        foreach(KeyValuePair<GameObject, bool> visibility in arrowsVisibilities)
        {
            GameObject arrow = visibility.Key;
            bool isVisible = visibility.Value;

            if (isVisible)
            {
                arrow.SetActive(false);
            }
        }
    }

    public void NavigateCadre(GameObject _original)
    {
        Debug.Log(_original.name);
        ResetArrows();
        
        foreach(KeyValuePair<GameObject, bool> targetCadre in stockCadreTarget)
        {
            GameObject cadre = targetCadre.Key;
            
            if(!arrowsVisibilities[_original]) continue;
            
            if (_original == arrowsVisibilities[_original])
            {
                Debug.Log("good: " + _original.name);
                player.SetPlayerDestination(cadre.transform.TransformPoint(cadre.GetComponent<GestionCadre>().center.localPosition));
                cadre.GetComponent<GestionCadre>().StockVisiblities();
                player.MovePlayer();
            }
        }
    }
}
