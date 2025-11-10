using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Zona Actual
    private string ubicacion = "overworld";

    //HUD
    private TextMeshProUGUI textoVida;

    //Sonidos
    [Header("Sonidos")]
    [SerializeField] AudioClip deathSound;
    private AudioSource audioSource;
    //Vida
    public float vidaPlayer;

    //Player
    private GameObject playerGameObject;

    //Estadisticas
    private int enemiesKilled = 0;

    [Header("Sprites")]
    [SerializeField] GameObject sangrePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textoVida = GameObject.Find("TextoVida").GetComponent<TextMeshProUGUI>();
        audioSource = gameObject.GetComponent<AudioSource>();
        playerGameObject = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReducirVida(int reduccion)
    {
        vidaPlayer -= reduccion;

        textoVida.text = "Vida: " + vidaPlayer;

        if (vidaPlayer <= 0)
        {
            PlayerDie();
        }
    }

    public void AumentarVida(int incrementacion)
    {
        vidaPlayer += incrementacion;

        textoVida.text = "Vida: " + vidaPlayer;
    }

    public void PlayerDie()
    {
        audioSource.PlayOneShot(deathSound);
        Transform playerTransform = playerGameObject.transform;
        Instantiate(sangrePrefab, playerTransform.position, Quaternion.identity);

        Time.timeScale = 0f;
    }

    public void ChangeUbication(string ubi)
    {
        ubicacion = ubi;
    }
}
