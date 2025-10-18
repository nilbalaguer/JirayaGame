using UnityEngine;

public class NpcStates : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public enum State { Idle, Patrol};
    public State currentState;

    public Transform[] patrolPoints; 
    private int currentPointIndex = 0;
    public float speed = 2f;
    public float waitTime = 2f;        
    private float waitCounter;         
    private bool waiting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentState = State.Idle;
        waitCounter = waitTime;

        if (patrolPoints.Length > 0)
        {
            transform.position = patrolPoints[0].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;

        switch (currentState)
        {
            case State.Idle:

                if (!waiting)
                {
                    waiting = true;
                    waitCounter = waitTime;
                }
                else
                {
                    waitCounter -= Time.deltaTime;
                    if (waitCounter <= 0f)
                    {
                        waiting = false;
                        currentState = State.Patrol;
                    }
                }
                break;

            case State.Patrol:

                if (Vector2.Distance(pos, patrolPoints[currentPointIndex].position) < 0.1f)
                {
                    currentState = State.Idle;
                    waitCounter = waitTime;
                    NextPoint();
                }
                break;
        }

        switch (currentState)
        {
            case State.Idle:
                rb.linearVelocity = Vector2.zero;
                anim.SetInteger("state", 0);
                break;
            case State.Patrol:
                MoveTowards(patrolPoints[currentPointIndex].position);
                break;
        }

    }
        void MoveTowards(Vector2 target)
        {
            Vector2 dir = (target - (Vector2)transform.position).normalized;
            rb.linearVelocity = dir * speed;
            UpdateSpriteDirection(dir);
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
                anim.SetInteger("state", dir.y > 0 ? 3 : 2);  
            }
        
        }
}
