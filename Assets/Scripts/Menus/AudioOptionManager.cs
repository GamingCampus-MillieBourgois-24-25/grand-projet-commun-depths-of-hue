using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundEffectsSlider;
    public static float musicVolume {  get; private set; }
    public static float soundEffectsVolume { get; private set; }

    public float startMusicVolume = 5f;
    public float startSoundEffectsVolume = 5f;

    private void Start()
    {
        musicSlider.value = startMusicVolume;
        AudioManager.Instance.UpdateMixerVolume();

        soundEffectsSlider.value = startSoundEffectsVolume;
        AudioManager.Instance.UpdateMixerVolume();
    }

    public void OnMusicSliderValueChange()
    {
        musicVolume = Mathf.Log10(musicSlider.value) * 20;
        AudioManager.Instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange()
    {
        soundEffectsVolume = Mathf.Log10(soundEffectsSlider.value) * 20;
        AudioManager.Instance.UpdateMixerVolume();
    }
}
