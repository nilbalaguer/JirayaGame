using UnityEngine;

public class CaidaDeObjetos : MonoBehaviour
{
    public GameObject objetoCaida;

    public GameObject objetoPuntosPositivos;

    public GameObject objetoPuntosNegativos;

    private float timer = 0;

    private float timerPoints = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timerPoints += Time.deltaTime;

        if (timer >= 2)
        {
            GameObject ultimo = Instantiate(objetoCaida, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

            Destroy(ultimo, 4f);

            timer = 0;
        }

        if (timerPoints >= 5)
        {
            int random = Random.Range(0, 2);

            Debug.Log(random);

            if (random >= 1)
            {
                GameObject ultimo = Instantiate(objetoPuntosPositivos, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

                Destroy(ultimo, 10f);

                timerPoints = 0;
            }
            else
            {
                GameObject ultimo = Instantiate(objetoPuntosNegativos, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

                Destroy(ultimo, 10f);

                timerPoints = 0;
            }

            
        }
    }
}
