using UnityEngine;

public class puzzleButtonScript : MonoBehaviour
{

    public GameObject objetivo;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Activando objetivo");
        if (other.CompareTag("intObject"))
        {
            Debug.Log("Activando objetivo");
            objetivo.SetActive(true);
        }
    }
}
