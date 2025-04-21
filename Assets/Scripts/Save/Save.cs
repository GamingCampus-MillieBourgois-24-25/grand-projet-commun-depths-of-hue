using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    private string savePath;
    [SerializeField] Inventaire inventaire;

    void Awake()
    {
        savePath = Application.persistentDataPath + "/gameSave.json";
        if (inventaire == null)
        {
            Debug.LogError("Inventaire not found in the scene!");
        }
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
            /*case "player":
                saveData.playerData = new PlayerData
                {
                    // Add player-specific data here
                    position = Vector3.zero,
                    health = 100
                };
                break;*/
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
            /*playerData = new PlayerData
            {
                position = Vector3.zero,
                health = 100
            }*/
        };

        SaveToFile(saveData);
        Debug.Log($"All categories saved to: {savePath}");
    }

    public void LoadCategory(string category)
    {
        SaveData saveData = LoadExistingData();
        switch (category.Trim().ToLower())
        {
            case "inventory":
                if (saveData.inventoryData != null)
                {
                    inventaire.SetId(saveData.inventoryData.scriptableObjectIDs);
                    inventaire.AddItemSave();
                    
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

    [System.Serializable]
    private class SaveData
    {
        public InventoryData inventoryData;
        
    }

    [System.Serializable]
    private class InventoryData
    {
        public List<string> scriptableObjectIDs;
    }

    
}