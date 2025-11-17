using UnityEngine;
using UnityEngine.UI;

public class NpcStates : MonoBehaviour
{
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
    public float rangoPlayer = 2f;
    public float rangoEnemy = 3f;

    public GameObject dialogueBox;
    public GameObject introDialog;
    public ScrollPanel scrollPanel;
    public GameObject canvasImagen;

    public Image npcIcono;
    public Sprite iconoNormal;
    public Sprite iconoIntro;

    public bool hasTalked = false;
    public bool NpcIntro = false;

    public bool necesitaAlejarse = false;
    public bool introTerminada = false;
    private bool introAsignada = false;

    public bool puedeInteractuar = true;

    public Misions misionNpc;
    public bool dialogMisionMostrado = false;
    public GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        /*if (NpcIntro)
        {
            currentState = State.Intro;
        }*/
        //else
        //{
            currentState = State.Idle;
        //}
        waitCounter = waitTime;

        /*if (patrolPoints.Length > 0)
        {
            transform.position = patrolPoints[0].position;
        }*/
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
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (!introAsignada && GameManager.Instance != null)
        {
            if (NpcIntro)
            {
                currentState = State.Intro;
                introAsignada = true;
            }
            else
            {
                introAsignada = true;
            }
        }
        if (!introAsignada)
        {
            return;
        }
        
        Vector2 pos = transform.position;

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
                else if (puedeInteractuar && PlayerinRange() && !hasTalked)
                {
                    currentState = State.Alerted;
                }
                else if (EnemyinRange())
                {
                    currentState = State.Scared;
                }else if (misionNpc.misionCompletada && !dialogMisionMostrado)
                {
                    currentState = State.EndMision;
                }
                break;

            case State.Patrol:
                if (puedeInteractuar && PlayerinRange() && !hasTalked){
                    currentState = State.Alerted;
                }else if (EnemyinRange()){
                    currentState = State.Scared;
                }else if (misionNpc.misionCompletada && !dialogMisionMostrado)
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

        switch (currentState)
        {
            case State.Idle:
                rb.linearVelocity = Vector2.zero;
                anim.SetInteger("state", 0);
                /*if (dialogueBox != null)
                {
                    scrollPanel.ClosePanel();
                }*/
                break;
            case State.Patrol:
                MoveTowards(patrolPoints[currentPointIndex].position);
                /*if (dialogueBox != null)
                {
                    scrollPanel.ClosePanel();
                }*/
                break;
            case State.Alerted:
                rb.linearVelocity = Vector2.zero;
                Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
                if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
                {
                    transform.localScale = new Vector3(directionToPlayer.x < 0 ? -5 : 5, 5, 5);
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
                    dialogueBox.SetActive(true);
                    scrollPanel.npcScript = this;
                    player.GetComponent<movement>().puedoMoverme = false;

                    scrollPanel.misionsScript = misionNpc;
                }
                else
                {
                    if (!misionNpc.misionActiva)
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
                //anim.SetTrigger("scared");
                break;
            case State.Intro:
                npcIcono.sprite = iconoIntro;
                MoveTowardsPlayer();
                break;
            case State.EndMision:
                rb.linearVelocity = Vector2.zero;
                directionToPlayer = (player.transform.position - transform.position).normalized;
                if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
                {
                    transform.localScale = new Vector3(directionToPlayer.x < 0 ? -5 : 5, 5, 5);
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
                 //Añadir logica para mostrar dialogo de mision completada
                break;


        }

    }
        void MoveTowards(Vector2 target)
        {
            Vector2 dir = (target - (Vector2)transform.position).normalized;
            rb.linearVelocity = dir * speed;
            UpdateSpriteDirection(dir);
        }
        void MoveTowardsPlayer()
        {
        Vector2 playerPos = player.transform.position;
        Vector2 npcPos = transform.position;
        Vector2 directionToPlayer = (playerPos - npcPos).normalized;

        float distanciaParada = 1.5f;
        float distance = Vector2.Distance(npcPos, playerPos);
        Vector2 targetPos = playerPos - directionToPlayer * distanciaParada;
        UpdateSpriteDirection(directionToPlayer);
        
        if (distance > distanciaParada)
        {
            transform.position = Vector2.MoveTowards(
            npcPos, targetPos,
            3f * Time.deltaTime
            );
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            //anim.SetInteger("state", 0);
            if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
                {
                    transform.localScale = new Vector3(directionToPlayer.x < 0 ? -5 : 5, 5, 5);
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

            rb.simulated = false;
            npcIcono.sprite = iconoNormal;
        }
        
        }

        void NextPoint()
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }

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
            transform.localScale = new Vector3(dir.x < 0 ? -5 : 5, 5, 5);
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
        float distancia = Vector2.Distance(transform.position, player.transform.position);
        if (necesitaAlejarse && distancia > 3f)
        {
            necesitaAlejarse = false;
        }
        return distancia <= rangoPlayer && !necesitaAlejarse;
    }
    
    bool EnemyinRange()
    {
        float distancia = Vector2.Distance(transform.position, enemy.transform.position);
        return distancia <= rangoEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            /*misionNpc.CompletarMision();
            gameManager.monedas -= 1; 
            gameManager.textoMonedas.text = gameManager.monedas.ToString();*/
            if (!misionNpc.misionActiva) return;
            if (misionNpc.misionCompletada) return;

            switch (scrollPanel.misionsScript.tipoMision)
            {
                case Misions.MisionTipo.RecolectarMoneda:
                    MisionMonedas();
                    break;
               case Misions.MisionTipo.BuscarObjeto:
                    MisionObjeto();
                    break;
            }  

        }//else if (collision.CompareTag("Player"))
    }

    public void MisionMonedas()
    {
        if (gameManager.monedas >= 1)
        {
            misionNpc.CompletarMision();
            gameManager.monedas -= 1; 
            gameManager.textoMonedas.text = gameManager.monedas.ToString();
        }
    }

    public void MisionObjeto()
    {
        Objeto objeto = player.GetComponent<StatesMachine>().objetoSujeto;
        if (objeto != null && objeto.nombreObjeto == "ObjetoCampesino")
        {
            misionNpc.CompletarMision();
            Objeto objetoEntregado = objeto;
            objeto.Soltar();
            objeto = null;
            objetoEntregado.gameObject.SetActive(false);
        }
    }


}
