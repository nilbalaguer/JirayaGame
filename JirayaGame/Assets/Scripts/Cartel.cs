using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Cartel : MonoBehaviour
{
    public GameObject panelCartel;
    public GameObject Canvas;
    private Transform player;
    public TextMeshProUGUI textoCartel;
    public string contenidoCartel;
    private bool cartelAbierto = false;
    public Button btnCerrarCartel;
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

        if (distance < 2f && (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Y")))
        {
            Debug.Log("Cartel abierto");
            textoCartel.text = contenidoCartel;
            panelCartel.SetActive(true);
            cartelAbierto = true;
            EventSystem.current.SetSelectedGameObject(btnCerrarCartel.gameObject);
        }
    }

    public void ClosePanel()
    {
        panelCartel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
