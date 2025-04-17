using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    private string savePath;
    [SerializeField] Inventaire inventaire;
    [SerializeField] ShowMap showMap;

    #region Event

    public delegate void SaveStartGamePlayer();
    public static event SaveStartGamePlayer OnSaveStartPlayer;
    
    public delegate void SaveStartGameActualCadre();
    public static event SaveStartGameActualCadre OnSaveStartActualCadre;

    #endregion

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/gameSave.json";
        // if (inventaire == null)
        // {
        //     Debug.LogError("Inventaire not found in the scene!");
        // }
        EnsureSaveFileExists();
    }

    private void EnsureSaveFileExists()
    {
        if (!File.Exists(savePath))
        {
            SaveAllCategories();
            Debug.Log("New save file created at: " + savePath);
        }
    }

    public void SaveCategory(string category)
    {
        SaveData saveData = LoadExistingData();

        switch (category.ToLower())
        {
            case "inventory":
                saveData.inventoryData = new InventoryData
                {
                    scriptableObjectIDs = inventaire.GetId()
                };
                break;
            // Add more categories as needed
            case "mapcadre":
                saveData.mapData = new MapData
                {
                    mapInfo = ConvertDictToList(showMap.GetMapStatus())
                };
                break;
            case "explorationcadre":
                saveData.cadreData = new CadreData
                {
                    actualCadre = showMap.ActualCadre
                };
                break;
            default:
                Debug.LogWarning($"Category {category} not recognized!");
                return;
        }

        SaveToFile(saveData);
        Debug.Log($"Category {category} saved to: {savePath}");
    }

    public void SaveAllCategories()
    {
        SaveData saveData = new SaveData
        {
            inventoryData = new InventoryData
            {
                scriptableObjectIDs = inventaire.GetId()
            },
            mapData = new MapData
            {
                mapInfo = ConvertDictToList(showMap.GetMapStatus())
            },
            cadreData = new CadreData
            {
                actualCadre = showMap.ActualCadre
            }
        };

        SaveToFile(saveData);
        Debug.Log($"All categories saved to: {savePath}");
    }

    public void LoadCategory(string category)
    {
        SaveData saveData = LoadExistingData();
        Debug.Log("Contenu du saveData : " + JsonUtility.ToJson(saveData, true));
        switch (category.Trim().ToLower())
        {
            case "inventory":
                if (saveData.inventoryData != null)
                {
                    print(saveData.inventoryData.scriptableObjectIDs);
                    inventaire.SetId(saveData.inventoryData.scriptableObjectIDs);
                    inventaire.AddItemSave();
                    
                }
                break;
            case "mapcadre":
                if (saveData.mapData != null)
                {
                    if (saveData.mapData.mapInfo == null || saveData.mapData.mapInfo.Count == 0)
                    {
                        Debug.Log("New Save Cadre");
                        OnSaveStartPlayer?.Invoke();
                    }
                    else
                    {
                        showMap.SetMapStatus(ConvertListToDict(saveData.mapData.mapInfo));
                    }
                }
                break;
            case "explorationcadre":
                if (saveData.cadreData != null)
                {
                    if (string.IsNullOrEmpty(saveData.cadreData.actualCadre))
                    {
                        Debug.Log("New Save Actual Cadre");
                        OnSaveStartActualCadre?.Invoke();
                    }
                    else
                    {
                        print(saveData.cadreData.actualCadre);
                        showMap.SetActualCadre(saveData.cadreData.actualCadre);
                    }
                }
                break;
            default:
                Debug.LogWarning($"Category {category} not recognized!");
                return;
        }

        Debug.Log($"Category {category} loaded from: {savePath}");
    }

    public void LoadAllCategories()
    {
        SaveData saveData = LoadExistingData();

        if (saveData.inventoryData != null)
        {
            inventaire.SetId(saveData.inventoryData.scriptableObjectIDs);
            inventaire.AddItemSave();
        }

        /*if (saveData.playerData != null)
        {
            // Apply player data here
            // Example: dataManager.SetPlayerPosition(saveData.playerData.position);
        }*/

        Debug.Log($"All categories loaded from: {savePath}");
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            SaveAllCategories();
            Debug.Log("Save file deleted and recreated at: " + savePath);
        }
    }

    private SaveData LoadExistingData()
    {

        if (!File.Exists(savePath))
        {
            return new SaveData(); // <-- vide !
        }
        
        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<SaveData>(json);
    }

    private void SaveToFile(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
    }
    
    private List<SerializableKeyValuePair> ConvertDictToList(Dictionary<string, bool> dict)
    {
        var list = new List<SerializableKeyValuePair>();
        foreach (var kvp in dict)
        {
            list.Add(new SerializableKeyValuePair { key = kvp.Key, value = kvp.Value });
        }
        return list;
    }

    private Dictionary<string, bool> ConvertListToDict(List<SerializableKeyValuePair> list)
    {
        var dict = new Dictionary<string, bool>();
        foreach (var kvp in list)
        {
            dict[kvp.key] = kvp.value;
        }
        return dict;
    }

    [System.Serializable]
    private class SaveData
    {
        public InventoryData inventoryData;
        public MapData mapData;
        public CadreData cadreData;
    }

    [System.Serializable]
    private class InventoryData
    {
        public List<string> scriptableObjectIDs;
    }

    #region Serialize Map Cadre Data

    [System.Serializable]
    public class SerializableKeyValuePair
    {
        public string key;
        public bool value;
    }

    [System.Serializable]
    private class MapData
    {
        public List<SerializableKeyValuePair> mapInfo;
    }

    #endregion

    #region Serialize Cadre Spawn Player

    [System.Serializable]
    private class CadreData
    {
        public string actualCadre;
    }

    #endregion
}