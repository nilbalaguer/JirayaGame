using UnityEngine;

public class DificultadBosScript : MonoBehaviour
{
    [SerializeField] scriptbotonpuzzlehabilidad boton1;
    [SerializeField] scriptbotonpuzzlehabilidad boton2;
    [SerializeField] BossController bossController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boton1.botonActivado)
        {
            boton1.botonActivado = false;
            boton2.botonActivado = false;
            boton2.light2D.color = Color.black;
            bossController.dificil = false;
        }
        else if (boton2.botonActivado)
        {
            boton2.botonActivado = false;
            boton1.botonActivado = false;
            boton1.light2D.color = Color.black;
            bossController.dificil = true;
        }
    }
}
