using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float maxSpeed = 5;

    [SerializeField] TextMeshProUGUI textoVida;
    

    public int vida = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float forceX = 0;
        float forceY = 0;

        if (Input.GetKey(KeyCode.W))
        {
            forceY = 1;
        } else if (Input.GetKey(KeyCode.S)) {
            forceY = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            forceX = 1;
        } else if (Input.GetKey(KeyCode.A)) {
            forceX = -1;
        }

        Vector2 movimiento = new Vector2(forceX, forceY) * maxSpeed;

        animator.SetFloat("MoveX", movimiento.x);
        animator.SetFloat("MoveY", movimiento.y);

        rigidBody.linearVelocity = movimiento;
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