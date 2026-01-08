using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class CicloDiaController : MonoBehaviour
{
    public Light2D luzGlobal;
    public CicloNoche[] ciclosDia;
    public float duracionTransicion;
    private float tiempoActualCiclo = 0f;
    private float cicloActualIndex;
    public int cicloActual = 0;
    private int cicloSiguiente = 1;
    public Image iconosCiclo;
    public Sprite iconoSol;
    public Sprite iconoLuna;
    public ParticleSystem particulasLluvia;
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

        if (cicloActual == 0 || cicloActual == 2)
        {
            iconosCiclo.sprite = iconoSol;
            if (particulasLluvia.isPlaying)
            {
                particulasLluvia.Stop();
            }
        }
        else
        {
            iconosCiclo.sprite = iconoLuna;
            if (ProbabilidadLluvia())
            {
                if (!particulasLluvia.isPlaying)
                {
                    particulasLluvia.Play();
                }
            }
        }
    }

    public bool ProbabilidadLluvia()
    {
        int probabilidad = Random.Range(0, 100);
        if (probabilidad < 30)
        {
            return true;
        }else
        {
            return false;
        }
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
