using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Misions : MonoBehaviour
{
    public enum MisionTipo
    {
        RecolectarMoneda,
        BuscarObjeto,
        HablarConNpc,
        Ninguna
    }
    public MisionTipo tipoMision;
    public TextMeshProUGUI texto;
    public Sprite[] npcIconos;
    public Image panelIconoNpc;
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
    //public StatesMachine playerScript;
    public PlayerController playerScript;
    public GameObject[] panelMisionesCompletadas;
    public string npcDestino;
    public GameObject notaPrefab;
    [HideInInspector]
    public bool panelCompletadoMostrado = false;
    public Sprite iconoMisionKama;
    public int objetivoMonedas;
    [HideInInspector]
    public int monedasMin = 5;
    [HideInInspector]
    public int monedasMax = 15;
    public GameObject objetoKana;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Mision[MisionActual] = false;
        //texto.gameObject.SetActive(false);
        panelMision.SetActive(false);
        //playerScript = GameObject.Find("player").GetComponent<StatesMachine>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        foreach (GameObject panel in panelMisionesCompletadas)
        {
            if (panel != null)
                panel.SetActive(false);
        }
        objetoKana.SetActive(false);
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
            //npcScript.npcIcono.sprite = npcScript.iconoIntro;

            if (npcScript.nameNpc == "campesino1"){
                panelIconoNpc.sprite = npcIconos[0];
            }else if (npcScript.nameNpc == "campesino2"){
                panelIconoNpc.sprite = npcIconos[1];
            }

            switch (tipoMision)
            {
                case MisionTipo.HablarConNpc:
                    npcScript.npcIcono.sprite = npcScript.iconoIntro;
                    GameObject nota = Instantiate(notaPrefab, playerScript.puntoSujecion.position, Quaternion.identity);
                    Objeto objetoNota = nota.GetComponent<Objeto>();
                    playerScript.objetoSujeto = objetoNota;
                    objetoNota.Coger(playerScript.puntoSujecion);
                    break;
                case MisionTipo.BuscarObjeto:
                    npcScript.npcIcono.sprite = iconoMisionKama;
                    objetoKana.SetActive(true);
                    break;
                case MisionTipo.RecolectarMoneda:
                    npcScript.npcIcono.sprite = npcScript.iconoIntro;
                    objetivoMonedas = Random.Range(monedasMin, monedasMax);
                    texto.text = "Necesito que me traigas " + objetivoMonedas + " monedas.";
                    break;
            }
            npcScript.canvasImagen.SetActive(true);
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
                case MisionTipo.HablarConNpc:
                    GameManager.Instance.monedas += 15;
                    GameManager.Instance.textoMonedas.text = GameManager.Instance.monedas.ToString();

                    npcScript.dialogMisionMostrado = true;
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
