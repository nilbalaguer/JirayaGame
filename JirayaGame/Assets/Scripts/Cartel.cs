using UnityEngine;

public class Cartel : MonoBehaviour
{
    public GameObject panelCartel;
    public GameObject Canvas;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panelCartel.SetActive(false);
        Canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance < 2f)
        {
            Canvas.SetActive(true);
        }
        else
        {
            Canvas.SetActive(false);
        }

        if (distance < 2f && Input.GetKeyDown(KeyCode.E))
        {
            panelCartel.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        panelCartel.SetActive(false);
    }
}
