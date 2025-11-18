using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Start"))
        {
            TogglePause();
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
