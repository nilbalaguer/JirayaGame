using UnityEngine;

public class CambiarHabilidadActivar : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject aprenderHabilidad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.swapHabilidad = true;
            gameManager.estadosTP["mazmorraHabilidad"] = false;
            aprenderHabilidad.SetActive(false);
            
        }
    }
    
}
