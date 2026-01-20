using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
    public PlayableDirector timeline;
    private bool played = false;
    public BehaviourErmitaño ermitañoScript;
    public static Timeline Instance;
    private PlayerController playerScript;
    private bool colisionado = false;
    public bool misionIniciada = false;
    public GameObject panelErmitañoMision;
    public GameObject BarreraErmitaño;
    public GameObject[] barreras;
    public AudioSource musicaFondo;
    public AudioClip musicaMisionErmitaño;
    public AudioClip musicaNormal;

    void Awake()
    {
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        panelErmitañoMision.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (played && ermitañoScript != null)
        {
            ermitañoScript.puedeMoverse = true;
        }
        else
        {
            return;
        }

        if (played)
        {
            playerScript.puedoMoverme = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !played)
        {
            colisionado = true;
            played = true;
            playerScript.maxSpeed = 0;
            timeline.Play();
        }
    }

    public void OnCinematicEnd()
    {
        played = true;
        playerScript.maxSpeed = 5;
        misionIniciada = true;
        panelErmitañoMision.SetActive(true);
        ScreenCinematic.Instance.ActivarCinematic();
        ermitañoScript.canvasPatrol.SetActive(true);
        foreach (GameObject barrera in barreras)
        {
            if (barrera != null)
            {
                barrera.SetActive(true);
            }
        }

        musicaFondo.clip = musicaMisionErmitaño;
        musicaFondo.Play();
    }
}
