using UnityEngine;
using Mirror;
using UnityEngine.Rendering.Universal;

public class IndicadorNumeroScriptMultiplayer : NetworkBehaviour
{
    private AudioSource audioSource;
    [SerializeField] Transform indicadorTransform;
    private float posicionY;
    private float posicionX;
    public int numeroActual = 0;
    [SerializeField] Light2D light;

    private bool playerTouching = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicionY = indicadorTransform.position.y;
        posicionX = indicadorTransform.position.x;

        audioSource = gameObject.GetComponent<AudioSource>();
        light.intensity = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        //0,71 movimiento
        // if (playerTouching && Input.GetButtonDown("Fire2"))
        // {
        //     posicionY -= 0.72f;
        //     numeroActual += 1;
        //     // Debug.Log("Numero actual: " + numeroActual);
        // }
    }

    void FixedUpdate() {
        if (posicionY < indicadorTransform.position.y)
        {
            indicadorTransform.position = new Vector2(posicionX, indicadorTransform.position.y - 0.01f);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            light.intensity = 40f;
        } 
        else
        {
            audioSource.Stop();
            light.intensity = 0f;
        }

        if (indicadorTransform.localPosition.y < -3.98f)
        {
            indicadorTransform.localPosition = new Vector2(0f, 3.21f);
            posicionY = indicadorTransform.position.y;
            numeroActual = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            playerTouching = true;
            posicionY -= 0.72f;
            numeroActual += 1;
        }

        // if (other.CompareTag("KatanaFriend")) {
        //     posicionY -= 0.72f;
        //     numeroActual += 1;
        // }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            playerTouching = false;
        }
    }
}
