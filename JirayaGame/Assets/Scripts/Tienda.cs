using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Tienda : MonoBehaviour
{
    public List<Objeto> objetos = new List<Objeto>();
    public Objeto[] objetosTienda;
    private List<GameObject> btnSlots = new List<GameObject>();

    public GameObject btnPrefab;
    public Transform btnContenedorBotones;
    public Button btnCerrarTienda;
    public BehaviourErmitaño ermitañoTienda;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < objetosTienda.Length; i++)
        {
            objetos.Add(objetosTienda[i]);
        }

        GameObject btnObj = Instantiate(btnPrefab, btnContenedorBotones);
        btnObj.SetActive(true);
        btnObj.GetComponent<Image>().enabled = false;
        btnObj.GetComponent<Button>().interactable = false;
        TextMeshProUGUI textoPrecio = btnObj.GetComponentInChildren<TextMeshProUGUI>();
        textoPrecio.gameObject.SetActive(true);
        btnSlots.Add(btnObj);

        MostrarObjetosTienda();
        PosicionarPrimerObjetoXbox();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PosicionarPrimerObjetoXbox()
    {
        if (btnSlots.Count > 0 && btnSlots[0] != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(btnSlots[0]);

            //Posicionar boton cerrar Tienda
            if ((Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.X)) && ermitañoTienda.tiendaAbierta)
            {
                Debug.Log("Posicionando boton cerrar tienda");
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(btnCerrarTienda.gameObject);
            }
        }
    }

    public void MostrarObjetosTienda()
    {
        for (int i = 0; i < btnSlots.Count; i++)
        {
            GameObject btn = btnSlots[i];
            Image img = btn.GetComponent<Image>();
            Button btnComp = btn.GetComponent<Button>();
            TextMeshProUGUI precioTexto = btn.GetComponentInChildren<TextMeshProUGUI>();

            if (i < objetos.Count)
            {
                Objeto obj = objetos[i];
                img.sprite = obj.icono;
                img.enabled = true;
                btnComp.interactable = true;
                precioTexto.text = objetos[i].precioTienda.ToString();

                btnComp.onClick.RemoveAllListeners();

                Objeto objetoCapturado = obj;
                btnComp.onClick.AddListener(() =>
                {
                    GameManager.Instance.ComprarObjetos(objetoCapturado);
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
}
