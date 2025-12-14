using UnityEngine;

public class KeyHintPopupItem : MonoBehaviour
{
    [SerializeField] Canvas canva;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canva.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            Debug.Log("PlayerDEntro");
            canva.enabled = true;
        }
    }

    
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            Debug.Log("PlayerFuera");
            canva.enabled = false;
        }
    }
}