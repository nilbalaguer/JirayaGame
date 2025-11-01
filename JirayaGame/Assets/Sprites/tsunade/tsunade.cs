using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tsunade : MonoBehaviour
{
    private Animator anim;
    public enum State {Idle, Talking};
    private State currentState;
    private GameObject player;
    public float rangoPlayer = 2f;
    public ScrollPanel scrollPanel;
    public GameObject panelDialogo;

    public GameObject[] recompensas;
    public StatesMachine playerScript;
    private Objeto objetoSujeto;
    public Objeto objetoRecibido;
    private GameObject prefabRecompensa;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        panelDialogo.SetActive(false);
        anim = GetComponent<Animator>();
        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                if (PlayerinRange() && scrollPanel.entregarObjeto)
                {
                    currentState = State.Talking;

                }
                break;

            case State.Talking:
                if (!scrollPanel.entregarObjeto)
                {
                    currentState = State.Idle;

                }
                break;
        }
        
        switch (currentState)
        {
            case State.Idle:
                Vector2 directionToPlayer = player.transform.position - transform.position;
                if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
                {
                    transform.localScale = new Vector3(directionToPlayer.x < 0 ? -5 : 5, 5, 5);
                    anim.SetInteger("state", 0);
                }
                else
                {
                    if (directionToPlayer.y > 0)
                    {
                        anim.SetInteger("state", 4);
                    }
                    else
                    {
                        anim.SetInteger("state", 5);
                    }
                }
                break;

            case State.Talking:
                anim.SetInteger("state", 1);
                panelDialogo.SetActive(true);
                panelDialogo.GetComponent<panelTsunade>().ConfigurarDialogo(objetoRecibido.nombreObjeto);
                break;
        }
    }

    bool PlayerinRange()
    {
        float distancia = Vector2.Distance(transform.position, player.transform.position);
        return distancia <= rangoPlayer;
    }

    public void EntregarRecompensa()
    {

        switch (objetoRecibido.tipo)
        {
            case Objeto.TipoObjeto.PergaminoSagrado:
                prefabRecompensa = recompensas[0];
                break;
            case Objeto.TipoObjeto.CollarShizune:
                prefabRecompensa = recompensas[1];
                break;
            case Objeto.TipoObjeto.Flor:
                prefabRecompensa = recompensas[2];
                break;
        }
        objetoRecibido = null;
        GameObject recompensaInstanciada = Instantiate(prefabRecompensa, playerScript.puntoSujecion.position, Quaternion.identity);
        Objeto objetoRecompensa = recompensaInstanciada.GetComponent<Objeto>();
        objetoRecompensa.esRecompensa = true;

        objetoSujeto = objetoRecompensa;
        objetoRecompensa.Coger(playerScript.puntoSujecion);

        playerScript.RecibirRecompensa(objetoRecompensa);
        
        //StatesMachine playerScript = player.GetComponent<StatesMachine>();
        //playerScript.RecibirRecompensa(recompensaInstanciada.GetComponent<Objeto>());
    }
}
