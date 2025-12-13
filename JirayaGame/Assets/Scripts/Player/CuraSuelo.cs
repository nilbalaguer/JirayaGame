using UnityEngine;

public class CuraSuelo : MonoBehaviour
{
    private GameManager gameManager;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            if (gameManager.vidaPlayer < 10) {
                gameManager.RecuperarVida(10);
                audioSource.Play();
                Destroy(gameObject, 1f);
            }
        }
    }
}
