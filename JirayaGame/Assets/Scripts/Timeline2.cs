using UnityEngine;
using UnityEngine.Playables;
public class Timeline2 : MonoBehaviour
{
    public PlayableDirector timeline;
    private bool played = false;
    public static Timeline2 Instance;
    private PlayerController playerScript;
    private BehaviourErmitaño ermitañoScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        ermitañoScript = GameObject.FindWithTag("Ermitaño").GetComponent<BehaviourErmitaño>();
    }

    // Update is called once per frame
    void Update()
    {

        /*if (played)
        {
            playerScript.puedoMoverme = true;
            playerScript.human = false;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !played)
        {
            played = true;
            playerScript.puedoMoverme = false;
            //ermitañoScript.enabled = false;
            //ermitañoScript.transform.localScale = new Vector3(3, 3, 3);
            timeline.Play();

            timeline.stopped += OnTimelineFinished;
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        playerScript.puedoMoverme = true;
        playerScript.human = false;
    }
}
