using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProyectilMagicoScript : MonoBehaviour
{
    private Transform playerTrans;
    private Light2D light2D;

    [SerializeField] GameObject explosion;

    private float tiempo = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTrans = GameObject.Find("Player").GetComponent<Transform>();
        Destroy(gameObject, 5f);
        light2D = gameObject.GetComponentInChildren<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;

        if (tiempo > 4.99)
        {
            GameObject temporal = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(temporal, 0.7f);
        }
    }

    void FixedUpdate() {
        transform.position = Vector2.MoveTowards(transform.position, playerTrans.position, 0.07f);
        light2D.intensity += Time.deltaTime * 2;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            GameObject temporal = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(temporal, 0.7f);
            Destroy(gameObject);
        }
    }
}
