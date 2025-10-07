using UnityEngine;
using System;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float maxSpeed = 3;

    [SerializeField] TextMeshProUGUI textoVida;

    public int vida = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float force = Input.GetAxis("Horizontal");

        // rigidBody.AddForce(new Vector2(force, 0));

        if (Math.Abs(rigidBody.linearVelocityX) < maxSpeed)
        {
            rigidBody.AddForce(new Vector2(force, 0));
            // rigidBody.linearVelocity = rigidBody.linearVelocity.normalized * maxSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("objetoCaida"))
        {
            vida -= 1;
            textoVida.text = "Vida: " + vida;
        }
    }
}
