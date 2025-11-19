using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BombardaScript : MonoBehaviour
{
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
        if (Input.GetButtonDown("Fire1") && playerTouching)
        {
            shoting = true;
            shotingCounter = 0.1f;
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
            transform.position = Vector3.MoveTowards(transform.position, startPosition, 0.5f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            playerTouching = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            playerTouching = false;
        }
    }
}
