using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuPausa : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;
    private AudioManager audioManager;
    public Slider volumeSlider;
    public Slider sfxVolumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            float defaultMusicVolume = 0.3f;

            audioManager.SetMusicVolume(defaultMusicVolume);

            volumeSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
            //volumeSlider.value = audioManager.musicSource.volume;
            volumeSlider.value = defaultMusicVolume;

            sfxVolumeSlider.onValueChanged.AddListener(audioManager.SetSFXVolume);
            sfxVolumeSlider.value = audioManager.sfxSource.volume;
        }
        else
        {
            Debug.LogWarning("AudioManager no encontrado en la escena.");
        }
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Start"))
        {
            TogglePause();
        }

          if (isPaused && EventSystem.current.currentSelectedGameObject == volumeSlider.gameObject)
        {
            float input = Input.GetAxis("Horizontal"); 
            volumeSlider.value += input * Time.deltaTime * 10f; 
        }else if (isPaused && EventSystem.current.currentSelectedGameObject == sfxVolumeSlider.gameObject)
        {
            float input = Input.GetAxis("Horizontal"); 
            sfxVolumeSlider.value += input * Time.deltaTime * 10f; 
        }

    }

    void TogglePause()
    {
        isPaused = !isPaused;

        // Activar o desactivar el menú
        pauseMenu.SetActive(isPaused);

        // Pausar o reanudar el juego
        Time.timeScale = isPaused ? 0 : 1;
        Cursor.visible = true;
      //Cursor.visible = isPaused;
      //Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

        if (isPaused)
        {
            EventSystem.current.SetSelectedGameObject(pauseMenu.transform.GetChild(0).gameObject);
        }
    }

    public void IrAlMenu()
    {
        SceneManager.LoadScene("Menu"); 
    }
}
