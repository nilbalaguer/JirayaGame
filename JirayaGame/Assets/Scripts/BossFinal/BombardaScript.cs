using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BombardaScript : MonoBehaviour
{
    public BossController bossController;
    private Transform puntoDisparo;

    [SerializeField] Animator animatorExplosion;
    [SerializeField] SpriteRenderer spriteRendererExplosion;
    [SerializeField] Light2D luzDisparo;

    private bool playerTouching = false;

    private bool shoting = false;
    private float shotingCounter = 0f;
    private AudioSource audioSource;
    [SerializeField] AudioClip shotSound;
    public float retrocesoMultiplicar = 1;

    private Vector3 startPosition;
    public bool municion = false;
    public bool polvora = false;
    [SerializeField] GameObject prefabProyectil;
    [SerializeField] Transform camera;
    [SerializeField] Transform posicionCamara;
    [SerializeField] CabezaSerpiente cabezaSerpiente;

    void Start()
    {
        puntoDisparo = GameObject.Find("puntoDisparoBombarda").GetComponent<Transform>();
        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRendererExplosion.enabled = false;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && playerTouching && polvora && municion)
        {

            shoting = true;
            shotingCounter = 0.1f;

            //Le dice al bossController que se a disparado la bombarda
            bossController.DispararBombarda();

            GameObject tempProyectil = Instantiate(prefabProyectil, puntoDisparo.position, Quaternion.identity);
            Rigidbody2D tempProyectilrb = tempProyectil.GetComponent<Rigidbody2D>();
            tempProyectilrb.AddForce(Vector2.up * 800f, ForceMode2D.Impulse);
            Destroy(tempProyectil, 30f);
        }

        if (shotingCounter >= 0.1f)
        {
            if (shotingCounter == 0.1f)
            {
                audioSource.PlayOneShot(shotSound);
                spriteRendererExplosion.enabled = true;
            }

            shotingCounter += Time.deltaTime;
            luzDisparo.intensity = shotingCounter * 50f;
            transform.Translate(-transform.up * retrocesoMultiplicar * Time.deltaTime, Space.World);

            municion = false;
            polvora = false;

            if (shotingCounter >= 0.5f)
            {
                shotingCounter = 0f;
                spriteRendererExplosion.enabled = false;
                shoting = false;
            }
        }

        if (shoting == false && luzDisparo.intensity > 0)
        {
            luzDisparo.intensity -= Time.deltaTime * 50;
        }

        if (!shoting && transform.position != startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, 1.5f * Time.deltaTime);
        }
    }

    void LateUpdate() {

        if ((playerTouching && polvora && municion) || (transform.position != startPosition))
        {
            camera.position = posicionCamara.position;

            cabezaSerpiente.disparando = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            playerTouching = true;
        }

        if (other.gameObject.name == "bolaDeCanyonObject(Clone)" && !municion)
        {
            municion = true;
            Destroy(other.gameObject);
        }

        if (other.gameObject.name == "barrilPolvora(Clone)" && !polvora)
        {
            polvora = true;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            playerTouching = false;
            cabezaSerpiente.disparando = false;
        }
    }
}
