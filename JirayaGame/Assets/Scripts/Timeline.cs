using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{
    public PlayableDirector timeline;
    private bool played = false;
    public BehaviourErmitaño ermitañoScript;
    public static Timeline Instance;
    private PlayerController playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
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
            played = true;
            playerScript.puedoMoverme = false;
            timeline.Play();
        }
    }
}
