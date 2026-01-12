using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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
    public int capacidadMaxima = 5;

    public GameObject inventarioUI;
    public GameObject btnPrefab;
    public Transform btnContenedorBotones;
    //public StatesMachine player;
    public PlayerController player;
    private bool navegacionActiva = false;
    private int indiceSeleccionActual = 0;

    public bool modoEntrega = false;
    private tsunade tsunadeScript;
    public ScrollPanel panelTsunade;

    //navegacion xbox modo entrega
    [HideInInspector]
    public int indiceSeleccionEntrega = 0;
    private float dpadCooldown = 0.2f;
    private float dpadTimer = 0f;
    private float lastDpadY = 0f;

    public GameObject usarBoton;
    private InventoryEntry objetoUsable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < capacidadMaxima; i++)
        {
            GameObject btnObj = Instantiate(btnPrefab, btnContenedorBotones);
            btnObj.SetActive(true);
            btnObj.GetComponent<Image>().enabled = false;
            btnObj.GetComponent<Button>().interactable = false;
            Transform cursor = btnObj.transform.Find("Flecha");
            cursor.gameObject.SetActive(false);
            TextMeshProUGUI textoCantidad = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            textoCantidad.gameObject.SetActive(true);
            btnSlots.Add(btnObj);
        }
        tsunadeScript = GameObject.FindWithTag("Tsunade").GetComponent<tsunade>();
        usarBoton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float dpadX = Input.GetAxisRaw("DPadX");
        float dpadY = Input.GetAxisRaw("DPadY");
        //Manejar inventario xbox controller
        //if (Input.GetButtonDown("Fire2"))
        if (Input.GetButtonDown("RB"))
        {
            if (!navegacionActiva)
            {
                navegacionActiva = true;
                indiceSeleccionActual = 0;
                dpadTimer = 0f;
                MostrarVisualInicial();
            }
            else
            {
                navegacionActiva = false;
                OcultarVisual();
            }
        }

        if (!navegacionActiva) return;
        if (usarBoton.activeSelf) return;

        dpadTimer -= Time.deltaTime;

        if (dpadTimer <= 0f)
        {
            if (dpadY > 0.5f && lastDpadY <= 0.5f)
            {
                CambiarSeleccion(1);
                dpadTimer = dpadCooldown;
            }
            else if (dpadY < -0.5f && lastDpadY >= -0.5f)
            {
                CambiarSeleccion(-1);
                dpadTimer = dpadCooldown;
            }
        }
        lastDpadY = dpadY;

        if (Input.GetButtonDown("Submit")){
            EquiparSeleccionado();
        }
        
        if (Input.GetButtonDown("B"))
        {
            if (usarBoton.activeSelf)
            {
                CancelarUso();
            }else{
                navegacionActiva = false;
                OcultarVisual();
            }
        }
    }

    //Funciones navegacion inventario xbox controller
    void OcultarVisual()
    {
        foreach (GameObject btn in btnSlots)
        {
            Image img = btn.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
            btn.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        }
    }
    void MostrarVisualInicial()
    {
        for (int i = 0; i < btnSlots.Count; i++)
        {
            GameObject btn = btnSlots[i];
            Image img = btn.GetComponent<Image>();
            RectTransform rt = btn.GetComponent<RectTransform>();
            Transform cursor = btn.transform.Find("Flecha");

            if (i < objetos.Count)
            {
                // Mostrar todos los objetos
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
                rt.localScale = new Vector3(1,1,1);

                if (cursor != null)
                {
                    cursor.gameObject.SetActive(false);
                }    
            }
            else
            {

                img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
                rt.localScale = new Vector3(1,1,1);

                if (cursor != null)
                {
                    cursor.gameObject.SetActive(false);
                }
            }
        }

        ActualizarVisual();
    }

    //Mostrar botones seleccionados con el mando, con una flechita al lado de cada boton
    void ActualizarVisual()
    {
        for (int i = 0; i < btnSlots.Count; i++)
        {
            GameObject btn = btnSlots[i];
            Image img = btn.GetComponent<Image>();
            RectTransform rt = btn.GetComponent<RectTransform>();
            Transform cursor = btn.transform.Find("Flecha");
            if (i == indiceSeleccionActual)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
                rt.localScale = new Vector3(2,2,2);
                if (cursor != null)
                {
                    cursor.gameObject.SetActive(true);
                }
            }
            else
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
                rt.localScale = new Vector3(1,1,1);
                if (cursor != null)
                {
                    cursor.gameObject.SetActive(false);
                }
            }
        }
    }

    void CambiarSeleccion(int direccion)
    {
        indiceSeleccionActual += direccion;
        if (indiceSeleccionActual < 0)
        {
            indiceSeleccionActual = objetos.Count - 1;
        }
        else if (indiceSeleccionActual >= objetos.Count)
        {
            indiceSeleccionActual = 0;
        }
        ActualizarVisual();
    }

    void EquiparSeleccionado()
    {
        if (indiceSeleccionActual >= 0 && indiceSeleccionActual < objetos.Count)
        {
            InventoryEntry entry = objetos[indiceSeleccionActual];
            GameObject go = Instantiate(entry.prefab);
            Objeto objInst = go.GetComponent<Objeto>();
            go.transform.localScale = entry.escalaOriginal;
            if (objInst != null && objInst.tipo == Objeto.TipoObjeto.Recompensa)
            {
                //player.EquiparObjeto(objInst);
                //EliminarObjeto(entry);
                objetoUsable = entry;
                GameObject slot = btnSlots[indiceSeleccionActual];
                Vector3 botonPos = slot.transform.position;
                
                usarBoton.transform.position = botonPos;
                usarBoton.SetActive(true);
                EventSystem.current.SetSelectedGameObject(usarBoton);
            }else{
                player.SoltarObjetoInventario(objInst);
                EliminarObjeto(entry);
            }
            ActualizarInventario();
            ActualizarVisual();
        }
    }

    //Funcionamiento inventario
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
                entry.escalaOriginal = objeto.transform.localScale;
                objetos.Add(entry);

                // Auto-equip si no tiene nada equipado y el shuriken es el objeto añadido
                if (player.objetoSujeto == null && entry.nombre == "Shuriken")
                {
                    GameObject nueva = Instantiate(entry.prefab);
                    Objeto nuevoObj = nueva.GetComponent<Objeto>();
                    nueva.transform.localScale = entry.escalaOriginal;
                    if (nuevoObj != null)
                    {
                        player.EquiparObjeto(nuevoObj);
                        //EliminarObjeto(entry);
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

            if (player.objetoSujeto == null && existe.nombre == "Shuriken")
            {
                GameObject nuevaExist = Instantiate(existe.prefab);
                Objeto nuevoDesdeExist = nuevaExist.GetComponent<Objeto>();
                nuevaExist.transform.localScale = existe.escalaOriginal;
                if (nuevoDesdeExist != null)
                {
                    player.EquiparObjeto(nuevoDesdeExist);
                    //EliminarObjeto(existe);
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

    //Mostrar botones actualizados en el inventario
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
                    if (modoEntrega)
                    {
                        NavegarEntregaXbox();
                        //modo entrega para tsunade
                        GameObject objeto = Instantiate(captured.prefab);
                        Objeto objInstanciado = objeto.GetComponent<Objeto>();
                        tsunadeScript.objetoRecibido = objInstanciado;
                        
                        tsunadeScript.entregado = true;
                        panelTsunade.entregarObjeto = true;
                        panelTsunade.animator.SetTrigger("Close");

                        EliminarObjeto(captured);

                        //tsunadeScript.MostrarDialogo();

                        modoEntrega = false;
                        panelTsunade.flecha.SetActive(false);

                        return;
                    }
                    GameObject go = Instantiate(captured.prefab);
                    Objeto objInst = go.GetComponent<Objeto>();
                    go.transform.localScale = captured.escalaOriginal;
                    if (objInst != null && objInst.tipo == Objeto.TipoObjeto.Recompensa)
                    {
                        //player.EquiparObjeto(objInst);
                        //EliminarObjeto(captured);
                        //Mostrar boton usar
                        objetoUsable = captured;
                        usarBoton.transform.position = btn.transform.position;
                        usarBoton.SetActive(true);
                    }
                    else
                    {
                        player.SoltarObjetoInventario(objInst);
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

    public void Usar()
    {
        player.BeberPocion(objetoUsable.nombre);
        //eliminar pocion del inventario
        EliminarObjeto(objetoUsable);
        objetoUsable = null;
        usarBoton.SetActive(false);
        ActualizarInventario();

        indiceSeleccionActual = 0;
        ActualizarVisual();
    }

    public void CancelarUso()
    {
        objetoUsable = null;
        usarBoton.SetActive(false);

        ActualizarVisual();
    }

    //Eliminar objeto del inventario

    public void EliminarObjeto(InventoryEntry entry)
    {
        entry.cantidad--;

        if (entry.cantidad <= 0)
        {
            objetos.Remove(entry);
        }
        ActualizarInventario();
    }

    //Eliminar objeto del inventario por referencia al objeto
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

    //Comprobar que los objetos entregables a tsunade existen en el inventario
    public List<InventoryEntry> ObtenerEntregables()
    {
        List<InventoryEntry> entregables = new List<InventoryEntry>();

        foreach (var entry in objetos)
        {
            if (entry.tipo == Objeto.TipoObjeto.PergaminoSagrado ||
                entry.tipo == Objeto.TipoObjeto.Flor ||
                entry.tipo == Objeto.TipoObjeto.CollarShizune)
            {
                entregables.Add(entry);
            }
        }
        return entregables;
    }

    //Navegacion xbox para el modo entrega
    public void NavegarEntregaXbox()
    {
        dpadTimer -= Time.deltaTime;

        float dpadX = Input.GetAxis("DPadX");
        float dpadY = Input.GetAxisRaw("DPadY");
        if (dpadTimer <= 0f)
        {
            if (dpadY > 0.5f && lastDpadY <= 0.5f)
            {
                if (dpadY > 0.5f)
                {
                    indiceSeleccionActual++;
                    ActualizarVisual();
                }
                else if (dpadY < -0.5f && lastDpadY >= -0.5f)
                {
                    indiceSeleccionActual--;
                    ActualizarVisual();
                }

                indiceSeleccionActual = Mathf.Clamp(indiceSeleccionActual, 0, objetos.Count - 1);

                dpadTimer = dpadCooldown;
            }
        }
        lastDpadY = dpadY;

        if (Input.GetButtonDown("Submit"))
        {
            EntregarObjetoSeleccionado();
        }
    }

    private void EntregarObjetoSeleccionado()
    {
        InventoryEntry entry = objetos[indiceSeleccionActual];

        GameObject objeto = Instantiate(entry.prefab);
        Objeto objInstanciado = objeto.GetComponent<Objeto>();
        tsunadeScript.objetoRecibido = objInstanciado;

        EliminarObjeto(entry);


        modoEntrega = false;
    }
    
}