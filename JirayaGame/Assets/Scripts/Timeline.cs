using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{
    public PlayableDirector timeline;
    private bool played = false;
    public BehaviourErmitaño ermitañoScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (played)
        {
            ermitañoScript.puedeMoverse = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !played)
        {
            played = true;
            timeline.Play();
        }
    }
}
