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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
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
                if (!PlayerinRange())
                {
                    currentState = State.Idle;

                }
                break;
        }
        
        switch (currentState)
        {
            case State.Idle:
                anim.SetInteger("state", 0);
                break;

            case State.Talking:
                anim.SetInteger("state", 1);
                break;
        }
    }

    bool PlayerinRange()
    {
        float distancia = Vector2.Distance(transform.position, player.transform.position);
        return distancia <= rangoPlayer;
    }
}
