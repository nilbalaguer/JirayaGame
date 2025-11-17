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
    //public Image[] npcIconos;
    //public Image panelIconoNpc;
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
    public GameObject[] panelMisionesCompletadas;
    [HideInInspector]
    public bool panelCompletadoMostrado = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Mision[MisionActual] = false;
        //texto.gameObject.SetActive(false);
        panelMision.SetActive(false);
        playerScript = GameObject.Find("player").GetComponent<StatesMachine>();
        foreach (GameObject panel in panelMisionesCompletadas)
        {
            if (panel != null)
                panel.SetActive(false);
        }
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
            /*if (npcScript.nameNpc == "campesino1"){
                panelIconoNpc.sprite = npcIconos[0].sprite;
            }else if (npcScript.nameNpc == "campesino2"){
                panelIconoNpc.sprite = npcIconos[1].sprite;
            }*/
        }
    }

    public void CompletarMision()
    {
        //npcScript.currentState = NpcStates.State.EndMision;
        misionActiva = false;
        misionCompletada = true;
        //texto.text = textoFinalizarMision;
        npcScript.canvasImagen.SetActive(false);
        if (!panelCompletadoMostrado)
        {
            switch (tipoMision)
            {
                case MisionTipo.RecolectarMoneda:
                    GameManager.Instance.monedas += 10;
                    panelMisionesCompletadas[0].SetActive(true); 
                    panelInfoManager info = panelMisionesCompletadas[0].GetComponent<panelInfoManager>();
                    info.npcScript = npcScript;
                    break;
                case MisionTipo.BuscarObjeto:
                    GameManager.Instance.monedas += 20;
                    GameManager.Instance.textoMonedas.text = GameManager.Instance.monedas.ToString();
                    panelMisionesCompletadas[1].SetActive(true); 
                    panelInfoManager info2 = panelMisionesCompletadas[1].GetComponent<panelInfoManager>();
                    info2.npcScript = npcScript;
                    break;
            }
            panelCompletadoMostrado = true;
        }
        /*switch (tipoMision)
        {
            case MisionTipo.RecolectarMoneda:
                //prefabRecompensa = recompensas[0];
                GameManager.Instance.monedas += 10;
                panelMisionesCompletadas[0].SetActive(true); 
                break;
            case MisionTipo.BuscarObjeto:
                //prefabRecompensa = recompensas[1];
                panelMisionesCompletadas[1].SetActive(true); 
                break;
        }*/
        /*GameObject recompensaInstanciada = Instantiate(prefabRecompensa, playerScript.puntoSujecion.position, Quaternion.identity);
        objetoRecompensa = recompensaInstanciada.GetComponent<Objeto>();
        objetoRecompensa.esRecompensa = true;
        playerScript.objetoSujeto = objetoRecompensa;
        objetoRecompensa.Coger(playerScript.puntoSujecion);*/
        //Invoke ("DesactivarPanel", 1f);
        panelMision.SetActive(false);
    }

    public void DesactivarPanel()
    {
        panelMision.SetActive(false);
    }
}
