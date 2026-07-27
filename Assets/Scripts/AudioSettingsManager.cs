using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("UI Slider")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Slider mit Funktionen verknüpfen
        if (masterSlider != null) masterSlider.onValueChanged.AddListener(SetMasterVolume);
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Standardwerte setzen (voller Sound)
        if (masterSlider != null) masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 0.75f);
        if (musicSlider != null) musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        if (sfxSlider != null) sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 0.75f);
    }

    public void SetMasterVolume(float value) => SetVolume("MasterVol", value);
    public void SetMusicVolume(float value) => SetVolume("MusicVol", value);
    public void SetSFXVolume(float value) => SetVolume("SFXVol", value);

    private void SetVolume(string parameterName, float value)
    {
        // Mathematische Umrechnung von Slider (0 bis 1) auf Mixer Dezibel (-80 bis 20)
        float dB = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        audioMixer.SetFloat(parameterName, dB);
        PlayerPrefs.SetFloat(parameterName, value); // Speichert die Einstellung
    }
}
