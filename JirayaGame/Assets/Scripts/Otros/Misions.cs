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
    public TextMeshProUGUI nombreNpc;
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
    public GameObject panelMisionCompletada;
    public string npcDestino;
    public ObjetoData notaPrefab;
    [HideInInspector]
    public bool panelCompletadoMostrado = false;
    public Sprite iconoMisionKama;
    public Sprite iconoEntregarNota;
    public int objetivoMonedas;
    [HideInInspector]
    public int monedasMin = 5;
    [HideInInspector]
    public int monedasMax = 15;
    public GameObject objetoKana;
    private Inventario inventario;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Mision[MisionActual] = false;
        //texto.gameObject.SetActive(false);
        panelMision.SetActive(false);
        //playerScript = GameObject.Find("player").GetComponent<StatesMachine>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        panelMisionCompletada.SetActive(false);
        objetoKana.SetActive(false);
        if (iconoEntregarNota == null)
        {
            return;
        }
        inventario = playerScript.GetComponent<Inventario>();
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
            nombreNpc.text = npcScript.nameNpc;
            //npcScript.npcIcono.sprite = npcScript.iconoIntro;

            if (npcScript.nameNpc == "Goro" || npcScript.nameNpc == "Saburo"){
                panelIconoNpc.sprite = npcIconos[0];
            }else if (npcScript.nameNpc == "Kichiro"){
                panelIconoNpc.sprite = npcIconos[1];
            }else if (npcScript.nameNpc == "Jiro"){
                panelIconoNpc.sprite = npcIconos[2];
            }else if (npcScript.nameNpc == "Taro"){
                panelIconoNpc.sprite = npcIconos[3];
            }

            switch (tipoMision)
            {
                case MisionTipo.HablarConNpc:
                    npcScript.npcIcono.sprite = npcScript.iconoIntro;
                    /*playerScript.objetoSujeto = objetoNota;
                    objetoNota.Coger(playerScript.puntoSujecion);*/
                    npcScript.npcIcono.sprite = iconoEntregarNota;
                    inventario.AñadirObjeto(notaPrefab);
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

    public void MostrarPanelMisionCompletada(string[] textoCompletado)
    {
        panelMisionCompletada.SetActive(true);
        panelInfoManager info = panelMisionCompletada.GetComponent<panelInfoManager>();
        info.npcScript = npcScript;
        info.paginas = textoCompletado;
        info.audioSource = npcScript.GetComponent<AudioSource>();
        info.nombreNpc.text = npcScript.nameNpc;
        if (tipoMision == MisionTipo.HablarConNpc)
        {
            info.nombreNpc.text = "Jiro";
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
                    GameManager.Instance.textoMonedas.text = GameManager.Instance.monedas.ToString();
                    MostrarPanelMisionCompletada(new string[] {"¡Gracias por traerme las monedas!", "Aquí tienes tu recompensa."});
                    break;
                case MisionTipo.BuscarObjeto:
                    GameManager.Instance.monedas += 20;
                    GameManager.Instance.textoMonedas.text = GameManager.Instance.monedas.ToString();
                    MostrarPanelMisionCompletada(new string[] {"¡Porfin podre cortar mi arroz!", "Te lo agradezco mucho."});
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

    public void CancelarMision()
    {
        misionActiva = false;
        panelMision.SetActive(false);

        if (npcScript != null)
        {
            npcScript.canvasImagen.SetActive(false);
            npcScript.misionNpc = null;
        }
    }

    public void ActivarMision()
    {
        if (misiones != null && misiones != this)
        {
            misiones.CancelarMision();
        }
        misiones = this;
        misionActiva = true;
        MostrarMision();
    }

    public void DesactivarPanel()
    {
        panelMision.SetActive(false);
    }
}
