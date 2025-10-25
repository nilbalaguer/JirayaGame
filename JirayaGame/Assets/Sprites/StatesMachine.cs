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
    private Inventario inventario;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        PlayerState = State.Idle;
        inventario = GetComponent<Inventario>();
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
                //animacion de coger
                CogerObjeto();
                break;
        }
    }

    public void CogerObjeto()
    {
        if (objetoSujeto == null && objetoCercano != null)
        {
            float distancia = Vector2.Distance(transform.position, objetoCercano.transform.position);
            float rangoDeteccion = 2f;
            if (distancia <= rangoDeteccion)
            {
                inventario.AñadirObjeto(objetoCercano);
                objetoCercano.CogerObjeto();
            }
            else
            {
                objetoCercano = null;
                Debug.Log("El objeto está fuera de rango para ser cogido.");
            }
        }
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
}
