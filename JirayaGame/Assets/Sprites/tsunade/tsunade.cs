using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tsunade : MonoBehaviour
{
    private Animator anim;
    public enum State {Idle, Talking};
    public State currentState;
    private GameObject player;
    public float rangoPlayer = 1.5f;
    public ScrollPanel scrollPanel;
    public GameObject panelDialogo;
    public GameObject tsunadePanel2;

    public GameObject[] recompensas;
    //public StatesMachine playerScript;
    public PlayerController playerScript;
    //private Objeto objetoSujeto;
    private Objeto objetoRecompensa;
    public Objeto objetoRecibido;
    private GameObject prefabRecompensa;

    [HideInInspector]
    public float ultimoDialogo = 0f;
    public float cooldownDialogo = 2f;
    public bool entregado = false;
    [HideInInspector]
    public bool recompensaEntregadaRecientemente = false;
    public TextMeshProUGUI nombreTsunade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        panelDialogo.SetActive(false);
        anim = GetComponent<Animator>();
        currentState = State.Idle;
        tsunadePanel2.SetActive(false);
        nombreTsunade.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                if (PlayerinRange() && entregado)
                {
                    currentState = State.Talking;

                }
                break;

            case State.Talking:
                if (!entregado)
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
                    transform.localScale = new Vector3(directionToPlayer.x < 0 ? -3 : 3, 3, 3);
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
                Vector2 dirToPlayer = player.transform.position - transform.position;
                if (Mathf.Abs(dirToPlayer.x) > Mathf.Abs(dirToPlayer.y))    
                {
                    transform.localScale = new Vector3(dirToPlayer.x < 0 ? -3 : 3, 3, 3);
                    anim.SetInteger("state", 1);
                }
                else
                {
                    if (dirToPlayer.y > 0)
                    {
                        anim.SetInteger("state", 3);
                    }else
                    {
                        anim.SetInteger("state", 2);
                        //añadir state 3 animacion back talk
                    }
                }
                //anim.SetInteger("state", 1);
                //panelDialogo.SetActive(true);
                if (objetoRecibido != null){
                    panelDialogo.SetActive(true);
                    panelDialogo.GetComponent<panelTsunade>().DialogoSetup(objetoRecibido.nombreObjeto);
                }
                //Si se han entregado los 3 objetos mostrar dialofo final
                playerScript.puedoMoverme = false;
                
                break;
        }
        
        if (PlayerinRange() && !playerScript.ObjetoTsunadeExiste() && !scrollPanel.entregarObjeto && !recompensaEntregadaRecientemente)
        {
            if (Time.time - ultimoDialogo >= cooldownDialogo)
            {
                tsunadePanel2.SetActive(true);
                Vector2 dirToPlayer = player.transform.position - transform.position;
                if (Mathf.Abs(dirToPlayer.x) > Mathf.Abs(dirToPlayer.y))    
                {
                    transform.localScale = new Vector3(dirToPlayer.x < 0 ? -3 : 3, 3, 3);
                    anim.SetInteger("state", 1);
                }
                else
                {
                    if (dirToPlayer.y > 0)
                    {
                        anim.SetInteger("state", 3);
                    }else
                    {
                        anim.SetInteger("state", 2);
                        //añadir state 3 animacion back talk
                    }
                }
                
                playerScript.puedoMoverme = false;
            }
        }else if (PlayerinRange() && recompensaEntregadaRecientemente)
        //else if (PlayerinRange() && playerScript.objetoSujeto != null && objetoRecompensa != null &&objetoRecompensa.esRecompensa)
        {
            tsunadePanel2.SetActive(false);
        }

        if (!PlayerinRange())
        {
            recompensaEntregadaRecientemente = false;
        }

        float distanciaTsunadePlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanciaTsunadePlayer <= 2.5f)
        {
            nombreTsunade.gameObject.SetActive(true);
        }
        else
        {
            nombreTsunade.gameObject.SetActive(false);
        }
    }

    public void MostrarDialogo()
    {
        if (objetoRecibido == null) return;

        panelDialogo.SetActive(true);

        panelDialogo.GetComponent<panelTsunade>().DialogoSetup(objetoRecibido.nombreObjeto);
    }

    bool PlayerinRange()
    {
        float distancia = 1f;
        Vector2[] direcciones = 
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };
        foreach (Vector2 direccion in direcciones)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direccion, distancia, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, direccion * distancia, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
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
        GameObject recompensaInstanciada = Instantiate(prefabRecompensa);
        objetoRecompensa = recompensaInstanciada.GetComponent<Objeto>();
        objetoRecompensa.esRecompensa = true;

        playerScript.inventario.AñadirObjeto(objetoRecompensa);
        recompensaEntregadaRecientemente = true;

        //Destroy(recompensaInstanciada);
        objetoRecibido = null;
    }
}
