using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProyectilMagicoScript : MonoBehaviour
{
    private Transform playerTrans;
    private float scala = 1;
    private Light2D light2D;

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
        
    }

    void FixedUpdate() {
        transform.position = Vector2.MoveTowards(transform.position, playerTrans.position, 0.04f);
        scala -= 0.003f;
        transform.localScale = new Vector2(scala, scala);
        light2D.intensity = scala * 2;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
