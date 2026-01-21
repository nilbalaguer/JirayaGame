using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcStates : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    private Animator anim;

    public enum State { Idle, Patrol, Alerted, Scared, Intro, EndMision };
    public State currentState;

    public Transform[] patrolPoints; 
    private int currentPointIndex = 0;
    public float speed = 2f;
    public float waitTime = 2f;        
    private float waitCounter;
    private bool waiting = false;

    private GameObject player;
    private GameObject enemy;
    public float rangoPlayer = 1f;
    public float rangoEnemy = 3f;

    public GameObject dialogueBox;
    public GameObject introDialog;
    public PanelNpc panelNpcScript;
    public ScrollPanel scrollPanel;
    public GameObject canvasImagen;
    public GameObject dialogueShamuzen;

    public Image npcIcono;
    public Sprite iconoNormal;
    public Sprite iconoIntro;

    public bool hasTalked = false;
    public bool NpcIntro = false;

    public bool necesitaAlejarse = false;
    public bool introTerminada = false;
    public bool introAsignada = false;

    public bool puedeInteractuar = true;

    public Misions misionNpc;
    public bool dialogMisionMostrado = false;
    public GameManager gameManager;
    public string nameNpc;
    
    public bool esNpcShamizen;

    public string dialogMision;
    public bool playerDetectado = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = State.Idle;
        
        /*if (NpcIntro)
        {
            currentState = State.Intro;
        }*/
        //else
        //{
        if (!NpcIntro)
        currentState = State.Idle;

        //}
        waitCounter = waitTime;

        player = GameObject.FindWithTag("Player");
        enemy = GameObject.FindWithTag("Enemy");
        dialogueBox.SetActive(false);
        introDialog.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (puedeInteractuar == false)
        {
            hasTalked = true;
            canvasImagen.SetActive(false);
        }

        if (dialogueShamuzen == null && panelNpcScript == null && !esNpcShamizen)
        {
            return;
        }
        else
        {
            dialogueShamuzen.SetActive(false);
        }

        if (dialogMision == null || dialogMision == "")
        {
            return;
        }

        if (introDialog == null)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (!introAsignada)
        {
            if (GameManager.Instance == null) return;
            if (player == null) return;

            if (NpcIntro)
                currentState = State.Intro;

            introAsignada = true;
        }
        
        Vector2 pos = transform.position;
        
        //Switch para gestionar los cambios de estado
        switch (currentState)
        {
            case State.Idle:

                if (!waiting)
                {
                    waiting = true;
                    waitCounter = waitTime;
                }
                else if (!PlayerinRange())
                {
                    waitCounter -= Time.deltaTime;
                    if (waitCounter <= 0f)
                    {
                        waiting = false;
                        currentState = State.Patrol;
                    }
                }
                else if (puedeInteractuar && PlayerinRange() && !hasTalked && (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.X)))
                {
                    currentState = State.Alerted;
                }
                else if (EnemyinRange())
                {
                    currentState = State.Scared;
                }else if (misionNpc != null && misionNpc.misionCompletada && !dialogMisionMostrado)
                {
                    currentState = State.EndMision;
                }
                break;
            //Patrullaje del npc
            case State.Patrol:
                if (puedeInteractuar && PlayerinRange() && !hasTalked && (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.X))){
                    currentState = State.Alerted;
                }else if (EnemyinRange()){
                    currentState = State.Scared;
                }else if (misionNpc != null && misionNpc.misionCompletada && !dialogMisionMostrado)
                {
                    currentState = State.EndMision;
                }
                else
                {
                    if (Vector2.Distance(pos, patrolPoints[currentPointIndex].position) < 0.1f)
                    {
                        currentState = State.Idle;
                        waitCounter = waitTime;
                        NextPoint();
                    }
                }
                break;
            case State.Alerted:
                if (!PlayerinRange() && hasTalked)
                {
                    currentState = State.Idle;
                    waitCounter = waitTime;
                }
                break;
            case State.Scared:
                if (!EnemyinRange())
                {
                    currentState = State.Idle;
                    waitCounter = waitTime;
                }
                break;
            case State.Intro:
                if (!NpcIntro)
                {
                    currentState = State.Idle;
                    waitCounter = waitTime;
                }
                break;
            case State.EndMision:
                if (dialogMisionMostrado)
                {
                    currentState = State.Idle;
                    waitCounter = waitTime;
                }
                break;
        }

        //Switch para gestionar las animaciones y todo lo que se ejecutara en cada estado (cambio de propiedades, etc)

        switch (currentState)
        {
            case State.Idle:
                rb.linearVelocity = Vector2.zero;
                anim.SetInteger("state", 0);
                break;
            case State.Patrol:
                MoveTowards(patrolPoints[currentPointIndex].position);
                break;
            case State.Alerted:
                rb.linearVelocity = Vector2.zero;
                Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
                if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
                {
                    //transform.localScale = new Vector3(directionToPlayer.x < 0 ? -3 : 3, 3, 3);
                    spriteRenderer.flipX = directionToPlayer.x < 0;
                    anim.SetInteger("state", 7);
                }
                else
                {
                    if (directionToPlayer.y > 0)
                    {
                        anim.SetInteger("state", 5);
                    }
                    else
                    {
                        anim.SetInteger("state", 4);
                    }
                }
                if (!hasTalked)
                {
                    if (!dialogueBox.activeSelf)
                    {
                        if (!esNpcShamizen){
                            dialogueBox.SetActive(true);
                            scrollPanel.npcScript = this;
                            scrollPanel.AsignarTextoMision(this);
                            player.GetComponent<PlayerController>().puedoMoverme = false;

                            scrollPanel.misionsScript = misionNpc;
                            scrollPanel.audioSource = GetComponent<AudioSource>();
                        }
                        else{
                            dialogueShamuzen.SetActive(true);
                            dialogueShamuzen.GetComponent<PanelNpc>().audioSource = GetComponent<AudioSource>();
                            panelNpcScript.npcScript = this;
                            player.GetComponent<PlayerController>().puedoMoverme = false;
                        }
                        GameManager.Instance.inputDesactivado = true;
                    }
                }
                else
                {
                    if (misionNpc == null || !misionNpc.misionActiva)
                    {
                        canvasImagen.SetActive(false);
                    }
                    else
                    {
                        canvasImagen.SetActive(true);
                    }
                }
                break;
            case State.Scared:
                rb.linearVelocity = Vector2.zero;
                anim.SetInteger("state", 6);
                break;
            case State.Intro:
                npcIcono.sprite = iconoIntro;
                player.GetComponent<PlayerController>().MirarObjetivo();
                MoveTowardsPlayer();
                break;
            case State.EndMision:
                rb.linearVelocity = Vector2.zero;
                directionToPlayer = (player.transform.position - transform.position).normalized;
                if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
                {
                    spriteRenderer.flipX = directionToPlayer.x < 0;
                    anim.SetInteger("state", 7);
                }
                else
                {
                    if (directionToPlayer.y > 0)
                    {
                        anim.SetInteger("state", 5);
                    }
                    else
                    {
                        anim.SetInteger("state", 4);
                    }
                }
                break;


        }

    }
    //Funciones para patrullar por los puntos y seguir al jugador en el caso del npc inicial
        void MoveTowards(Vector2 target)
        {
            Vector2 dir = (target - (Vector2)transform.position).normalized;
            rb.linearVelocity = dir * speed;
            UpdateSpriteDirection(dir);
        }
        void MoveTowardsPlayer()
        {
        player.GetComponent<PlayerController>().puedoMoverme = false;
        Vector2 playerPos = player.transform.position;
        Vector2 npcPos = transform.position;
        Vector2 directionToPlayer = (playerPos - npcPos).normalized;
        playerDetectado = PlayerinRange();

        if (!playerDetectado)
        {
            /*float distanciaParada = 1f;
            float distance = Vector2.Distance(npcPos, playerPos);
            Vector2 targetPos = playerPos - directionToPlayer * distanciaParada;*/
            Vector2 dir = (playerPos - npcPos).normalized;
            UpdateSpriteDirection(dir);
            
            //if (distance > distanciaParada)
            //{
                transform.position = Vector2.MoveTowards(
                npcPos, playerPos,
                3f * Time.deltaTime
                );
            //}
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
                {
                    //transform.localScale = new Vector3(directionToPlayer.x < 0 ? -3 : 3, 3, 3);
                    spriteRenderer.flipX = directionToPlayer.x < 0;
                    anim.SetInteger("state", 7);
                }
                else
                {
                    if (directionToPlayer.y > 0)
                    {
                        anim.SetInteger("state", 5);
                    }
                    else
                    {
                        anim.SetInteger("state", 4);
                    }
                }
            introDialog.SetActive(true);
            introDialog.GetComponent<PanelNpc>().audioSource = GetComponent<AudioSource>();

            rb.simulated = false;
            npcIcono.sprite = iconoNormal;
        }
        
        }

        void NextPoint()
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }

    //Actualizar animaciones de walk al patrullar
    void UpdateSpriteDirection(Vector2 dir)
    {
        float absX = Mathf.Abs(dir.x);
        float absY = Mathf.Abs(dir.y);

        if (dir.magnitude < 0.1f)
        {
            anim.SetInteger("state", 0);
            return;
        }

        if (absX > absY)
        {
            anim.SetInteger("state", 1);
            spriteRenderer.flipX = dir.x < 0;
        }
        else
        {
            if (dir.y > 0)
                anim.SetInteger("state", 3);
            else
                anim.SetInteger("state", 2);
        }

    }

    bool PlayerinRange()
    {
        /*float distancia = Vector2.Distance(transform.position, player.transform.position);
        if (necesitaAlejarse && distancia > 3f)
        {
            necesitaAlejarse = false;
        }
        return distancia <= rangoPlayer && !necesitaAlejarse;*/

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, rangoPlayer, LayerMask.GetMask("Player"));
        if (hit != null && hit.Length > 0 && !necesitaAlejarse)
        {
            return true;
        }
        
        if (necesitaAlejarse && hit.Length == 0)
        {
            necesitaAlejarse = false;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangoPlayer);
    }
    
    bool EnemyinRange()
    {
        if (enemy == null)
        {
            return false;
        }
        Vector2[] direcciones = 
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };
        foreach (Vector2 direccion in direcciones)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direccion, rangoEnemy, LayerMask.GetMask("Enemy"));

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    public bool ObjetoMisionExiste()
    {
        foreach (var entry in player.GetComponent<PlayerController>().inventario.objetos){
            if (entry.item.nombre == "ObjetoCampesino" || 
            entry.item.nombre == "NotaMision")
            {
                return true;
            }
        }
        return false;
    }


    //Sistema de misiones npc
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            /*misionNpc.CompletarMision();
            gameManager.monedas -= 1; 
            gameManager.textoMonedas.text = gameManager.monedas.ToString();*/
            if (misionNpc != null && !misionNpc.misionActiva)
            {
                return;
            }
            if(misionNpc != null && misionNpc.misionCompletada){
                return;
            }
            if (misionNpc == null)
            {
                return;
            }

            //Dependiendo de la mision activa se ejecutara una funcion o otra para poder determinar si se ha completado la mision correspondiente
            switch (misionNpc.tipoMision)
            {
                case Misions.MisionTipo.RecolectarMoneda:
                    MisionMonedas();
                    break;
               case Misions.MisionTipo.BuscarObjeto:
                    MisionObjeto();
                    break;
                case Misions.MisionTipo.HablarConNpc:
                    MisionHablar();
                    break;
            }  

        }
    }

    public void MisionMonedas()
    {
        if (gameManager.monedas >= misionNpc.objetivoMonedas)
        {
            misionNpc.CompletarMision();
            gameManager.monedas -= misionNpc.objetivoMonedas; 
            gameManager.textoMonedas.text = gameManager.monedas.ToString();
        }
    }

    public void MisionObjeto()
    {
        //Objeto objeto = player.GetComponent<PlayerController>().objetoSujeto;
        Inventario.InventoryEntry entry = player.GetComponent<PlayerController>().inventario.objetos.Find(e => e.item.nombre == "ObjetoCampesino");
        
        if (entry != null)
        {
            misionNpc.CompletarMision();
            player.GetComponent<PlayerController>().inventario.EliminarObjeto(entry);
            /*Objeto objetoEntregado = objeto;
            objeto.Soltar();
            Destroy(objetoEntregado.gameObject);
            objeto = null;*/
            player.GetComponent<PlayerController>().CanvasInfo.SetActive(false);
        }
        else
        {
            return;
        }
    }

    public void MisionHablar()
    {

            if (misionNpc == null)
            {
                Debug.Log("misionNpc ES NULL");
                return;
            }

            if (nameNpc != misionNpc.npcDestino)
            {
                Debug.Log("Este NPC no es el destino");
                return;
            }
            Inventario.InventoryEntry notaExiste = player.GetComponent<PlayerController>().inventario.objetos.Find(e => e.item.nombre == "NotaMision");
            if (notaExiste != null){
                misionNpc.CompletarMision();
                //dialogMisionMostrado = false;
                MostrarDialogoFinal();
                player.GetComponent<PlayerController>().inventario.EliminarObjeto(notaExiste);
            }
            else
            {
                Debug.Log("No llevas la nota correcta");
            }
            /*Objeto objeto = player.GetComponent<PlayerController>().objetoSujeto;
            if (objeto != null && objeto.nombreObjeto == "NotaMision")
            {
                objeto.Soltar();
                Destroy(objeto.gameObject);
                objeto = null;

                misionNpc.CompletarMision();
                dialogMisionMostrado = false;
                MostrarDialogoFinal();
            }
            else
            {
                Debug.Log("No llevas la nota correcta.");
            }*/

    }

    public void MostrarDialogoFinal()
    {
        //dialogFinalMision.SetActive(true);
        misionNpc.MostrarPanelMisionCompletada(new string[] {"Gracias por la nota", "Dicen que orochimaru esta furioso, ten cuidado."});
        canvasImagen.SetActive(false); 
    }


}
