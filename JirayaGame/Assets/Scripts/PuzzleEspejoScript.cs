using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PuzzleEspejoScript : MonoBehaviour
{
    [SerializeField] bool foco = false;
    [SerializeField] bool espejo = false;
    [SerializeField] bool final = false;
    [SerializeField] int direccion = 1;

    private bool reciviendoLuz = false;
    [SerializeField] LayerMask layerMask;
    private LineRenderer lineRenderer;

    private bool playerTouching = false;

    [Header("Sprites")]
    [SerializeField] Sprite UpSprite;
    [SerializeField] Sprite DownSprite;
    [SerializeField] Sprite RightSprite;
    [SerializeField] Sprite LeftSprite;

    [Header("Prefab")]
    [SerializeField] GameObject partituraPrefab;

    private void Start()
    {
        // Configurar LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.red;
    }

    private void Update()
    {
        lineRenderer.enabled = false;

        if (foco)
        {
            Encendido();
        }

        if (espejo && reciviendoLuz)
        {
            Encendido();
            Rotar();
        }

        if (final && reciviendoLuz)
        {
            Debug.Log("Final");
            Instantiate(partituraPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            reciviendoLuz = false;
        }
    }

    private void Rotar()
    {
        if (playerTouching && Input.GetButtonDown("Fire1"))
        {
            direccion += 1;
            if (direccion > 4)
            {
                direccion = 1;
            }
        }

        SpriteRenderer temporalRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        switch (direccion)
        {
            case 1:
                temporalRenderer.sprite = UpSprite;
                break;

            case 2:
                temporalRenderer.sprite = DownSprite;
                break;

            case 3:
                temporalRenderer.sprite = RightSprite;
                break;

            case 4:
                temporalRenderer.sprite = LeftSprite;
                break;
        }

        
    }

    private void Encendido()
    {
        float rayDistance = 7f;
        Vector2 rayDirection = Vector2.right;

        switch (direccion)
        {
            case 1: rayDirection = Vector2.up; break;
            case 2: rayDirection = Vector2.down; break;
            case 3: rayDirection = Vector2.right; break;
            case 4: rayDirection = Vector2.left; break;
        }

        Vector2 rayOrigin = (Vector2)transform.position + rayDirection * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layerMask);

        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, rayOrigin);

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);
            reciviendoLuz = true;

            if (hit.collider.CompareTag("intObject"))
            {
                PuzzleEspejoScript otroEspejo = hit.collider.GetComponent<PuzzleEspejoScript>();
                if (otroEspejo != null)
                {
                    otroEspejo.reciviendoLuz = true;
                }
            }

            if (hit.collider.CompareTag("espejoFinal"))
            {
                PuzzleEspejoScript otroEspejo = hit.collider.GetComponent<PuzzleEspejoScript>();
                if (otroEspejo != null)
                {
                    otroEspejo.reciviendoLuz = true;
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + (Vector3)(rayDirection * rayDistance));
        }

        reciviendoLuz = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
