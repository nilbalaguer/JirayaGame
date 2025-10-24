using UnityEngine;

public class BehaviourErmitaño : MonoBehaviour
{
    private Animator anim;

    public enum State { Idle, Talking };
    public State currentState;

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
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                if (PlayerinRange())
                {
                    currentState = State.Talking;
                }
                break;

            case State.Talking:
                if (!PlayerinRange())
                {
                    currentState = State.Idle;
                }
                break;
        }

        switch (currentState)
        {
            case State.Idle:
                anim.SetBool("isTalking", false);
                break;

            case State.Talking:
                anim.SetBool("isTalking", true);
                if (!panelScript.hasTalked)
                {
                    panelDialogo.SetActive(true);
                }
                break;
        }
    }

    bool PlayerinRange()
    {
        float distancia = Vector2.Distance(transform.position, player.transform.position);
        return distancia <= rangoPlayer;
    }
}
