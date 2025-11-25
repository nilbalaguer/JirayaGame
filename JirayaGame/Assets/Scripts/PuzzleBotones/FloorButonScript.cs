using UnityEngine;

public class FloorButonScript : MonoBehaviour
{
    public bool activado = false;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("intObject"))
        {
            activado = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("intObject"))
        {
            activado = false;
        }
    }
}
