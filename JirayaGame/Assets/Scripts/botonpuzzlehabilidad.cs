using UnityEngine;
using UnityEngine.Tilemaps;
public class botonpuzzlehabilidad : MonoBehaviour
{
    int contador = 0;
    public scriptbotonpuzzlehabilidad[] botones;
    public TilemapRenderer puertaFinal;
    public TilemapCollider2D colliderPuertaFinal;
    public TilemapRenderer puertaPrincipal;
    public TilemapCollider2D colliderPuertaPrincipal;

    public AudioClip sonidoError;
    public AudioClip sonidoCorrecto;
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < botones.Length; i++)
        {
            SpriteRenderer spriteRenderer = botones[i].GetComponent<SpriteRenderer>();
            
            if (botones[i].botonActivado == true && contador == i)
            {
                
                contador++;
                spriteRenderer.enabled = true;
                botones[i].light2D.color = Color.green;

            }
            else if (botones[i].botonActivado == true && contador < i)
            {
                
                contador = 0;
                i = 0;
                for (int j = 0; j < botones.Length; j++)
                {
                    audioSource.PlayOneShot(sonidoError); //poner cooldown para que no suene todo el rato  
                    SpriteRenderer spriteRenderer1 = botones[j].GetComponent<SpriteRenderer>();
                    spriteRenderer1.enabled = false;
                    botones[j].botonActivado = false;
                    botones[j].light2D.color = new Color(251f/255f,242f/255f,53f/255f);
                    if (j == botones.Length - 1)
                    {
                        j = 0;
                        break;
                    }
                }
                Debug.Log("Puzzle de botones mal hecho");
                
                break;
            }
        }

        if (contador == botones.Length)
        {
            Debug.Log("Puzzle de botones completado");
            colliderPuertaFinal.enabled = false;
            puertaFinal.enabled = false;
            colliderPuertaPrincipal.enabled = true;
            puertaPrincipal.enabled = true;
            audioSource.PlayOneShot(sonidoCorrecto);
        }
    }

    IEnumerator cooldownsounderror()
    {
        sonidoError = false;
        yield return new WaitForSeconds(1f);
    }
}
