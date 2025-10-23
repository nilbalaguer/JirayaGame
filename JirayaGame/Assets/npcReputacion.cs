using UnityEngine;
using UnityEngine.UI;

public class npcReputacion : MonoBehaviour
{
    public Sprite caraSonriente;
    public Sprite caraNeutral;
    public Sprite caraEnfadada;
    private Image imagenReputacion;

    private int respuestaNegativa = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imagenReputacion = GetComponent<Image>();
        imagenReputacion.sprite = caraNeutral;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RespuestaNegativa()
    {
        respuestaNegativa++;
        if (respuestaNegativa == 1)
        {
            imagenReputacion.sprite = caraNeutral;
        }
        else if (respuestaNegativa == 2)
        {
            imagenReputacion.sprite = caraEnfadada;
        }
    }
    
    public void RespuestaPositiva()
    {
        respuestaNegativa--;
        if (respuestaNegativa <= 0)
        {
            respuestaNegativa = 0;
            imagenReputacion.sprite = caraSonriente;
        }
        else if (respuestaNegativa == 1)
        {
            imagenReputacion.sprite = caraNeutral;
        }
    }
}
