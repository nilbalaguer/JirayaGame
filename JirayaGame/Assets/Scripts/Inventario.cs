using UnityEngine;
using System.Collections.Generic;

public class Inventario : MonoBehaviour
{
    public List<Objeto> objetos = new List<Objeto>();
    public int capacidadMaxima = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            Debug.Log("Objeto añadido al inventario. Total de objetos: " + objetos.Count);
        }
        else
        {
            Debug.Log("Inventario lleno. No se puede añadir más objetos.");
        }
    }
}
