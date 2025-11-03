using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum Estado {Normal, Intro};
    public Estado estadoActual = Estado.Normal;
    public Inventario inventario;
    public StatesMachine player;

    public NpcStates npcIntro;
    private NpcStates npcIntroActual;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IniciarIntro();
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

    }

    public void PantallaDerrota()
    {
        
    }
    
    //Mas funciones que afecten al hud del jugador y cosas que pasen en el juego
}
