using UnityEngine;
using UnityEngine.UI;

public class IconoMinimap : MonoBehaviour
{
    public GameObject iconoJugador;
    public GameObject IconoSapo;
    private PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.human == true)
        {
            iconoJugador.SetActive(true);
            IconoSapo.SetActive(false);
        }
        else
        {
            iconoJugador.SetActive(false);
            IconoSapo.SetActive(true);
        }
    }
}
