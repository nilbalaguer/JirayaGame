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

    public NpcStates npcScript;
    public bool misionActiva = false;
    private GameObject prefabRecompensa;
    public GameObject[] recompensas;
    private Objeto objetoRecompensa;
    public StatesMachine playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Mision[MisionActual] = false;
        //texto.gameObject.SetActive(false);
        panelMision.SetActive(false);
        playerScript = GameObject.Find("player").GetComponent<StatesMachine>();
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
            misionActiva = true;
            texto.text = textoMision;
            npcScript.npcIcono.sprite = npcScript.iconoIntro;
            npcScript.canvasImagen.SetActive(true);
        }
    }

    public void CompletarMision()
    {
        //Mision[MisionActual] = true;
        misionActiva = false;
        misionCompletada = true;
        texto.text = textoFinalizarMision;
        npcScript.canvasImagen.SetActive(false);
        switch (tipoMision)
        {
            case MisionTipo.RecolectarMoneda:
                prefabRecompensa = recompensas[0];
                break;
            case MisionTipo.BuscarObjeto:
                prefabRecompensa = recompensas[1];
                break;
        }
        GameObject recompensaInstanciada = Instantiate(prefabRecompensa, playerScript.puntoSujecion.position, Quaternion.identity);
        objetoRecompensa = recompensaInstanciada.GetComponent<Objeto>();
        objetoRecompensa.esRecompensa = true;
        playerScript.objetoSujeto = objetoRecompensa;
        objetoRecompensa.Coger(playerScript.puntoSujecion);
        Invoke ("DesactivarPanel", 1f);
    }

    public void DesactivarPanel()
    {
        panelMision.SetActive(false);
    }
}
