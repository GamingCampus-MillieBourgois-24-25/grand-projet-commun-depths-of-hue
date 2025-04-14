using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Save : MonoBehaviour
{
    private string savePath;
    [SerializeField] Inventaire inventaire;

    void Awake()
    {
        savePath = Application.persistentDataPath + "/gameSave.json";
        if (inventaire == null)
        {
            Debug.LogError("DataManager not found in the scene!");
        }
        EnsureSaveFileExists();
    }

    private void EnsureSaveFileExists()
    {
        if (!System.IO.File.Exists(savePath))
        {
            SaveGame();
            Debug.Log("New save file created at: " + savePath);
        }
    }

    public void SaveGame()
    {
        var data = new SaveDataTemp
        {
            scriptableObjectIDs = inventaire.GetId(),
        };

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public void LoadGame()
    {
        string json = System.IO.File.ReadAllText(savePath);
        var data = JsonUtility.FromJson<SaveDataTemp>(json);

        /*dataManager.SetPlayerPosition(data.playerPos);*/
        inventaire.SetId(data.scriptableObjectIDs);
        inventaire.AddItemSave();
        /*dataManager.SetGameParameters(data.parameters);*/

        Debug.Log("Game loaded from: " + savePath);
    }

    public void DeleteSave()
    {
        if (System.IO.File.Exists(savePath))
        {
            System.IO.File.Delete(savePath);
            SaveGame(); // Recrée avec les valeurs par défaut
            Debug.Log("Save file deleted and recreated at: " + savePath);
        }
    }

    [System.Serializable]
    private class SaveDataTemp
    {
        public List<string> scriptableObjectIDs;
    }
}
