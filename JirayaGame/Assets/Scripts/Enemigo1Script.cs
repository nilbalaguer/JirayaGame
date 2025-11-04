using UnityEngine;
using UnityEngine.UI;

public class Enemigo1Script : MonoBehaviour
{
    public int vida = 10;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] CircleCollider2D enemyCollider;
    [SerializeField] Image healthFillImage;
    [SerializeField] float maxSpeed = 3;
    [SerializeField] float desiredSpeed = 0;

    public float knockForce = 2;

    [Header("Movimiento")]

    private GameObject playerGameObject;

    private bool playerInRange = false;
    private float enemigoKnockout = 0f;
    private float enemigoKnockBack = 0f;

    [Header("Armas")]
    [SerializeField] CircleCollider2D katanaCollider;
    [SerializeField] float preAttackTime = 0.2f;
    [SerializeField] float cooldownTime = 0.4f;
    private float cooldownTimer;
    private float preAttackTimer;

    [Header("Animaciones")]
    [SerializeField] Animator enemyAnimator;

    [Header("NavMeshAgent")]
    private UnityEngine.AI.NavMeshAgent agent;
    public Transform target;

    [Header("Patrulla")]
    [SerializeField] Transform puntoA;
    [SerializeField] Transform puntoB;
    private Transform destinoActual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        katanaCollider.enabled = false;

        cooldownTimer = cooldownTime;
        preAttackTimer = preAttackTime;

        //Opciones NavMeshAgent
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        destinoActual = puntoA;

        agent.speed = desiredSpeed;

        agent.acceleration = 10000f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerGameObject.transform.position) < 4)
        {
            playerInRange = true;
            target = playerGameObject.transform;
            desiredSpeed = maxSpeed;
        }
        else
        {
            playerInRange = false;
            target = destinoActual;
            desiredSpeed = 2;

        }
        
        if (Vector2.Distance(transform.position, playerGameObject.transform.position) < 1)
        {
            AtaqueKatana();
        } else
        {
            preAttackTimer = preAttackTime;
        }

        if (Vector2.Distance(transform.position, playerGameObject.transform.position) > 0.9)
        {
            if (enemigoKnockout <= 0)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
                agent.speed = desiredSpeed;
                
            }
            else
            {
                agent.isStopped = true;
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

        //Animator
        Vector2 velocity = agent.velocity;

        enemyAnimator.SetFloat("speedX", velocity.x);
        enemyAnimator.SetFloat("speedY", velocity.y);
    }

    void FixedUpdate()
    {
        if (vida <= 0)
        {
            Destroy(gameObject);
        }

        if (Vector2.Distance(transform.position, destinoActual.position) < 0.2)
        {
            destinoActual = destinoActual == puntoA ? puntoB : puntoA;
        }


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("KatanaFriend") && other.IsTouching(enemyCollider))
        {
            vida -= 1;

            healthFillImage.fillAmount = Mathf.Clamp01(vida * (float)0.1);

            enemigoKnockout = 0.4f;
            enemigoKnockBack = 0.1f;
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

    }

    void AtaqueKatana()
    {
        preAttackTimer -= Time.deltaTime;
        katanaCollider.enabled = false;

        if (preAttackTimer <= 0)
        {
            //Atacar activando el collider de katana
            katanaCollider.enabled = true;
            preAttackTimer = preAttackTime;
        }
    }
}
