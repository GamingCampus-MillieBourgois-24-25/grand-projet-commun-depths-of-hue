using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

[Serializable]
public class DialogueData
{
    public string npc;
    public Sprite image;
    public string dialogueKey;
}

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private TextMeshProUGUI nameComponent;
    [SerializeField] private Image imageComponent;

    [SerializeField] private GameObject dialogFrame;
    [SerializeField] private AudioClip dialogSound;

    [Header("Settings")]
    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private DialogueGroup dialoguesToPreload;

    private StringTable dialogueTable;
    private int index = 0;
    private bool isBusy = false;

    // Start is called before the first frame update
    void Start()
    {
        if (dialogFrame != null)
        {
            dialogFrame.SetActive(false);
        }

        if (textComponent != null)
        {
            textComponent.text = string.Empty;
        }

        if (dialoguesToPreload != null)
        {
            LoadLocalizationFromTable(dialoguesToPreload.tableKey);
            StartCoroutine(WriteDialogue(dialoguesToPreload));
        }
    }

    public void LoadLocalizationFromTable(string tableKey)
    {
        dialogueTable = LocalizationSettings.StringDatabase.GetTable(tableKey);
    }

    public string GetDialogue(DialogueData dialogueData)
    {
        if (dialogueTable != null)
        {
            var localizedString = dialogueTable.GetEntry(dialogueData.dialogueKey);
            return localizedString != null ? localizedString.LocalizedValue : "Dialogue not found.";
        }
        else
        {
            Debug.LogWarning("Dialogue is not loaded.");
            return "Dialogue unavailable";
        }
    }

    IEnumerator TypeLine(DialogueData dialogueData)
    {
        nameComponent.text = dialogueData.npc;
        imageComponent.color = new Color(1, 1, 1, dialogueData.image == null ? 0 : 1);
        imageComponent.sprite = dialogueData.image;
        foreach (char c in GetDialogue(dialogueData))
        {
            textComponent.text += c;
            audioSource.PlayOneShot(dialogSound);

            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(1);
    }

    public IEnumerator WriteDialogue(DialogueGroup dialogueGroup)
    {
        if (isBusy)
            yield break;

        isBusy = true;
        index = 0;
        textComponent.text = string.Empty;
        dialogFrame.SetActive(true);

        foreach (DialogueData dialogue in dialogueGroup.dialogues)
        {
            yield return StartCoroutine(TypeLine(dialogue));

            index++;
            textComponent.text = string.Empty;
        }

        dialogFrame.SetActive(false);
        isBusy = false;
    }

    public bool IsBusy()
    {
        return isBusy;
    }
}
