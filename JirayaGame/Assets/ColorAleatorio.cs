using UnityEngine;

public class ColorAleatorio : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Color color = Color.HSVToRGB(
            Random.value,          // HUE (tono distinto)
            Random.Range(0.7f, 1f), // Saturación alta
            Random.Range(0.7f, 1f)  // Brillo alto (sin negros)
        );

        spriteRenderer.color = color;
    }
}
