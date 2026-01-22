using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
public class Timeline2 : MonoBehaviour
{
    public PlayableDirector timeline;
    private bool played = false;
    private bool colisionado = false;
    public static Timeline2 Instance;
    public PlayerController playerScript;
    private BehaviourErmitaño ermitañoScript;
    private Rigidbody2D rigidBody;
    //public Image habilidadRana;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        ermitañoScript = GameObject.FindWithTag("Ermitaño").GetComponent<BehaviourErmitaño>();
        rigidBody = playerScript.GetComponent<Rigidbody2D>();
        //habilidadRana.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        /*if (played)
        {
            playerScript.puedoMoverme = true;
            playerScript.human = false;
        }*/
        if (!played && colisionado)
        {
            playerScript.maxSpeed = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !played && !GameManager.Instance.timelineSapoMostrado)
        {
            colisionado = true;
            //playerScript.puedoMoverme = false;
            playerScript.maxSpeed = 0;
            ermitañoScript.enabled = false;
            ermitañoScript.transform.localScale = new Vector3(3, 3, 3);
            timeline.Play();

            //timeline.stopped += OnTimelineFinished;
        }
    }

    public void OnCinematicEnd()
    {
        played = true;
        //playerScript.puedoMoverme = true;
        playerScript.maxSpeed = 5;
        playerScript.human = false;
        GameManager.Instance.puedeTransformarse = true;
        GameManager.Instance.timelineSapoMostrado = true;
        //habilidadRana.enabled = true;
    }
}
