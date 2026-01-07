using UnityEngine;

public class MonedasController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip coinSound;
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
            audioSource.PlayOneShot(coinSound);
            Destroy(gameObject, coinSound.length);
            PanelInterno.Instance.AbrirPanelInterno(new string[]
            {
                "Esto parece una moneda",
                "Me pregunto para que podrian servir."
            });
        }
    }
}
