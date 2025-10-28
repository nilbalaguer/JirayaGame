using UnityEngine;
using UnityEngine.UI;

public class Enemigo1Script : MonoBehaviour
{
    public int vida = 10;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] CircleCollider2D playerRangeCollider;
    [SerializeField] CircleCollider2D enemyCollider;
    [SerializeField] Image healthFillImage;
    [SerializeField] float maxSpeed = 3;

    public float knockForce = 2;

    [Header("Movimiento")]

    private GameObject playerGameObject;

    private bool playerInRange = false;
    private float enemigoKnockout = 0f;
    private float enemigoKnockBack = 0f;
    private int lastMove = 1;

    [Header("Armas")]
    private CircleCollider2D katanaCollider;

    [Header("Animaciones")]
    [SerializeField] Animator enemyAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Vector2.Distance(transform.position, playerGameObject.transform.position) > 0.7)
        {
            if (enemigoKnockout <= 0)
            {
                float forceX = 0;

                float forceY = 0;

                if (transform.position.x > playerGameObject.transform.position.x)
                {
                    forceX = -1;
                    lastMove = 4;
                }
                else if (transform.position.x < playerGameObject.transform.position.x)
                {
                    forceX = 1;
                    lastMove = 3;
                }

                if (transform.position.y > playerGameObject.transform.position.y)
                {
                    forceY = -1;
                    lastMove = 2;
                }
                else if (transform.position.y < playerGameObject.transform.position.y)
                {
                    forceY = 1;
                    lastMove = 1;
                }

                Vector2 movimiento = new Vector2(forceX, forceY) * maxSpeed;

                rb.linearVelocity = movimiento;
            }
            else
            {
                enemigoKnockout -= Time.deltaTime;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (enemigoKnockBack > 0)
        {
            Vector2 knockDirection = transform.position - playerGameObject.transform.position;

            rb.linearVelocity = knockDirection * knockForce;

            enemigoKnockBack -= Time.deltaTime;

            if (enemigoKnockBack < 0)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        enemyAnimator.SetFloat("LastDirection", lastMove);
    }

    void FixedUpdate()
    {
        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("KatanaFriend") && other.IsTouching(enemyCollider))
        {
            vida -= 1;

            healthFillImage.fillAmount = Mathf.Clamp01(vida * (float)0.1);

            enemigoKnockout = 1f;
            enemigoKnockBack = 0.1f;
            
        }

        if (other.gameObject.CompareTag("Player") && other.IsTouching(playerRangeCollider))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player") && !other.IsTouching(playerRangeCollider))
        {
            playerInRange = false;
        }
    }
}
