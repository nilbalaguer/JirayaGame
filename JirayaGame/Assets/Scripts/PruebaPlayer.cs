using UnityEngine;

public class move : MonoBehaviour
{
    private Rigidbody2D body;
    public float force;
    public float maxSpeed;
    public Rigidbody2D prefabBala;
    public Transform puntoDisparo;

    public Transform puntoSujecion;
    public float fuerzaLanzamiento = 10f;

    private Objeto objetoCercano;
    private Objeto objetoSujeto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        //body.AddForce(new Vector2(inputX * force, inputY * force));

        //body.linearVelocity = Vector2.ClampMagnitude(body.linearVelocity, maxSpeed);
        float angle = Mathf.Atan2(inputY, inputX) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        body.linearVelocity = new Vector2(inputX * maxSpeed, inputY * maxSpeed);
        body.AddForce(new Vector2(inputX * force, inputY * force));

        body.linearVelocity = Vector2.ClampMagnitude(body.linearVelocity, maxSpeed);
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objetoSujeto == null && objetoCercano != null)
            {
                float distancia = Vector2.Distance(transform.position, objetoCercano.transform.position);
                float rangoDeteccion = 2f;
                if (distancia <= rangoDeteccion)
                {
                    objetoSujeto = objetoCercano;
                    objetoSujeto.Coger(puntoSujecion);
                }
                else
                {
                    objetoCercano = null;
                    Debug.Log("El objeto está fuera de rango para ser cogido.");
                }
            }
            else if (objetoSujeto != null)
            {
                /*Vector2 direccion = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                objetoSujeto.Lanzar(direccion * fuerzaLanzamiento);
                objetoSujeto = null;*/

                Vector2 direccion = puntoSujecion.right.normalized;
                objetoSujeto.Lanzar(direccion, fuerzaLanzamiento);
                Vector2 direccion = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                objetoSujeto.Lanzar(direccion * fuerzaLanzamiento);
                objetoSujeto = null;
            }
        }


        //body.linearVelocity = new Vector2(horizontal * 5, vertical * 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ObjetoCogible"))
        {
            objetoCercano = collision.GetComponent<Objeto>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ObjetoCogible"))
        {
            if (objetoCercano != null && collision.gameObject == objetoCercano.gameObject)
            {
                objetoCercano = null;
            }
        }
    }

    /*void lanzarObjeto()
    { 
        Rigidbody2D bala = Instantiate(prefabBala, gameObject.transform.position, Quaternion.identity);
        bala.AddForce(new Vector2(10f, 0f), ForceMode2D.Impulse);
        bala.linearVelocity = Vector2.ClampMagnitude(bala.linearVelocity, 10f);
    }*/
}
