using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class CabezaSerpiente : MonoBehaviour
{
    [SerializeField] Transform punto1;
    [SerializeField] Transform punto2;
    public int vida = 10;

    private Rigidbody2D rb;

    public float speed = 3f;
    private bool intercanvio = false;
    private bool moviendo = true;
    public bool disparando = false;

    //OBtener player
    private GameObject player;
    private GameManager gameManager;

    //Obtener luz para laser
    [SerializeField] Light2D laserLuz;
    [SerializeField] SpriteRenderer spriteLuzRespaldo;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip cargandoLaserSonido;
    [SerializeField] AudioClip disparoSonido;

    [Header("Animaciones")]
    [SerializeField] Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        player = GameObject.Find("Player");

        laserLuz.enabled = false;
        spriteLuzRespaldo.enabled = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moviendo)
        {
            if (!intercanvio)
            {
                rb.MovePosition(Vector2.MoveTowards(
                    transform.position,
                    punto1.position,
                    speed * Time.deltaTime));

                if (Vector2.Distance(punto1.position, transform.position) < 0.1f)
                {
                    intercanvio = true;
                }
            }
            else
            {
                rb.MovePosition(Vector2.MoveTowards(
                    transform.position,
                    punto2.position,
                    speed * Time.deltaTime));

                if (Vector2.Distance(punto2.position, transform.position) < 0.1f)
                {
                    intercanvio = false;
                }
            }
        }

        if (transform.position.x +0.5f >= player.transform.position.x && transform.position.x -0.5f <= player.transform.position.x && !disparando)
        {
            Debug.Log("Player Detectado");

            disparando = true;
            moviendo = false;

            StartCoroutine(disparar());
        }
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("nenufar"))
        {
            vida -= 1;

            Destroy(other.gameObject);
            animator.Play("serpiente_damage_Clip", 0, 0f);

            if (vida <= 0)
            {
                animator.Play("serpiente_muerte_Clip", 0, 0f);
                moviendo = false;
                Destroy(gameObject, 1f);
            }
        }
    }

    IEnumerator disparar()
    {
        animator.Play("serpiente_angry_Clip", 0, 0f);

        float timerAdicion = 1f;

        laserLuz.enabled = true;
        spriteLuzRespaldo.enabled = true;

        audioSource.Play();

        while(timerAdicion > 0)
        {
            timerAdicion -= Time.deltaTime;
            laserLuz.shapeLightFalloffSize = timerAdicion;

            audioSource.pitch = -timerAdicion * 4;

            yield return null;
        }

        laserLuz.enabled = false;
        spriteLuzRespaldo.enabled = false;

        timerAdicion = 1f;

        if (transform.position.x +0.5f >= player.transform.position.x && transform.position.x -0.5f <= player.transform.position.x)
        {
            gameManager.ReducirVida(1);
        }

        audioSource.pitch = 1;

        audioSource.Stop();
        audioSource.PlayOneShot(disparoSonido);

        disparando = false;
        moviendo = true;
    }
}
