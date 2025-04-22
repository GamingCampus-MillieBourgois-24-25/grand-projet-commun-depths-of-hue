using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corail : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip baseMelodySound;

    public AudioClip targetMelodySound;

    public delegate void CorailClickedHandler();
    public event CorailClickedHandler OnCorailClicked;

    [SerializeField] private Color glowColor = Color.white;
    [SerializeField] private float glowIntensity = 2f;

    private List<Material> coralMaterials = new List<Material>();
    private Coroutine glowRoutine;

    private Renderer[] coralRenderers;

    void Start()
    {
        targetMelodySound = baseMelodySound;
        audioSource = GetComponent<AudioSource>();

        // Récupère tous les Renderers dans les enfants
        coralRenderers = GetComponentsInChildren<Renderer>();


        if (coralRenderers.Length == 0)
        {
            Debug.LogError($"[Corail] Aucun Renderer trouvé dans les enfants de {gameObject.name}!");
            return;
        }

        // Stocke les materials pour les modifier plus tard
        foreach (Renderer rend in coralRenderers)
        {
            coralMaterials.Add(rend.material); // .material instancie une copie du material si nécessaire
        }
    }

    [ContextMenu("Debug")]
    public void OnObjectClicked()
    {
        OnCorailClicked?.Invoke();
    }

    public void PlaySound()
    {
        if (glowRoutine != null)
            StopCoroutine(glowRoutine);

        glowRoutine = StartCoroutine(PlaySoundWithGlow());
    }

    IEnumerator PlaySoundWithGlow()
    {
        EnableGlow();
        audioSource.PlayOneShot(targetMelodySound);
        yield return new WaitForSeconds(baseMelodySound.length);
        DisableGlow();
        targetMelodySound = baseMelodySound;
    }

    void EnableGlow()
    {
        foreach (Material mat in coralMaterials)
        {
            mat.EnableKeyword("_EMISSION");

            Color baseColor = mat.GetColor("_BaseColor"); // ou "_Color" selon ton shader
            mat.SetColor("_EmissionColor", baseColor * glowIntensity);

            StartCoroutine(PulseGlow(mat));
        }
    }

    void DisableGlow()
    {
        foreach (Material mat in coralMaterials)
        {
            mat.DisableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", Color.black);
        }
    }

    IEnumerator PulseGlow(Material mat, float pulseDuration = 1.5f, float fadeOutDuration = 1f)
    {
        float time = 0f;
        float pulseSpeed = 1f; // vitesse du pulsé
        Color baseColor = mat.GetColor("_BaseColor"); // ou "_Color" si ton shader n'a pas _BaseColor

        // Phase de pulse
        while (time < pulseDuration)
        {
            time += Time.deltaTime;
            float intensity = (Mathf.Sin(time * pulseSpeed * Mathf.PI * 2f) + 1f) / 2f;
            mat.SetColor("_EmissionColor", baseColor * Mathf.Lerp(0.5f, 1.5f, intensity));
            yield return null;
        }

        // Phase de fade-out
        Color currentColor = mat.GetColor("_EmissionColor");
        float fadeTime = 0f;
        while (fadeTime < fadeOutDuration)
        {
            fadeTime += Time.deltaTime;
            float t = fadeTime / fadeOutDuration;
            mat.SetColor("_EmissionColor", Color.Lerp(currentColor, baseColor * 0f, t));
            yield return null;
        }

        mat.DisableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.black);
    }

    public void SwapSound(AudioClip clip)
    {
        targetMelodySound = clip;
    }
}
