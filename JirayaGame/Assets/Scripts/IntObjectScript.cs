using UnityEngine;
using System.Collections.Generic;

public class IntObjectScript : MonoBehaviour
{
    [Header("Sprites")]
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite spriteNormal;
    [SerializeField] Sprite spriteLengua;
    private Transform player;
    public GameObject Canvas;
    // [SerializeField] Sprite spriteRoto;

    public List<string> frutas = new List<string> {"Manzana", "Banana", "Naranja" };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.sprite = spriteNormal;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (Canvas == null){
            return;
        }
        Canvas.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Canvas == null){
            return;
        }
        
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance < 2f)
        {
            Canvas.SetActive(true);
        }
        else
        {
            Canvas.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("tongeCollider"))
        {
            spriteRenderer.sprite = spriteLengua;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("tongeCollider"))
        {
            spriteRenderer.sprite = spriteNormal;
        }
    }
}