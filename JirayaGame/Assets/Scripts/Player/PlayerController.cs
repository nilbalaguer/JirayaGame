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


    public int vida = 10;

    [Header("Armas")]
    [SerializeField] GameObject katanaObject;
    [SerializeField] PolygonCollider2D colliderKatana;
    [SerializeField] float cooldownMele = 0;
    [SerializeField] float cooldownForMele = 0.5f;
    private string lastMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderKatana.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float forceX = Input.GetAxis("Horizontal");
        float forceY = Input.GetAxis("Vertical");

        if (forceY > 0)
        {
            lastMove = "up";
        } else if (forceY < 0) {
            lastMove = "down";
        }

        if (forceX > 0)
        {
            lastMove = "right";
        } else if (forceX < 0) {
            lastMove = "left";
        }

        Vector2 movimiento = new Vector2(forceX, forceY) * maxSpeed;


        rigidBody.linearVelocity = movimiento;


        //Maquina de estados
        switch (state)
        {
            default:
            case "idle":
                if (forceX > 0)
                {
                    state = "MoveRight";
                }
                else if (forceX < 0)
                {
                    state = "MoveLeft";
                }

                if (forceY > 0)
                {
                    state = "MoveUp";
                }
                else if (forceY < 0)
                {
                    state = "MoveDown";
                }

                break;

            case "MoveRight":
            case "MoveLeft":
            case "MoveUp":
            case "MoveDown":
                if (forceX > 0)
                {
                    state = "MoveRight";
                }
                else if (forceX < 0)
                {
                    state = "MoveLeft";
                }

                if (forceY > 0)
                {
                    state = "MoveUp";
                }
                else if (forceY < 0)
                {
                    state = "MoveDown";
                }

                if (forceX == 0 && forceY == 0)
                {
                    state = "idle";
                }

                break;

        }

        /*
            State 0 = idle
            State 1 = Walk right
            State 2 = Walk left
            State 3 = Walk Up
            State 4 = Walk Down
        */
        
        switch (state)
        {
            default:
            case "idle":
                animator.SetInteger("State", 0);

                break;

            case "MoveRight":

                animator.SetInteger("State", 1);

                break;

            case "MoveLeft":

                animator.SetInteger("State", 2);

                break;

            case "MoveUp":

                animator.SetInteger("State", 3);

                break;

            case "MoveDown":

                animator.SetInteger("State", 4);

                break;

        }


        //Rotacion de las armas siempre igual que el ultimo movimiento del jugador
        //Activar catana por unos milisegundos
        if (Input.GetButtonDown("Fire1") && cooldownMele == 0)
        {
            cooldownMele = 0.01f;

            switch (lastMove)
            {
                case "up":
                    katanaObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case "down":
                    katanaObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case "right":
                    katanaObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case "left":
                    katanaObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;

                default:
                    katanaObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
            }
        }
        
        if (cooldownMele > 0)
        {
            colliderKatana.enabled = true;

            cooldownMele += Time.deltaTime;

            if (cooldownMele >= cooldownForMele)
            {
                cooldownMele = 0;

                colliderKatana.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("objetoCaida"))
        {
            vida -= 1;
            textoVida.text = "Vida: " + vida;
        }
    }
}