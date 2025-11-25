using UnityEngine;

public class PartituraItemScript : MonoBehaviour
{
    private GameManager gameManager;

    [Tooltip("Clave usada para activar i desactivar puerta")]
    public string claveTPdesactivar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            gameManager.ObtenerPartitura();
            gameManager.DesabilitarTP(claveTPdesactivar);
            Destroy(gameObject);
        }
    }
}
