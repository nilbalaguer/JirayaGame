using UnityEngine;

public class Enemigo1Script : MonoBehaviour
{
    public int vida = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
        if (other.gameObject.CompareTag("KatanaFriend"))
        {
            vida -= 5;


            Vector2 knockbackDir = (other.transform.position - transform.position).normalized;

            float knockbackForce = 10f;
            rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        }
    }
}
