using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Inventario : MonoBehaviour
{
    //Guardar los datos del objeto en el inventario
    [System.Serializable]
    public class InventoryEntry
    {
        public GameObject prefab;
        public string nombre;
        public Sprite icono;
        public int cantidad = 1;
        public Objeto.TipoObjeto tipo;
        public Vector3 escalaOriginal;
    }

    public List<InventoryEntry> objetos = new List<InventoryEntry>();
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
        InventoryEntry existe = objetos.Find(e => e.nombre == objeto.nombreObjeto);

        if (existe == null)
        {
            if (objetos.Count < capacidadMaxima)
            {
                InventoryEntry entry = new InventoryEntry();
                entry.prefab = objeto.gameObject;
                entry.nombre = objeto.nombreObjeto;
                entry.icono = objeto.icono;
                entry.tipo = objeto.tipo;
                entry.cantidad = objeto.cantidad > 0 ? objeto.cantidad : 1;
                entry.escalaOriginal = new Vector3(1, 1, 1);
                objetos.Add(entry);

                // Auto-equip si no tiene nada equipado
                if (player.objetoSujeto == null)
                {
                    GameObject nueva = Instantiate(entry.prefab);
                    Objeto nuevoObj = nueva.GetComponent<Objeto>();
                    nueva.transform.localScale = entry.escalaOriginal;
                    if (nuevoObj != null)
                    {
                        player.EquiparObjeto(nuevoObj);
                        EliminarObjeto(entry);
                    }
                    else
                    {
                        Debug.Log("No se pudo instanciar objeto para equipar.");
                    }
                }
            }
            else
            {
                Debug.Log("Capacidad maxima alcanzada, no se pudo añadir nueva entrada.");
            }
        }
        else
        {
            existe.cantidad++;

            if (player.objetoSujeto == null)
            {
                GameObject nuevaExist = Instantiate(existe.prefab);
                Objeto nuevoDesdeExist = nuevaExist.GetComponent<Objeto>();
                nuevaExist.transform.localScale = existe.escalaOriginal;
                if (nuevoDesdeExist != null)
                {
                    player.EquiparObjeto(nuevoDesdeExist);
                    EliminarObjeto(existe);
                }
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
                InventoryEntry entry = objetos[i];
                img.sprite = entry.icono;
                img.enabled = true;
                btnComp.interactable = true;
                if (entry.cantidad > 1)
                {
                    cantidadTexto.text = entry.cantidad.ToString();
                }
                else
                {
                    cantidadTexto.text = "";
                }

                btnComp.onClick.RemoveAllListeners();

                InventoryEntry captured = entry;
                btnComp.onClick.AddListener(() =>
                {
                    GameObject go = Instantiate(captured.prefab);
                    Objeto objInst = go.GetComponent<Objeto>();
                    go.transform.localScale = captured.escalaOriginal;
                    if (objInst != null)
                    {
                        player.EquiparObjeto(objInst);
                        EliminarObjeto(captured);
                    }
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

    public void EliminarObjeto(InventoryEntry entry)
    {
        entry.cantidad--;

        /*if (objetos.Contains(objeto))
        {
            objetos.Remove(objeto);
            ActualizarInventario();        
        }
        else
        {
            Debug.Log("El objeto no está en el inventario.");
        }*/
        if (entry.cantidad <= 0)
        {
            objetos.Remove(entry);
        }
        ActualizarInventario();
    }

    public void EliminarObjeto(Objeto objeto)
    {
        if (objeto == null) return;
        InventoryEntry entry = objetos.Find(e => e.nombre == objeto.nombreObjeto);
        if (entry != null)
        {
            EliminarObjeto(entry);
        }
        else
        {
            Debug.Log("No se ha encontrado nada para eliminar");
        }
    }
    
}