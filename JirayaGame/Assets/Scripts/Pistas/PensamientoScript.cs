using UnityEngine;
using TMPro;

public class PensamientoScript : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI textoPensamiento;
    [Header("Texto a mostrar")]
    [SerializeField] string textoAMostrar = "Mente Vacia";

    void Start() {
        canvas.enabled = false;
        textoPensamiento.text = textoAMostrar;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            canvas.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            canvas.enabled = false;
        }
    }
}
