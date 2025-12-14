using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float tiempoVida = 5f;
    public int daño = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Destroy(gameObject, tiempoVida);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")){
            Enemigo1Script enemigo = other.GetComponent<Enemigo1Script>();

            if (enemigo != null)
            {
                enemigo.RecibirDaño(daño);
            }

            Destroy(gameObject);
        }
    }
}
