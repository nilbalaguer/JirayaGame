using UnityEngine;

public class SonidoAgua : MonoBehaviour
{
    private AudioSource audioSource;

    void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            audioSource.Stop();
        }
    }
}
