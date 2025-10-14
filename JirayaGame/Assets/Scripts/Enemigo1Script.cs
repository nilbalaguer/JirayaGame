using UnityEngine;

public class Enemigo1Script : MonoBehaviour
{
    public int vida = 10;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] CircleCollider2D playerRangeCollider;
    [SerializeField] CircleCollider2D enemyCollider;
    [SerializeField] CircleCollider2D knockBackCollider;
    [SerializeField] float maxSpeed = 3;

    private GameObject playerGameObject;

    private bool playerInRange = false;

    private bool enemigoEnKnockback = false;
    private float enemigoKnockout = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Vector2.Distance(transform.position, playerGameObject.transform.position) > 1.1)
        {
            if (enemigoKnockout <= 0)
            {
                float forceX = 0;

                float forceY = 0;

                if (transform.position.x > playerGameObject.transform.position.x)
                {
                    forceX = -1;
                }
                else if (transform.position.x < playerGameObject.transform.position.x)
                {
                    forceX = 1;
                }

                if (transform.position.y > playerGameObject.transform.position.y)
                {
                    forceY = -1;
                }
                else if (transform.position.y < playerGameObject.transform.position.y)
                {
                    forceY = 1;
                }

                Vector2 movimiento = new Vector2(forceX, forceY) * maxSpeed;

                rb.linearVelocity = movimiento;
            } else
            {
                enemigoKnockout -= Time.deltaTime;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (enemigoEnKnockback)
        {
            Vector2 direction = (transform.position - playerGameObject.transform.position).normalized;

            rb.linearVelocity = direction * 9;

            enemigoKnockout = 1f;
        }
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
        if (other.gameObject.CompareTag("KatanaFriend") && other == enemyCollider)
        {
            vida -= 1;

            enemigoEnKnockback = true;
            
        }

        if (other.gameObject.CompareTag("Player") && other.IsTouching(playerRangeCollider))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other == knockBackCollider && other.gameObject.CompareTag("KatanaFriend"))
        {
            enemigoEnKnockback = false;
        }

        if (other.gameObject.CompareTag("Player") && !other.IsTouching(playerRangeCollider))
        {
            playerInRange = false;
        }
    }
}
