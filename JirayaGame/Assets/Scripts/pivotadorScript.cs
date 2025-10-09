using UnityEngine;

public class pivotadorScript : MonoBehaviour
{

    [SerializeField] Transform[] posicionesSeguir;
    [SerializeField] float velocidad = 2;

    private bool move = true;

    private int posicionArrayLongitud = 0;
    private int siguiendo = 0;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicionArrayLongitud = posicionesSeguir.Length;

        siguiendo = Random.Range(0, posicionArrayLongitud);

        transform.position = posicionesSeguir[Random.Range(0, posicionArrayLongitud)].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!move)
        {
            timer += Time.deltaTime;

            if (timer > 0.4)
            {
                move = true;
                timer = 0;
            }
        }
        else if (move && Vector2.Distance(transform.position, posicionesSeguir[siguiendo].position) < 0.5)
        {
            move = false;

            siguiendo = Random.Range(0, posicionArrayLongitud);
        }
        else if (move)
        {
            transform.position = Vector2.MoveTowards(transform.position, posicionesSeguir[siguiendo].position, velocidad * Time.deltaTime);

        }
    }
}
