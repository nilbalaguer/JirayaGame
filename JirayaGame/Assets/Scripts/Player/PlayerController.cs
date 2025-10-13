using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Varios")]
    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float maxSpeed = 5;

    [SerializeField] TextMeshProUGUI textoVida;


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

        animator.SetFloat("MoveX", movimiento.x);
        animator.SetFloat("MoveY", movimiento.y);

        rigidBody.linearVelocity = movimiento;


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