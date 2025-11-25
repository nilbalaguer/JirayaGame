using UnityEngine;

public class CabezaSerpiente : MonoBehaviour
{
    [SerializeField] Transform punto1;
    [SerializeField] Transform punto2;
    public int vida = 10;

    private Rigidbody2D rb;

    public float speed = 3f;
    private bool intercanvio = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("nenufar"))
        {
            vida -= 2;

            Destroy(other.gameObject);

            if (vida <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
