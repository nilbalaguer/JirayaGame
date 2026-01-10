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
    public Sprite iconoLluvia;
    [Range(0,100)]
    public int probabilidadNublado = 30;
    public ParticleSystem particulasLluvia;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        luzGlobal.color = ciclosDia[0].colorLuz;
        particulasLluvia.Stop();
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
            CambiarCiclo();
            //cicloSiguiente = (cicloSiguiente + 1) % ciclosDia.Length;
        }
        CambiarColor(ciclosDia[cicloActual].colorLuz, ciclosDia[cicloSiguiente].colorLuz);
    }

    public void CambiarCiclo()
    {
        //Ciclo dia 
        if (cicloActual == 0)
        {
            iconosCiclo.sprite = iconoSol;
            if (ProbabilidadLluvia())
            {
                cicloSiguiente = 1;
            }
            else
            {
                cicloSiguiente = 2;
            }
            if (particulasLluvia.isPlaying)
            {
                particulasLluvia.Stop();
            }
        }
        //Ciclo nublado
        else if (cicloActual == 1)
        {
            iconosCiclo.sprite = iconoLluvia;
            cicloSiguiente = 2;
            if (!particulasLluvia.isPlaying)
            {
                particulasLluvia.Play();
            }
        }
        //Ciclo noche
        else if (cicloActual == 2)
        {
            iconosCiclo.sprite = iconoLuna;
            cicloSiguiente = 3;
            if (particulasLluvia.isPlaying)
            {
                particulasLluvia.Stop();
            }
        }
        //Ciclo amanecer
        else
        {
            iconosCiclo.sprite = iconoSol;
            cicloSiguiente = 0;
            if (particulasLluvia.isPlaying)
            {
                particulasLluvia.Stop();
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
        luzGlobal.color = Color.Lerp(colorActual, colorSiguiente, Mathf.Clamp01(cicloActualIndex));
    }
}
