using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Misions : MonoBehaviour
{
    public enum MisionTipo
    {
        RecolectarMoneda,
        BuscarObjeto
    }
    public MisionTipo tipoMision;
    public TextMeshProUGUI texto;
    public GameObject panelMision;
    public static bool[] Mision;
    public string textoMision, textoFinalizarMision;
    public int MisionActual;
    public bool misionCompletada = false;
    public static Misions misiones;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Mision[MisionActual] = false;
        //texto.gameObject.SetActive(false);
        panelMision.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MostrarMision()
    {
        panelMision.SetActive(true);
        if (misionCompletada)
        {
            texto.text = textoFinalizarMision;
        }
        else
        {
            texto.text = textoMision;
        }
    }

    public void CompletarMision()
    {
        //Mision[MisionActual] = true;
        misionCompletada = true;
        texto.text = textoFinalizarMision;
        Invoke ("DesactivarPanel", 1f);
    }

    public void DesactivarPanel()
    {
        panelMision.SetActive(false);
    }
}
