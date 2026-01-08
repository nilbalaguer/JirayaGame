using UnityEngine;

public class MonedasController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip coinSound;
    public static bool panelInternoMostrado = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
            Destroy(gameObject);
            if (!panelInternoMostrado)
            {
                PanelInterno.Instance.AbrirPanelInterno(new string[]
                {
                    "Esto parece una moneda",
                    "Me pregunto para que podrian servir."
                });
                panelInternoMostrado = true;
            }
        }
    }
}
