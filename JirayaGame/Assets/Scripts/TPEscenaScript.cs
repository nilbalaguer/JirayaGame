using UnityEngine;
using UnityEngine.SceneManagement;

public class TPEscenaScript : MonoBehaviour
{
    [SerializeField] private string nombreEscenaObjetivo;
    [SerializeField] private Vector2 posicionObjetivoEscena;

    private GameManager gameManager;

    private void Start()
    {
        GameObject gameManagerObject = GameObject.Find("GameManager");
        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("No se encontró el objeto 'GameManager' en la escena.");
        }
    }

    public void CambiarDeEscena()
    {
        if (gameManager != null)
        {
            gameManager.CambiarEscena(nombreEscenaObjetivo, posicionObjetivoEscena);
        }
        else
        {
            Debug.LogError("No se pudo cambiar de escena porque el GameManager es nulo.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CambiarDeEscena();
        }
    }
}
