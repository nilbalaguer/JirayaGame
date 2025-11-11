using UnityEngine;

public class PartituraItemScript : MonoBehaviour
{
    private GameManager gameManager;

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
            Destroy(gameObject);
        }
    }
}
