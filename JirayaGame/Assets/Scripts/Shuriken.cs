using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float tiempoVida = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
