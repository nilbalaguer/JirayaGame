using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Varios")]
    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float maxSpeed = 5;

    [SerializeField] TextMeshProUGUI textoVida;

    [SerializeField] string state = "idle";
    public bool human = true;

    [Header("Armas")]
    [SerializeField] GameObject katanaObject;
    [SerializeField] PolygonCollider2D colliderKatana;
    [SerializeField] SpriteRenderer spriteRendererKatana;
    //Tonge
    // Tonge collider max extension 1.84581
    [SerializeField] GameObject toatTonge;
    private SpriteRenderer toatTongeTonge;
    private Animator tongeAnimator;
    [SerializeField] GameObject toatTongeColliderObject;
    private BoxCollider2D tongeCollider;

    private GameObject objectPicked = null;
    

    private float cooldownMele = 0;
    private float cooldownTonge = 0.75f;
    [SerializeField] float cooldownForMele = 0.5f;
    private int lastMove;

    [Header("Vida i Habilidades")]
    public int vida = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderKatana.enabled = false;
        spriteRendererKatana.enabled = false;

        //Lengua
        toatTongeTonge = toatTonge.GetComponentInChildren<SpriteRenderer>();
        tongeAnimator = toatTonge.GetComponentInChildren<Animator>();
        tongeCollider = toatTonge.GetComponentInChildren<BoxCollider2D>();
        toatTongeTonge.enabled = false;
        tongeAnimator.SetFloat("Blend", 0);
        tongeCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Sistema Katana para el cooldown i para activar i desactivar el katana collider solo por 0.1 segundos
        if (cooldownMele > 0)
        {
            if (cooldownMele == cooldownForMele)
            {
                colliderKatana.enabled = true;
                spriteRendererKatana.enabled = true;

            }
            else if (colliderKatana.enabled && cooldownMele < (cooldownForMele - 0.1))
            {
                colliderKatana.enabled = false;
            }

            cooldownMele -= Time.deltaTime;

            if (cooldownMele <= 0)
            {
                spriteRendererKatana.enabled = false;
            }
        }

        //Sistema de cooldown lengua.
        if (toatTongeTonge.enabled)
        {
            cooldownTonge -= Time.deltaTime;

            if (cooldownTonge <= 0)
            {
                toatTongeTonge.enabled = false;
                tongeCollider.enabled = false;
                cooldownTonge = 0.75f;
            }
            else
            {
                rigidBody.linearVelocity = new Vector2(0, 0);

                //NO DEJA MOVER NI HACER NADA MIENTRAS SE LANZA LA LENGUA!!!
                //No colocar nada por debajo de este return que quieres que se ejecute absolutamente siempre
                return;
            }
        }

        if (objectPicked != null)
        {
            objectPicked.transform.position = toatTongeColliderObject.transform.position;
        }
        
        //Sistema movimiento
        float forceX = Input.GetAxis("Horizontal");
        float forceY = Input.GetAxis("Vertical");

        if (forceY > 0)
        {
            lastMove = 1;
        }
        else if (forceY < 0)
        {
            lastMove = 2;
        }
        else if (forceX > 0)
        {
            lastMove = 3;
        }
        else if (forceX < 0)
        {
            lastMove = 4;
        }

        Vector2 movimiento = new Vector2(forceX, forceY) * maxSpeed;


        rigidBody.linearVelocity = movimiento;


        //Maquina de estados
        switch (state)
        {
            default:
            case "idle":
            case "MoveRight":
            case "MoveLeft":
            case "MoveUp":
            case "MoveDown":
                if (rigidBody.linearVelocity.x > 0)
                {
                    state = "MoveRight";
                }
                else if (rigidBody.linearVelocity.x < 0)
                {
                    state = "MoveLeft";
                }

                if (rigidBody.linearVelocity.y > 0)
                {
                    state = "MoveUp";
                }
                else if (rigidBody.linearVelocity.y < 0)
                {
                    state = "MoveDown";
                }

                if (rigidBody.linearVelocity.x == 0 && rigidBody.linearVelocity.y == 0 && state != "Attack")
                {
                    state = "idle";
                }

                if (Input.GetButtonDown("Fire1") && cooldownMele <= 0)
                {
                    if (objectPicked != null)
                    {
                        objectPicked = null;
                    } else
                    {
                        state = "Attack";
                    }
                    
                }

                if (Input.GetButtonDown("Jump"))
                {
                    human = !human;
                    if (objectPicked != null)
                    {
                        objectPicked = null;
                    }
                }

                break;

        }

        /*
            State 0 = idle
            State 1 = Walk right
            State 2 = Walk left
            State 3 = Walk Up
            State 4 = Walk Down
            State 5 = Attack
        */

        switch (state)
        {
            default:
            case "idle":
                animator.SetFloat("State", 0);
                animator.SetInteger("State-int", 0);

                break;

            case "MoveRight":

                animator.SetFloat("State", 1);
                animator.SetInteger("State-int", 1);

                toatTonge.transform.rotation = Quaternion.Euler(0, 0, 0);
                toatTonge.transform.localPosition = new Vector2(0, 0.11f);

                break;

            case "MoveLeft":

                animator.SetFloat("State", 2);
                animator.SetInteger("State-int", 2);

                toatTonge.transform.rotation = Quaternion.Euler(0, 0, 180);
                toatTonge.transform.localPosition = new Vector2(0, 0.11f);

                break;

            case "MoveUp":

                animator.SetFloat("State", 3);
                animator.SetInteger("State-int", 3);

                toatTonge.transform.rotation = Quaternion.Euler(0, 0, 90);
                toatTonge.transform.localPosition = new Vector2(0, 0);

                break;

            case "MoveDown":

                animator.SetFloat("State", 4);
                animator.SetInteger("State-int", 4);

                toatTonge.transform.rotation = Quaternion.Euler(0, 0, -90);
                toatTonge.transform.localPosition = new Vector2(0, 0);

                break;

            case "Attack":
                if (human)
                {
                    //Lanzar ataque katana
                    //Rotacion de las armas siempre igual que el ultimo movimiento del jugador
                    //Setea el cooldown para que empieze el ataque y retrase para el siguiente
                    animator.SetFloat("State", 5);
                    animator.SetInteger("State-int", 5);

                    cooldownMele = cooldownForMele;

                    switch (lastMove)
                    {
                        case 1:
                            katanaObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                            break;
                        case 2:
                            katanaObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                            break;
                        case 3:
                            katanaObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                            break;
                        case 4:
                            katanaObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            break;

                        default:
                            katanaObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                            break;
                    }

                    state = "idle";
                } else
                {
                    //Lanzar lengua

                    animator.SetFloat("State", 5);
                    animator.SetInteger("State-int", 5);

                    toatTongeTonge.enabled = true;
                    tongeCollider.enabled = true;
                    tongeAnimator.Play("Lengua-Right_Clip", 0, 0f);

                    state = "idle";
                }

                break;

        }

        //Sincronizar variables animator
        animator.SetFloat("LastDirection", lastMove);
        animator.SetBool("Human", human);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("objetoCaida"))
        {
            vida -= 1;
            textoVida.text = "Vida: " + vida;
        }

        if (other.CompareTag("intObject") && toatTongeTonge.enabled)
        {
            objectPicked = other.gameObject;
            
        }
    }
}