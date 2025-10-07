using UnityEngine;

public class CaidaDeObjetos : MonoBehaviour
{
    public GameObject objetoCaida;

    private float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 2)
        {
            GameObject ultimo = Instantiate(objetoCaida, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

            Destroy(ultimo, 4f);

            timer = 0;
        }
    }
}
