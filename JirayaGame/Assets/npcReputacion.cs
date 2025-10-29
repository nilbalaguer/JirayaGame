using UnityEngine;
using UnityEngine.UI;

public class npcReputacion : MonoBehaviour
{
    public Sprite caraSonriente;
    public Sprite caraNeutral;
    public Sprite caraEnfadada;
    public Image imagenReputacion;

    public Image barraReputacion;

    public Color colorPositivo = Color.green;
    public Color colorNeutral = Color.yellow;
    public Color colorNegativo = Color.red;

    public float reputacionMaxima = 100f;
    public float reputacionMinima = 0f;
    public float reputacionActual = 50f;
    //private int respuestaNegativa = 0;
    
    public float reputacionRespuestaPositiva = 10f;
    public float reputacionRespuestaNegativa = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imagenReputacion.sprite = caraNeutral;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RespuestaNegativa()
    {
        reputacionActual -= reputacionRespuestaNegativa;
        reputacionActual = Mathf.Clamp(reputacionActual, reputacionMinima, reputacionMaxima);
        ActualizarUI();
        /*respuestaNegativa++;
        if (respuestaNegativa == 1)
        {
            imagenReputacion.sprite = caraNeutral;
        }
        else if (respuestaNegativa == 2)
        {
            imagenReputacion.sprite = caraEnfadada;
        }*/
    }

    public void RespuestaPositiva()
    {
        reputacionActual += reputacionRespuestaPositiva;
        reputacionActual = Mathf.Clamp(reputacionActual, reputacionMinima, reputacionMaxima);
        ActualizarUI();
        /*respuestaNegativa--;
        if (respuestaNegativa <= 0)
        {
            respuestaNegativa = 0;
            imagenReputacion.sprite = caraSonriente;
        }
        else if (respuestaNegativa == 1)
        {
            imagenReputacion.sprite = caraNeutral;
        }*/
    }
    
    public void ActualizarUI()
    {
        float porcentajeReputacion = reputacionActual / reputacionMaxima;
        barraReputacion.fillAmount = porcentajeReputacion;

        if (porcentajeReputacion >= 0.7f)
        {
            barraReputacion.color = colorPositivo;
            imagenReputacion.sprite = caraSonriente;
        }
        else if (porcentajeReputacion >= 0.4f)
        {
            barraReputacion.color = colorNeutral;
            imagenReputacion.sprite = caraNeutral;
        }
        else
        {
            barraReputacion.color = colorNegativo;
            imagenReputacion.sprite = caraEnfadada;
        }
    }
}
