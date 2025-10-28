using UnityEngine;

public class Objeto : MonoBehaviour
{
    //public Transform puntoSujeccion;
    //private GameObject objetoSujeto;
    [HideInInspector] public bool estaSujeto = false;
    private Rigidbody2D rb;
    public Sprite icono;
    public GameObject Canvas;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Canvas.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    //Funciones para la mecanica de coger y lanzar objetos  
    public void Coger(Transform puntoSujecion)
    {
        estaSujeto = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        transform.position = puntoSujecion.position;
        transform.SetParent(puntoSujecion);
    }


    public void Lanzar(Vector2 direccion, float fuerza)
    {
        transform.SetParent(null);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;

        Vector2 dirNormalizada = direccion.normalized;
        rb.linearVelocity = dirNormalizada * fuerza;
        estaSujeto = false;

        Destroy(gameObject, 2f);
        Canvas.SetActive(false);
    }
    //Funcion para destruir objeto al cogerlo para guardarlo al inventario
    public void CogerObjeto()
    {
        gameObject.SetActive(false);
        Canvas.SetActive(false);
    }

    public void Soltar()
    {
        estaSujeto = false;
        transform.SetParent(null);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Canvas.SetActive(true);
    }

    public void DesactivarObjeto()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance < 2f && !estaSujeto)
        {
            Canvas.SetActive(true);
        }
        else
        {
            Canvas.SetActive(false);
        }
    }
}
