using UnityEngine;

public class Desvanecer : MonoBehaviour
{
    public float fadeSpeed = 1f; // Velocidad del desvanecimiento
    private SpriteRenderer spriteRenderer;
    private Color colorActual;

    void Start()
    {
        // Obtener el SpriteRenderer del objeto
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Guardar color inicial
        colorActual = spriteRenderer.color;
    }

    void Update()
    {
        if (colorActual.a > 0f)
        {
            // Reducir alpha (transparencia)
            colorActual.a -= fadeSpeed * Time.deltaTime;

            // Asignar color actualizado
            spriteRenderer.color = colorActual;
        }
    }
}
