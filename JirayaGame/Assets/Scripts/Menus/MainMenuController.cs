using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Button playButton;

    void Start()
    {
        // Asignamos la función al evento del botón
        playButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        // Carga la escena del juego (asegúrate de agregarla en Build Settings)
        SceneManager.LoadScene("SceneNil");
    }
}
