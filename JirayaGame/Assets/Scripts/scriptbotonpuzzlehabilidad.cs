using UnityEngine;
using UnityEngine.Rendering.Universal;

public class scriptbotonpuzzlehabilidad : MonoBehaviour
{
    public bool botonActivado = false;

    public Light2D light2D;
    
    void Start()
    {
        light2D = GetComponentInChildren<Light2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            botonActivado = true;
            light2D.color = Color.green;
        }
    }
}
