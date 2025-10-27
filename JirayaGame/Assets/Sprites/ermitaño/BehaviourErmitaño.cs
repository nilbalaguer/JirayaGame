using UnityEngine;

public class BehaviourErmitaño : MonoBehaviour
{
    private Animator anim;

    public enum State { Idle, Patrol, Talking };
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
    public panelErmitaño panelScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        currentState = State.Idle;

        player = GameObject.FindWithTag("Player");
        panelDialogo.SetActive(false);

        waitCounter = waitTime;
        if (puntosPatrulla.Length > 0)
        {
            transform.position = puntosPatrulla[0].position;
        }
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
                    if (waitCounter <= 0f)
                    {
                        waiting = false;
                        currentState = State.Patrol;
                    }
                }
                else if (PlayerinRange())
                {
                    currentState = State.Talking;
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
                if (PlayerinRange())
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
                }
                break;
            case State.Patrol:
                anim.SetInteger("state", 1);
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
        if (puntosPatrulla.Length == 0) return;

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
}
