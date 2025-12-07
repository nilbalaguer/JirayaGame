using UnityEngine;

public class BehaviourErmitaño : MonoBehaviour
{
    private Animator anim;

    public enum State { Idle, Patrol, Talking, TalkingShop, Chasing};
    public State currentState;

    public Transform[] puntosPatrulla;
    public float waitTime = 2f;        
    private float waitCounter;
    private bool waiting = false;
    private int indiceActual = 0;
    public float speed = 2f;

    private GameObject player;
    public float rangoPlayer = 1f;
    public GameObject panelDialogo;
    public GameObject panelTienda; 
    public panelErmitaño panelScript;
    public bool esErmitañoTienda = false;
    public GameObject CanvasTienda;
    [HideInInspector]
    public bool puedeMoverse = false;
    public float rangoEnemy = 5f;
    private GameObject enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        currentState = State.Idle;

        player = GameObject.FindWithTag("Player");
        panelDialogo.SetActive(false);
        panelTienda.SetActive(false);

        waitCounter = waitTime;
        /*if (puntosPatrulla.Length > 0)
        {
            transform.position = puntosPatrulla[0].position;
        }*/
        CanvasTienda.SetActive(false);
        enemy = GameObject.FindWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                if (!waiting)
                {
                    waiting = true;
                    waitCounter = waitTime;
                }
                else if (!PlayerinRange() && puedeMoverse)
                {
                    waitCounter -= Time.deltaTime;
                    if (waitCounter <= 0f && !esErmitañoTienda)
                    {
                        waiting = false;
                        currentState = State.Patrol;
                    }
                }
                else if (PlayerinRange() && !panelScript.hasTalked && !esErmitañoTienda)
                {
                    currentState = State.Talking;
                }
                else if (PlayerinRange() && esErmitañoTienda && Input.GetKeyDown(KeyCode.E))
                {
                    currentState = State.TalkingShop;
                }
                
                break;

            case State.Talking:
                if (!PlayerinRange())
                {
                    currentState = State.Idle;
                    waitCounter = waitTime;
                }
                break;
            case State.Patrol:
                if (esErmitañoTienda || !puedeMoverse)
                {
                    currentState = State.Idle;
                    break;
                }
                if (PlayerinRange() && !panelScript.hasTalked)
                {
                    currentState = State.Talking;
                }
                else
                {
                    NextPoint();
                }
                break;
            case State.Chasing:
                if (!PlayerinRange()){
                    currentState = State.Idle;
                    break;
                }

                if (Vector2.Distance(transform.position, player.transform.position) < 1f)
                {
                    currentState = State.Talking;
                }
                else
                {
                    Vector2 direction = (player.transform.position - transform.position).normalized;
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    UpdateSpriteAnimation(direction);
                }
                break;
        }

        switch (currentState)
        {
            case State.Idle:
                anim.SetInteger("state", 0);
                break;

            case State.Talking:
                //anim.SetInteger("state", 2);
                Vector2 dirToPlayer = player.transform.position - transform.position;
                if (Mathf.Abs(dirToPlayer.x) > Mathf.Abs(dirToPlayer.y))    
                {
                    transform.localScale = new Vector3(dirToPlayer.x < 0 ? -3 : 3, 3, 3);
                    anim.SetInteger("state", 3);
                }
                else
                {
                    if (dirToPlayer.y > 0)
                    {
                        anim.SetInteger("state", 4);
                    }else
                    {
                        anim.SetInteger("state", 2);
                        //añadir state 3 animacion back talk
                    }
                }
                if (!panelScript.hasTalked)
                {
                    panelDialogo.SetActive(true);
                    player.GetComponent<PlayerController>().puedoMoverme = false;
                }
                break;
            case State.Patrol:
                anim.SetInteger("state", 1);
                break;
            case State.TalkingShop:
                if (!panelTienda.activeSelf)
                {
                    panelTienda.SetActive(true);
                }
                break;
            case State.Chasing:
                anim.SetInteger("state", 1);
                break;
        }

        if (esErmitañoTienda && PlayerinRange())
        {
            CanvasTienda.SetActive(true);
        }
        else if (esErmitañoTienda && !PlayerinRange())
        {
            CanvasTienda.SetActive(false);
        }
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

    void NextPoint()
    {
        if (esErmitañoTienda && puntosPatrulla.Length == 0) return;

        Transform destino = puntosPatrulla[indiceActual];
        transform.position = Vector2.MoveTowards(transform.position, destino.position, speed * Time.deltaTime);

        UpdateSpriteAnimation((destino.position - transform.position).normalized);

        if (Vector2.Distance(transform.position, destino.position) < 0.1f)
        {
            currentState = State.Idle;
            indiceActual = (indiceActual + 1) % puntosPatrulla.Length;
        }
    }
    void UpdateSpriteAnimation(Vector2 dir)
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
            transform.localScale = new Vector3(dir.x < 0 ? -3 : 3, 3, 3);
        }
    }
    
    public void CerrarTienda()
    {
        panelTienda.SetActive(false);
        currentState = State.Idle;
    }

    bool EnemyinRange()
    {
        if (enemy == null)
        {
            return false;
        }
        float distancia = Vector2.Distance(transform.position, enemy.transform.position);
        return distancia <= rangoEnemy;
    }
}
