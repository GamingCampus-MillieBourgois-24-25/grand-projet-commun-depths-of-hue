using System.Collections.Generic;
using System.Linq;
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
    
    private Dictionary<string, bool> statusMap = new Dictionary<string, bool>();
    [Header("Sauvegarde")]
    [SerializeField] private Save sauvegarde;
    private List<GestionCadre> cadres = new List<GestionCadre>();
    private List<GameObject> cadresMap = new List<GameObject>();
    private bool receiveFromBGGridGenerator = false;
    private bool receiveFromSauvegarde = false;
    
    [Header("Materials")]
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material blurMaterial;
    
    [Header("Sprites")]
    [SerializeField] private Sprite ancre;
    [SerializeField] private Sprite lockedBubble;

    private bool isOpen;

    private void Start()
    {
        isOpen = false;
        if (sauvegarde) sauvegarde.LoadCategory("mapcadre");
    }

    private void OnEnable()
    {
        MapNavigateCadre.OnHide += ClickMapIcon;
        Save.OnSaveStartPlayer += SetReceiveFromSauvegarde;
        GestionCadre.OnSendNewStatus += ModifyStatusCadre;
    }

    private void OnDisable()
    {
        MapNavigateCadre.OnHide -= ClickMapIcon;
        Save.OnSaveStartPlayer -= SetReceiveFromSauvegarde;
        GestionCadre.OnSendNewStatus -= ModifyStatusCadre;
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

        UpdateStatusCadre();
        
        isOpen = !isOpen;
    }

    private void SetReceiveFromSauvegarde()
    {
        receiveFromSauvegarde = true;
        SaveStartingPlayer();
    }

    public void SetReceiveFromBGGridGenerator(List<GestionCadre> _cadres)
    {
        receiveFromBGGridGenerator = true;
        cadres = _cadres;
        SaveStartingPlayer();
    }

    private void SaveStartingPlayer()
    {
        if (!sauvegarde) return;
        if (receiveFromSauvegarde && receiveFromBGGridGenerator)
        {
            foreach (var cadre in cadres)
            {
                statusMap.Add(cadre.gameObject.name, cadre.gameObject.CompareTag("ActualCadre"));
            }
            sauvegarde.SaveCategory("mapcadre");
        }
    }
    
    public void SetMapStatus(Dictionary<string, bool> _mapInfo)
    {
        statusMap = _mapInfo;
    }
    
    public Dictionary<string, bool> GetMapStatus()
    {
        return statusMap;
    }

    private void ModifyStatusCadre(GestionCadre _cadre)
    {
        Debug.Log("ICIICCI");
        if (!statusMap.ContainsKey(_cadre.gameObject.name)) return;
        statusMap[_cadre.gameObject.name] = true;
        Debug.Log("NEW SAVE");
        sauvegarde.SaveCategory("mapcadre");
        UpdateStatusCadre();
    }
    
    public void SetMapCadre(List<GameObject> _cadres)
    {
        cadresMap.AddRange(_cadres);
    }

    private void UpdateStatusCadre()
    {
        sauvegarde.LoadCategory("mapcadre");
        
        foreach (var t in cadresMap)
        {
            if (!t) return;
            foreach (var cadre in statusMap)
            {
                string key = cadre.Key;
                key = "Map" + key;
                
                bool value = cadre.Value;

                if (t.name == key)
                {
                    t.GetComponent<InformationCadreMap>().Material.material = value ? originalMaterial : blurMaterial;
                    t.GetComponent<InformationCadreMap>().Locked.sprite = value ? ancre : lockedBubble;
                }
            }
        }
    }
}
