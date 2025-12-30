using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CicloDiaController : MonoBehaviour
{
    public Light2D luzGlobal;
    public CicloNoche[] ciclosDia;
    public float duracionTransicion;
    private float tiempoActualCiclo = 0f;
    private float cicloActualIndex;
    private int cicloActual = 0;
    private int cicloSiguiente = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        luzGlobal.color = ciclosDia[0].colorLuz;
    }

    // Update is called once per frame
    void Update()
    {
        tiempoActualCiclo += Time.deltaTime;
        cicloActualIndex = tiempoActualCiclo / duracionTransicion;
        if (tiempoActualCiclo >= duracionTransicion)
        {
            tiempoActualCiclo = 0f;
            cicloActual = cicloSiguiente;
            cicloSiguiente = (cicloSiguiente + 1) % ciclosDia.Length;

        }
        CambiarColor(ciclosDia[cicloActual].colorLuz, ciclosDia[cicloSiguiente].colorLuz);
    }

    private void CambiarColor(Color colorActual, Color colorSiguiente)
    {
        if (cicloActualIndex < 0 || cicloActualIndex > 1)
        {
            return;
        }
        luzGlobal.color = Color.Lerp(colorActual, colorSiguiente, cicloActualIndex);
    }
}
