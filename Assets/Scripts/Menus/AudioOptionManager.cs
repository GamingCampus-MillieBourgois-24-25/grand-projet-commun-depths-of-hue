using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundEffectsSlider;
    public static float musicVolume {  get; private set; }
    public static float soundEffectsVolume { get; private set; }

    public float startMusicVolume = 0.5f;
    public float startSoundEffectsVolume = 0.5f;

    [SerializeField] private TextMeshProUGUI musicSliderText;
    [SerializeField] private TextMeshProUGUI soundEffectsSliderText;

    private void Start()
    {
        musicSlider.value = startMusicVolume*10;
        musicSliderText.text = musicSlider.value.ToString();
        AudioManager.Instance.UpdateMixerVolume();

        soundEffectsSlider.value = startSoundEffectsVolume*10;
        soundEffectsSliderText.text = soundEffectsSlider.value.ToString();
        AudioManager.Instance.UpdateMixerVolume();
    }

    public void OnMusicSliderValueChange()
    {
        musicVolume = musicSlider.value;
        musicSliderText.text = musicSlider.value.ToString();
        AudioManager.Instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange()
    {
        soundEffectsVolume = soundEffectsSlider.value;
        soundEffectsSliderText.text = soundEffectsSlider.value.ToString();
        AudioManager.Instance.UpdateMixerVolume();
    }
}
