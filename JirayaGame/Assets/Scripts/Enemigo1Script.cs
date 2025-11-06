using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Enemigo1Script : MonoBehaviour
{
    public int vida = 10;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] CircleCollider2D enemyCollider;
    [SerializeField] Image healthFillImage;
    [SerializeField] float maxSpeed = 3;
    [SerializeField] float desiredSpeed = 0;

    public float knockForce = 2;
    [SerializeField] GameObject sangre;

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

    [Header("Sonidos")]
    private AudioSource audioSource;
    [SerializeField] AudioClip sonidoDamage;
    [SerializeField] AudioClip sonidoMuerte;
    [SerializeField] AudioClip sonidoKatana;

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

        //Sonido
        audioSource = gameObject.GetComponent<AudioSource>();

        //Animator
        enemyAnimator.SetInteger("State", 1);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerGameObject.transform.position);

        if (distanceToPlayer < 4)
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

        // Nuevo sistema de ataque
        if (distanceToPlayer <= 1f)
        {
            TryAttack();
        }

        if (distanceToPlayer > 0.9f)
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
            rb.linearVelocity = Vector2.zero; // corregido (era linearVelocity)
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

        if (velocity.x > 0)
        {
            enemyAnimator.SetFloat("LastDirection", 3);
        }
        else if (velocity.x < 0)
        {
            enemyAnimator.SetFloat("LastDirection", 4);
        }

        if (velocity.y > 0)
        {
            enemyAnimator.SetFloat("LastDirection", 1);
        }
        else if (velocity.y < 0)
        {
            enemyAnimator.SetFloat("LastDirection", 2);
        }
    }


    void FixedUpdate()
    {

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

            if (vida <= 0)
            {
                audioSource.PlayOneShot(sonidoMuerte);
                Instantiate(sangre, transform.position, Quaternion.identity);
                Destroy(gameObject, 1f);
            } else
            {
                audioSource.PlayOneShot(sonidoDamage);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

    }

    void TryAttack()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        preAttackTimer -= Time.deltaTime;

        if (preAttackTimer <= 0)
        {
            // Ejecutar el ataque real
            enemyAnimator.SetInteger("State", 2);
            StartCoroutine(AttackCoroutine());
            preAttackTimer = preAttackTime;
            cooldownTimer = cooldownTime;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        katanaCollider.enabled = true;
        audioSource.PlayOneShot(sonidoKatana);
        yield return new WaitForSeconds(0.15f);
        katanaCollider.enabled = false;
        enemyAnimator.SetInteger("State", 1);

    }

}
