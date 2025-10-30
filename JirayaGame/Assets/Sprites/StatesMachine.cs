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
    private Objeto objetoSujeto;
    public float fuerzaLanzamiento = 10f;
    private Inventario inventario;

    public GameObject CanvasInfo;
    public GameObject tsunadePanel;
    public bool tsunadeCerca = false;

    public ScrollPanel scrollPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        PlayerState = State.Idle;
        inventario = GetComponent<Inventario>();
        CanvasInfo.SetActive(false);
        tsunadePanel.SetActive(false);
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

        if (tsunadeCerca && objetoSujeto != null && !objetoSujeto.esRecompensa)
        {
            tsunadePanel.SetActive(true);
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
        Vector2 direccion = puntoSujecion.right.normalized;
        objetoSujeto.Lanzar(direccion, fuerzaLanzamiento);
        objetoSujeto = null;
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
        objetoSujeto.gameObject.SetActive(true);
        objetoSujeto.Coger(puntoSujecion);
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
        objetoSujeto.Soltar();
        objetoSujeto = null;
        tsunadePanel.SetActive(false);
        Invoke ("DesactivarObjetoCercano", 0.5f);
    }

    public void DesactivarObjetoCercano()
    {
        objetoCercano.gameObject.SetActive(false);
        inventario.EliminarObjeto(objetoCercano);
    }

    public void RecibirRecompensa(Objeto recompensa)
    {
        inventario.AñadirObjeto(recompensa);
    }
}
