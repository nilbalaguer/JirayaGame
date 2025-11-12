using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum Estado {Normal, Intro};
    public Estado estadoActual = Estado.Normal;
    public Inventario inventario;
    public StatesMachine player;
    private Objeto objetoCompradoNuevo;

    public NpcStates npcIntro;
    private NpcStates npcIntroActual;
    public TextMeshProUGUI textoMonedas;
    public int monedas = 0;

    public GameObject tiendaAlerta;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IniciarIntro();
        monedas = 0;
        textoMonedas.text = monedas.ToString();
        tiendaAlerta.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (estadoActual == Estado.Intro && npcIntroActual != null)
        {
            if (npcIntroActual.introTerminada)
            {
                estadoActual = Estado.Normal;
                player.GetComponent<movement>().puedoMoverme = true;
                npcIntroActual = null;
            }
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IniciarIntro()
    {
        estadoActual = Estado.Intro;
        player.GetComponent<movement>().puedoMoverme = false;

        npcIntro.NpcIntro = true;
        npcIntro.currentState = NpcStates.State.Intro;
        npcIntroActual = npcIntro;
    }

    public void FinalizarIntro(NpcStates npc)
    {
        if (npc == npcIntroActual)
        {
            npcIntroActual.introTerminada = true;
        }
    }

    public void PantallaJefeFinal()
    {
        
    }


    public void RecolectarMonedas()
    {
        monedas += 1;
        textoMonedas.text = monedas.ToString();
    }

    public void ComprarObjetos(Objeto objetoComprado)
    {
        int precio = objetoComprado.precioTienda;

        if (monedas >= precio)
        {
            monedas -= precio;
            textoMonedas.text = monedas.ToString();
            Debug.Log("Objeto comprado");
            //player.EquiparObjeto(objetoComprado);
            GameObject ObjInstanciado = Instantiate(objetoComprado.gameObject);
            objetoCompradoNuevo = ObjInstanciado.GetComponent<Objeto>();
            inventario.AñadirObjeto(objetoCompradoNuevo);
            //Equipar objeto y cada vez que se use restar cantidad en el inventario
        }
        else
        {
            Debug.Log("No tienes suficientes monedas para comprar este objeto");
            tiendaAlerta.SetActive(true);
            Invoke("DesactivartiendaAlerta", 1f);
        }
    }
    
    private void DesactivartiendaAlerta()
    {
        tiendaAlerta.SetActive(false);
    }

    public void PantallaDerrota()
    {
        //Si la vida del player es 0 se muestra esta pantalla
    }
}
