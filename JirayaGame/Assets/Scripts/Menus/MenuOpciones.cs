using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MenuOpciones : MonoBehaviour
{
    private AudioManager audioManager;
    public Slider musicSlider;
    public Slider sfxSlider;

    public Toggle muteToggle;
    private float savedMusicVolume = 1f;
    private float savedSFXVolume = 1f;
    public TMP_Dropdown resolutionDropdown;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager != null)
        {
            musicSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(audioManager.SetSFXVolume);

            musicSlider.value = audioManager.musicSource.volume;

            muteToggle.onValueChanged.AddListener(ToggleMute);
        }
        else
        {
            Debug.LogWarning("AudioManager no encontrado en la escena.");
        }

        List<string> resolutions = new List<string> { "1920x1080", "1280x720", "800x600" };
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutions);

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);

        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            int savedIndex = PlayerPrefs.GetInt("ResolutionIndex");
            resolutionDropdown.value = savedIndex;
            ChangeResolution(savedIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dpadX = Input.GetAxisRaw("DPadX");
        float dpadY = Input.GetAxisRaw("DPadY");

        if (musicSlider.gameObject.activeSelf)
        {
            float input = Input.GetAxis("Horizontal");
            musicSlider.value += input * Time.deltaTime;
        }

        if (resolutionDropdown.gameObject.activeSelf && EventSystem.current.currentSelectedGameObject == resolutionDropdown.gameObject)
        {
            if ((dpadY > 0.5f) || Input.GetAxis("Vertical") > 0.5f)
            {
                resolutionDropdown.value = Mathf.Max(resolutionDropdown.value - 1, 0);
            }

            if ((dpadY < -0.5f) || Input.GetAxis("Vertical") < -0.5f)
            {
                resolutionDropdown.value = Mathf.Min(resolutionDropdown.value + 1, resolutionDropdown.options.Count - 1);
            }
        }
        
            if (muteToggle.gameObject.activeSelf && EventSystem.current.currentSelectedGameObject == muteToggle.gameObject)
        {
            if (Input.GetButtonDown("Submit")) 
            {
                muteToggle.isOn = !muteToggle.isOn; 
                ToggleMute(muteToggle.isOn);
            }
        }
    }

    void ToggleMute(bool isMuted)
    {
        if (isMuted)
        {
            savedMusicVolume = audioManager.musicSource.volume;
            savedSFXVolume = audioManager.sfxSource.volume;

            audioManager.musicSource.volume = 0;
            audioManager.sfxSource.volume = 0;
        }
        else
        {
            audioManager.musicSource.volume = savedMusicVolume;
            audioManager.sfxSource.volume = savedSFXVolume;
        }
    }
    
        void ChangeResolution(int index)
    {
        switch (index)
        {
            case 0: Screen.SetResolution(1920, 1080, Screen.fullScreen); break;
            case 1: Screen.SetResolution(1280, 720, Screen.fullScreen); break;
            case 2: Screen.SetResolution(800, 600, Screen.fullScreen); break;
        }

        PlayerPrefs.SetInt("ResolutionIndex", index); // Guardar configuración
    }
}
