using UnityEngine;
using UnityEngine.Rendering.Universal; // Necesario para Light2D

public class AntorchaLight : MonoBehaviour
{
    private Light2D light2D;
    
    [Header("Configuración del parpadeo")]
    [Tooltip("Intensidad mínima del brillo")]
    private float minIntensity = 0.5f;
    
    [Tooltip("Intensidad máxima del brillo")]
    private float maxIntensity = 2.5f;
    
    [Tooltip("Velocidad del cambio de intensidad")]
    private float flickerSpeed = 6f;

    private float targetIntensity;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        if (light2D == null)
        {
            Debug.LogError("⚠️ No se encontró un componente Light2D en " + gameObject.name);
            enabled = false;
            return;
        }

        // Comienza con una intensidad aleatoria dentro del rango
        light2D.intensity = Random.Range(minIntensity, maxIntensity);
        targetIntensity = light2D.intensity;
    }

    void Update()
    {
        // Si estamos cerca del valor objetivo, elegimos uno nuevo aleatorio
        if (Mathf.Abs(light2D.intensity - targetIntensity) < 0.05f)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }

        // Interpolamos suavemente hacia la nueva intensidad
        light2D.intensity = Mathf.Lerp(light2D.intensity, targetIntensity, Time.deltaTime * flickerSpeed);
    }
}
