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
    public float reputacionActual = 100f;
    //private int respuestaNegativa = 0;
    
    public float reputacionRespuestaPositiva = 20f;
    public float reputacionRespuestaNegativa = 20f;
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
    }

    public void RespuestaPositiva()
    {
        if (reputacionActual >= reputacionMaxima)
        {
            return;
        }
        reputacionActual += reputacionRespuestaPositiva;
        reputacionActual = Mathf.Clamp(reputacionActual, reputacionMinima, reputacionMaxima);
        ActualizarUI();
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
