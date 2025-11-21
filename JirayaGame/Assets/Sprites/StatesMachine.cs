using UnityEngine;

public class StatesMachine : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    public enum State {Idle, Walk, Walk_front, Walk_back, Coger, Transforming};
    private State PlayerState;
    public float velocity;
    public Transform puntoSujecion;
    private Objeto objetoCercano;
    public Objeto objetoSujeto;
    public float fuerzaLanzamiento = 10f;
    private Inventario inventario;

    public GameObject CanvasInfo;
    public GameObject tsunadePanel;
    public GameObject tsunadePanel2;
    public bool tsunadeCerca = false;

    public ScrollPanel scrollPanel;
    public panelErmitaño panelScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        PlayerState = State.Idle;
        inventario = GetComponent<Inventario>();
        CanvasInfo.SetActive(false);
        tsunadePanel.SetActive(false);
        //tsunadePanel2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = body.linearVelocity;
        velocity = vel.magnitude;

        //Switch para cambiar de estado
        switch (PlayerState)
        {

            case State.Idle:
                if (velocity > 0.1f)
                {
                    if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y))
                    {
                        PlayerState = State.Walk;
                    }
                    else
                    {
                        if (vel.y > 0)
                            PlayerState = State.Walk_back;
                        else if (vel.y < 0)
                            PlayerState = State.Walk_front;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    PlayerState = State.Coger;
                }
                break;
            case State.Walk:
            case State.Walk_front:
            case State.Walk_back:
                if (velocity <= 0.1f)
                {
                    PlayerState = State.Idle;
                }
                else
                {
                    if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y))
                    {
                        PlayerState = State.Walk;
                    }
                    else
                    {
                        if (vel.y > 0)
                            PlayerState = State.Walk_back;
                        else if (vel.y < 0)
                            PlayerState = State.Walk_front;
                    }

                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PlayerState = State.Coger;
                }
                break;
            case State.Coger:
                if (velocity > 0.1f)
                {
                    if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y))
                    {
                        PlayerState = State.Walk;
                    }
                    else
                    {
                        if (vel.y > 0)
                            PlayerState = State.Walk_back;
                        else if (vel.y < 0)
                            PlayerState = State.Walk_front;
                    }
                }
                else
                {
                    PlayerState = State.Idle;
                }
                break;
        }

        //Switch para cambiar animacion y aplicar propiedades
        switch (PlayerState)
        {
            case State.Idle:
                animator.SetInteger("state", 0);
                break;
            case State.Walk:
                animator.SetInteger("state", 1);
                break;
            case State.Walk_front:
                animator.SetInteger("state", 2);
                break;
            case State.Walk_back:
                animator.SetInteger("state", 3);
                break;
            case State.Coger:
                animator.SetInteger("state", 6);
                break;
        }

        //si el objeto esta cogido puedo lanzarlo o guardarlo en el inventario
        if (Input.GetKeyDown(KeyCode.Space) && objetoSujeto != null)
        {
            LanzarObjeto();
        }
        else if (Input.GetKeyDown(KeyCode.G) && objetoSujeto != null)
        {
            GuardarObjeto();
        }
        //Soltar el objeto en caso de no necesitarlo
        else if (Input.GetKeyDown(KeyCode.Z) && objetoSujeto != null)
        {
            SoltarObjeto();
        }

        //Mostrar paneles tsunade al acercarme a ella
        if (tsunadeCerca && objetoSujeto != null && !objetoSujeto.esRecompensa)
        {
            tsunadePanel.SetActive(true);
            gameObject.GetComponent<movement>().puedoMoverme = false;
        }
    }


    //Coger objeto cercano
    public void CogerObjeto()
    {
        if (objetoSujeto == null && objetoCercano != null)
        {
            float distancia = Vector2.Distance(transform.position, objetoCercano.transform.position);
            float rangoDeteccion = 2f;
            if (distancia <= rangoDeteccion)
            {
                objetoSujeto = objetoCercano;
                objetoSujeto.Coger(puntoSujecion);
                CanvasInfo.SetActive(true);
                if ((objetoCercano.nombreObjeto == "PergaminoSagrado" || objetoCercano.nombreObjeto == "CollarShizune" || objetoCercano.nombreObjeto == "Flor")
                && objetoCercano.yaRecogido == false)
                {
                    CambioMapa.Instance.objetosRecogidos += 1;
                    CambioMapa.Instance.ActualizarContadorObjetos();
                    objetoCercano.yaRecogido = true;
                }
                else
                {
                    Debug.Log("El objeto ya ha sido recogido anteriormente.");
                }
            }
            else
            {
                objetoCercano = null;
                Debug.Log("El objeto está fuera de rango para ser cogido.");
            }
        }
    }

    //Lanzar objeto sujeto
    public void LanzarObjeto()
    {
        if (objetoSujeto == null)
        {
            return;
        }

    Vector2 direccion = puntoSujecion.position - transform.position;
    if (direccion.sqrMagnitude < 0.01)
    {
        if (body != null && body.linearVelocity.magnitude > 0.01f)
            direccion = body.linearVelocity.normalized;
        else
            direccion = Vector2.right; 
    }

    
    Objeto objetoLanzado = objetoSujeto;
    objetoLanzado.Lanzar(direccion, fuerzaLanzamiento);
    
    objetoSujeto = null;

        
        string nombre = objetoLanzado != null ? objetoLanzado.nombreObjeto : null;

        Inventario.InventoryEntry entrada = inventario.objetos.Find(e => e.nombre == nombre);
    if (entrada != null && entrada.cantidad > 0)
        {

            GameObject nuevaGO = Instantiate(entrada.prefab, puntoSujecion.position, puntoSujecion.rotation);
            Objeto nuevoObjeto = nuevaGO.GetComponent<Objeto>();
            if (nuevoObjeto != null)
            {
                nuevaGO.SetActive(true);

                // Equipar el nuevo objeto y decrementar la cantidad en el inventario
                EquiparObjeto(nuevoObjeto);
                inventario.EliminarObjeto(entrada);
            }
            else
            {
                Debug.Log("El objeto instanciado no tiene componente Objeto.");
                objetoSujeto = null;
            }
        }
        else
        {
            objetoSujeto = null;
            //objetoLanzado.gameObject.SetActive(false);
        }
    }

    //Guardar objeto en inventario
    public void GuardarObjeto()
    {
        inventario.AñadirObjeto(objetoSujeto);
        objetoSujeto.CogerObjeto();
        objetoSujeto = null;
        Debug.Log("Objeto guardado en inventario.");
    }

    //Equipar objeto desde inventario
    public void EquiparObjeto(Objeto objetoCercano)
    {
        if (objetoSujeto != null)
        {
            objetoSujeto.transform.SetParent(null);
            objetoSujeto.gameObject.SetActive(false);
        }
        objetoSujeto = objetoCercano;
        Debug.Log($"[StatesMachine] Equipando objeto '{objetoCercano?.nombreObjeto}'");
        objetoSujeto.gameObject.SetActive(true);
        //Resetar valores del rididbody cada vez que se equipe un objeto
        Rigidbody2D rb = objetoSujeto.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.rotation = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0;
        }
        objetoSujeto.transform.position = puntoSujecion.position;
        objetoSujeto.transform.rotation = puntoSujecion.rotation;
        
        objetoSujeto.Coger(puntoSujecion);
        objetoSujeto.transform.localScale = Vector3.one;
    }
    public void SoltarObjeto()
    {
        if (objetoSujeto != null)
        {
            objetoSujeto.Soltar();
            objetoSujeto = null;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("intObject"))
        {
            objetoCercano = collision.GetComponent<Objeto>();
        }
        if (collision.CompareTag("Tsunade"))
        {
            Debug.Log("Entraste en el rango de Tsunade");
            tsunadeCerca = true;
        }
        if (collision.CompareTag("Moneda"))
        {
            GameManager.Instance.RecolectarMonedas();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("intObject"))
        {
            if (objetoCercano != null && collision.gameObject == objetoCercano.gameObject)
            {
                objetoCercano = null;
            }
        }
        if (collision.CompareTag("Tsunade"))
        {
            Debug.Log("Saliste del rango de Tsunade");
            tsunadeCerca = false;
        }
    }

    public void AceptarEntrega()
    {
        /*objetoSujeto.Soltar();
        objetoSujeto = null;
        tsunadePanel.SetActive(false);
        Invoke ("DesactivarObjetoCercano", 0.5f);*/
        Objeto objetoEntregado = objetoSujeto;
        tsunade tsunadeScript = GameObject.FindWithTag("Tsunade").GetComponent<tsunade>();
        tsunadeScript.objetoRecibido = objetoEntregado;

        objetoSujeto.Soltar();
        objetoSujeto = null;

        objetoEntregado.gameObject.SetActive(false);
        inventario.EliminarObjeto(objetoEntregado);

        //Invoke ("DesactivarObjetoCercano", 0.5f);
    }

    public void RecibirRecompensa(Objeto recompensa)
    {
        inventario.AñadirObjeto(recompensa);
    }
}
