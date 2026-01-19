using UnityEngine;

public class FloorButonScript : MonoBehaviour
{
    public bool activado = false;
    private AudioSource audioSource;
    
    private void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("intObject"))
        {
            activado = true;
            other.transform.position = transform.position;
            audioSource.Play();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("intObject"))
        {
            activado = false;
        }
    }
}
