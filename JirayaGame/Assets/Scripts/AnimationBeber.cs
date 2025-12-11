using UnityEngine;

public class AnimationBeber : MonoBehaviour
{
    private PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeberPocion()
    {
        player.BeberPocion();
    }
}
