using UnityEngine;

public class puzzleButtonScript : MonoBehaviour
{

    public GameObject objetivo;

    private int objetosdentro = 0;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("intObject") || other.CompareTag("Player"))
        {
            ++objetosdentro;
            objetivo.SetActive(true);
        }
        
    }
    
    void OnTriggerExit2D(Collider2D other)
    {

       if (other.CompareTag("intObject") || other.CompareTag("Player"))
        {
            objetosdentro--;

            
            if (objetosdentro < 0)
            {
                objetosdentro = 0;
            }
                

         
            if (objetosdentro == 0)
            {
                objetivo.SetActive(false);
            }
        }
    }
}
