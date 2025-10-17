using UnityEngine;

public class StatesMachine : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    public enum State {Idle, Walk, Walk_front, Walk_back, Death};
    private State PlayerState;
    public float velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        PlayerState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = body.linearVelocity;
        velocity = vel.magnitude;

        //Switch para cambiar de estado
        switch (PlayerState)
        {
            /*case State.Idle:
                if (velocity > 0.1f && velocity <= 2f)
                    PlayerState = State.Walk;
                else if (Input.GetKey(KeyCode.LeftShift) && velocity > 0.1f)
                    PlayerState = State.Run;  
                break;
            case State.Walk:
                if (velocity <= 0.1f)
                    PlayerState = State.Idle;
                else if (Input.GetKey(KeyCode.LeftShift) && velocity > 0.1f)
                    PlayerState = State.Run;
                break;
            case State.Run:
                if (velocity <= 2f && velocity > 0.1f)
                    PlayerState = State.Walk;
                else if (velocity <= 0.1f)
                    PlayerState = State.Idle;
                break;*/

            case State.Idle:
                if (velocity > 0.1f)
                {
                    if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y))
                    {
                        PlayerState = State.Walk;
                    }
                    else
                    {
                        if (vel.y > 0)
                            PlayerState = State.Walk_back;
                        else if (vel.y < 0)
                            PlayerState = State.Walk_front;
                    }
                }
                break;
            case State.Walk:
            case State.Walk_front:
            case State.Walk_back:
                if (velocity <= 0.1f)
                {
                    PlayerState = State.Idle;
                }
                else
                {
                    if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y))
                    {
                        PlayerState = State.Walk;
                    }
                    else
                    {
                        if (vel.y > 0)
                            PlayerState = State.Walk_back;
                        else if (vel.y < 0)
                            PlayerState = State.Walk_front;
                    }

                }
                break;
        }
        
        //Switch para cambiar animacion y aplicar propiedades
        switch (PlayerState){
            case State.Idle:
                animator.SetInteger("state", 0);
                break;
            case State.Walk:
                animator.SetInteger("state", 1);
                break;
            case State.Walk_front:
                animator.SetInteger("state", 2);
                break;
            case State.Walk_back:
                animator.SetInteger("state", 3);
                break;
            case State.Death:
                //Animation de muerte
                break;
        }
    }
}
