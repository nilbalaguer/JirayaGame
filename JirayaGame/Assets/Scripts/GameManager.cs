using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Inventario inventario;
    public StatesMachine player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PantallaJefeFinal()
    {

    }


    public void RecolectarMonedas()
    {

    }

    public void PantallaDerrota()
    {
        
    }
    
    //Mas funciones que afecten al hud del jugador y cosas que pasen en el juego
}
