using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tsunade : MonoBehaviour
{
    private Animator anim;
    public enum State {Idle, Talking};
    private State currentState;
    private GameObject player;
    public float rangoPlayer = 1f;
    public ScrollPanel scrollPanel;
    public GameObject panelDialogo;
    public GameObject tsunadePanel2;

    public GameObject[] recompensas;
    public StatesMachine playerScript;
    //private Objeto objetoSujeto;
    private Objeto objetoRecompensa;
    public Objeto objetoRecibido;
    private GameObject prefabRecompensa;

    [HideInInspector]
    public float ultimoDialogo = 0f;
    public float cooldownDialogo = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        panelDialogo.SetActive(false);
        anim = GetComponent<Animator>();
        currentState = State.Idle;
        tsunadePanel2.SetActive(false);
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
                panelDialogo.GetComponent<panelTsunade>().DialogoSetup(objetoRecibido.nombreObjeto);
                break;
        }
        
        if (PlayerinRange() && playerScript.objetoSujeto == null && !scrollPanel.entregarObjeto)
        {
            if (Time.time - ultimoDialogo >= cooldownDialogo)
            {
                tsunadePanel2.SetActive(true);
            }
        }else if (PlayerinRange() && playerScript.objetoSujeto != null && objetoRecompensa.esRecompensa)
        {
            tsunadePanel2.SetActive(false);
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
        objetoRecompensa = recompensaInstanciada.GetComponent<Objeto>();
        objetoRecompensa.esRecompensa = true;

        playerScript.objetoSujeto = objetoRecompensa;
        objetoRecompensa.Coger(playerScript.puntoSujecion);

        playerScript.RecibirRecompensa(objetoRecompensa);
        
        //StatesMachine playerScript = player.GetComponent<StatesMachine>();
        //playerScript.RecibirRecompensa(recompensaInstanciada.GetComponent<Objeto>());
    }
}
