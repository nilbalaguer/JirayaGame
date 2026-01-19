using UnityEngine;

public class NpcMenu : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 2f;
    public float objetivo = 0.1f;

    private int currentIndex = 0;
     public float waitTime = 2f;
    private bool waiting = false;
    private float waitCounter;

    private Animator anim;
    private Vector2 lastDirection;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            return;
        }
        if (waiting)
        {
            waitCounter -= Time.deltaTime;

            anim.SetInteger("state", 0);

            if (waitCounter <= 0f)
            {
                waiting = false;
                NextPoint();
            }
            return;
        }

        Vector2 targetPos = patrolPoints[currentIndex].position;
        Vector2 posicionActual = transform.position;

        Vector2 direction = (targetPos - posicionActual).normalized;

        transform.position = Vector2.MoveTowards(
            posicionActual,
            targetPos,
            speed * Time.deltaTime
        );

        UpdateAnimation(direction);

        if (Vector2.Distance(posicionActual, targetPos) <= objetivo)
        {
            waiting = true;
            waitCounter = waitTime;
            NextPoint();
        }
    }

    void NextPoint()
    {
        currentIndex = (currentIndex + 1) % patrolPoints.Length;
    }

    void UpdateAnimation(Vector2 dir)
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
            transform.localScale = new Vector3(dir.x < 0 ? -3 : 3, 3, 3);
        }
        else
        {
            if (dir.y > 0)
                anim.SetInteger("state", 3);
            else
                anim.SetInteger("state", 2);
        }
    }
}