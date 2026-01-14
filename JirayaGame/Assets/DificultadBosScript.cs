using UnityEngine;

public class DificultadBosScript : MonoBehaviour
{
    [SerializeField] scriptbotonpuzzlehabilidad boton1;
    [SerializeField] scriptbotonpuzzlehabilidad boton2;
    [SerializeField] BossController bossController;
    private AudioSource audioSource;
    [SerializeField] AudioClip sonidoBoton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
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
            audioSource.PlayOneShot(sonidoBoton);
        }
        else if (boton2.botonActivado)
        {
            boton2.botonActivado = false;
            boton1.botonActivado = false;
            boton1.light2D.color = Color.black;
            bossController.dificil = true;
            audioSource.PlayOneShot(sonidoBoton);
        }
    }
}
