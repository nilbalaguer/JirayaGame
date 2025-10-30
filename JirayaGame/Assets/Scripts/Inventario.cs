using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    public List<Objeto> objetos = new List<Objeto>();
    public int capacidadMaxima = 10;

    public GameObject inventarioUI;
    public GameObject btnPrefab;
    public Transform btnContenedorBotones;
    public StatesMachine player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventarioUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AñadirObjeto(Objeto objeto)
    {
        if (objetos.Count < capacidadMaxima)
        {
            objetos.Add(objeto);
            ActualizarInventario();
            Debug.Log("Objeto añadido al inventario. Total de objetos: " + objetos.Count);
        }
        else
        {
            Debug.Log("Inventario lleno. No se puede añadir más objetos.");
        }
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
        /*foreach (Transform child in btnContenedorBotones)
        {
            Destroy(child.gameObject);
        }*/

        foreach (Objeto obj in objetos)
        {
            GameObject btnObj = Instantiate(btnPrefab, btnContenedorBotones);
            btnObj.GetComponent<Image>().sprite = obj.icono;

            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                player.EquiparObjeto(obj);
                CerrarInventario();
            });
        }
    }

    public void EliminarObjeto(Objeto objeto)
    {
        if (objetos.Contains(objeto))
        {
            objetos.Remove(objeto);
            ActualizarInventario();        
        }
        else
        {
            Debug.Log("El objeto no está en el inventario.");
        }
    }
    
}
