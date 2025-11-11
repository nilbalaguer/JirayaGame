using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Inventario : MonoBehaviour
{
    public List<Objeto> objetos = new List<Objeto>();
    private List<GameObject> btnSlots = new List<GameObject>();
    public int capacidadMaxima = 3;

    public GameObject inventarioUI;
    public GameObject btnPrefab;
    public Transform btnContenedorBotones;
    public StatesMachine player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < capacidadMaxima; i++)
        {
            GameObject btnObj = Instantiate(btnPrefab, btnContenedorBotones);
            btnObj.SetActive(true);
            btnObj.GetComponent<Image>().enabled = false;
            btnObj.GetComponent<Button>().interactable = false;
            TextMeshProUGUI textoCantidad = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            textoCantidad.gameObject.SetActive(true);
            btnSlots.Add(btnObj);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AñadirObjeto(Objeto objeto)
    {
        Objeto existe = objetos.Find(obj => obj.nombreObjeto == objeto.nombreObjeto);
        if (objetos.Count < capacidadMaxima && existe == null)
        {
            objetos.Add(objeto);
            Debug.Log("Objeto añadido al inventario. Total de objetos: " + objetos.Count);

            if (player.objetoSujeto == null)
            {
                player.EquiparObjeto(objeto);
            }
        }
        else
        {
            //Debug.Log("Inventario lleno. No se puede añadir más objetos.");
            existe.cantidad++;

            if (player.objetoSujeto == null || player.objetoSujeto.nombreObjeto == existe.nombreObjeto)
            {
                player.EquiparObjeto(existe);
            }
        }
        ActualizarInventario();
    }

    public void MostrarInventario()
    {
        inventarioUI.SetActive(true);

    }

    public void CerrarInventario()
    {
        inventarioUI.SetActive(false);
    }

    public void ActualizarInventario()
    {  
        for (int i = 0; i < btnSlots.Count; i++)
        {
            GameObject btn = btnSlots[i];
            Image img = btn.GetComponent<Image>();
            Button btnComp = btn.GetComponent<Button>();
            TextMeshProUGUI cantidadTexto = btn.GetComponentInChildren<TextMeshProUGUI>();

            if (i < objetos.Count)
            {
                Objeto obj = objetos[i];
                img.sprite = obj.icono;
                img.enabled = true;
                btnComp.interactable = true;
                if (obj.cantidad > 1)
                {
                    cantidadTexto.text = obj.cantidad.ToString();
                }
                else
                {
                    cantidadTexto.text = "";
                }


                btnComp.onClick.RemoveAllListeners();

                Objeto objetoCapturado = obj;
                btnComp.onClick.AddListener(() =>
                {
                    player.EquiparObjeto(objetoCapturado);
                    EliminarObjeto(objetoCapturado);
                });
            }
            else
            {
                img.sprite = null;
                img.enabled = false;
                btnComp.interactable = false;
                btnComp.onClick.RemoveAllListeners();
            }
        }
    }

    public void EliminarObjeto(Objeto objeto)
    {
        objeto.cantidad--;
        /*if (objetos.Contains(objeto))
        {
            objetos.Remove(objeto);
            ActualizarInventario();        
        }
        else
        {
            Debug.Log("El objeto no está en el inventario.");
        }*/
        if (objeto.cantidad <= 0)
        {
            objetos.Remove(objeto);
        }
        ActualizarInventario();
    }
    
}
