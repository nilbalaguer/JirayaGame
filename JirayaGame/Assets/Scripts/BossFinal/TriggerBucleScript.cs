using UnityEngine;

public class TriggerBucleScript : MonoBehaviour
{
    private bool detectando = false;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            detectando = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            detectando = false;
        }
    }

    public bool IsActive()
    {
        return detectando;
    }
}
