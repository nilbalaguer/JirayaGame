using UnityEngine;

public class Objeto : MonoBehaviour
{
    //public Transform puntoSujeccion;
    //private GameObject objetoSujeto;
    [HideInInspector] public bool estaSujeto = false;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Coger(Transform puntoSujecion)
    {
        estaSujeto = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        transform.position = puntoSujecion.position;
        transform.SetParent(puntoSujecion);
        
    }


    public void Lanzar(Vector2 fuerza)
    {
        transform.SetParent(null);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(fuerza, ForceMode2D.Impulse);
        rb.gravityScale = 0;
        estaSujeto = false;

        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    /*void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            objetoSujeto = collision.gameObject;
            objetoRigidbody = GetComponent<Rigidbody2D>();
            transform.position = puntoSujeccion.position;
            transform.parent = objetoSujeto.transform;
        }
    }*/
}
