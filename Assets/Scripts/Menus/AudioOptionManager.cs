using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionManager : MonoBehaviour
{  
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    private bool isLoad;
    public Slider MusicSlider
    {
        get { return musicSlider; }
        set { musicSlider = value; }
    }

    public Slider SoundEffectsSlider
    {
        get { return soundEffectsSlider; }
        set { soundEffectsSlider = value; }
    }

    public bool IsLoad
    {
        get { return isLoad; }
        set {  isLoad = value; }
    }

    public static float musicVolume {  get; private set; }
    public static float soundEffectsVolume { get; private set; }

    public float startMusicVolume = 5f;
    public float startSoundEffectsVolume = 5f;

    private void Start()
    {
        if (!isLoad)
        {

            musicSlider.value = startMusicVolume;
            AudioManager.Instance.UpdateMixerVolume();

            soundEffectsSlider.value = startSoundEffectsVolume;
            AudioManager.Instance.UpdateMixerVolume();
        }
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
