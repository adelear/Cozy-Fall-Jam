using UnityEngine;
using UnityEngine.UI;

public class SettingsPage : MonoBehaviour
{

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;


    void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVol", 1);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVol", 1);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVol", 1);

        masterVolumeSlider.onValueChanged.AddListener(MasterVolume_Callback);
        sfxVolumeSlider.onValueChanged.AddListener(SFXVolume_Callback);
        musicVolumeSlider.onValueChanged.AddListener(MusicVolume_Callback);
    }

    private void MusicVolume_Callback(float value)
    {
        AudioManager.Instance.SetMixerVolume("MusicVol", musicVolumeSlider.value);
    }

    private void SFXVolume_Callback(float value)
    {
        AudioManager.Instance.SetMixerVolume("SFXVol", sfxVolumeSlider.value);
    }

    private void MasterVolume_Callback(float value)
    {
        AudioManager.Instance.SetMixerVolume("MasterVol", masterVolumeSlider.value);
    }
}