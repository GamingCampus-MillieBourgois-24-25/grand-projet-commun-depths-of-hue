using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private bool launchTimer;
    private float timer;
    [SerializeField] private Inventaire inventaire;
    [SerializeField] private UI_Inventaire inv;
    [SerializeField] private ParticleSystem part;
    private AudioSource audioSource;
    void Start()
    {
        part.Stop();
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Appuie sur Espace pour jouer le son
        {
            PlayAudio();
        }

    }

    public void OnObjectClicked()
    {
        Debug.Log(gameObject.name + " a �t� cliqu� !");
        
        string gameObjectName = gameObject.name.ToLower();

        ItemData foundItem = inventaire.GetItems().Find(item => item.itemName.ToLower().Contains(gameObjectName));

        if (foundItem != null)
        {
            if (inventaire != null && inventaire.GetItemsData().Count < inv.spriteSlots.Count)
            {
                PlayAudio();
                inventaire.Add(foundItem);
                Destroy(gameObject);
                part.transform.position = transform.position;
                part.Play();
            }
            else
            {
                print("plus de place");
            }
        }
        else
        {
            // Si aucun �l�ment correspondant n'est trouv�
            Debug.Log("Aucun �l�ment trouv� avec un nom similaire � : " + gameObjectName);
        }
        
    }



    public void PlayAudio()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
