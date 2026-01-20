using UnityEngine;

public class TextoFijo : MonoBehaviour
{
    private GameObject player;
    public GameObject textoFijo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //Colocar texto encima del npc correspondiente
        //gameObject.transform.position = new Vector3(npc.transform.position.x, npc.transform.position.y + 1.5f, npc.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance < 2f)
        {
            textoFijo.SetActive(true);
        }
        else
        {
            textoFijo.SetActive(false);
        }
    }
}
