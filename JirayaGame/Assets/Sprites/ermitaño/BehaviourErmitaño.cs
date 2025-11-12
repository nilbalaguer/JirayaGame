using UnityEngine;

public class BehaviourErmitaño : MonoBehaviour
{
    private Animator anim;

    public enum State { Idle, Patrol, Talking, TalkingShop };
    public State currentState;

    public Transform[] puntosPatrulla;
    public float waitTime = 2f;        
    private float waitCounter;
    private bool waiting = false;
    private int indiceActual = 0;
    public float speed = 2f;

    private GameObject player;
    public float rangoPlayer = 2f;
    public GameObject panelDialogo;
    public GameObject panelTienda; 
    public panelErmitaño panelScript;
    public bool esErmitañoTienda = false;
    public GameObject CanvasTienda;
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
                else if (!PlayerinRange())
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
                
                if (PlayerinRange() && esErmitañoTienda)
                {
                    CanvasTienda.SetActive(true);
                }
                else
                {
                    CanvasTienda.SetActive(false);
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
                if (esErmitañoTienda)
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
            
        }

        switch (currentState)
        {
            case State.Idle:
                anim.SetInteger("state", 0);
                break;

            case State.Talking:
                anim.SetInteger("state", 2);
                if (!panelScript.hasTalked)
                {
                    panelDialogo.SetActive(true);
                    player.GetComponent<movement>().puedoMoverme = false;
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
        }
    }

    bool PlayerinRange()
    {
        float distancia = Vector2.Distance(transform.position, player.transform.position);
        return distancia <= rangoPlayer;
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
            transform.localScale = new Vector3(dir.x < 0 ? -5 : 5, 5, 5);
        }
    }
    
    public void CerrarTienda()
    {
        panelTienda.SetActive(false);
        currentState = State.Idle;
    }
}
