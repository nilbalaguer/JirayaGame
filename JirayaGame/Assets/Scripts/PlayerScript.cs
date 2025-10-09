using UnityEngine;
using System;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float maxSpeed = 3;

    [SerializeField] TextMeshProUGUI textoVida;

    [SerializeField] TextMeshProUGUI textoPuntos;
    [SerializeField] TextMeshProUGUI textoWiner;

    public int vida = 10;

    public int puntos = 0;

    // [SerializeField] float jumpCoolDown = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float force = Input.GetAxis("Horizontal");

        if (Math.Abs(rigidBody.linearVelocityX) < maxSpeed)
        {
            rigidBody.AddForce(new Vector2(force, 0));
        }

    }

    void FixedUpdate()
    {
        if (vida <= 0)
        {
            Debug.Log("Game Over");

            textoWiner.text = "Game Over \nYou LOSE";

            Time.timeScale = 0f;
        }

        if (puntos >= 3)
        {
            Debug.Log("Felicidades Has Ganado");

            textoWiner.text = "You are a winer!!";
            
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("objetoCaida"))
        {
            vida -= 1;
            textoVida.text = "Vida: " + vida;
        }

        if (other.CompareTag("objetoPuntosPositivos"))
        {
            puntos += 1;
            textoPuntos.text = "Puntos: " + puntos;

            Destroy(other.gameObject);
        }

        if (other.CompareTag("objetoPuntosNegativos"))
        {
            puntos -= 1;
            textoPuntos.text = "Puntos: " + puntos;

            Destroy(other.gameObject);
        }
    }
}
