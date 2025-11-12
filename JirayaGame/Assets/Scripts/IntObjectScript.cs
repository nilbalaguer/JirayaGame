using UnityEngine;
using System.Collections.Generic;

public class IntObjectScript : MonoBehaviour
{
    [Header("Sprites")]
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite spriteNormal;
    [SerializeField] Sprite spriteLengua;
    // [SerializeField] Sprite spriteRoto;

    public List<string> frutas = new List<string> {"Manzana", "Banana", "Naranja" };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.sprite = spriteNormal;

    }

    // Update is called once per frame
    void Update()
    {

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