using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class panelInfoManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip blip;
    public string[] paginas;
    private int paginaActual = 0;
    public TextMeshProUGUI textoPanel;
    public Button btnNext;
     private Animator animator;
    public Sprite iconoFlecha;
    public Sprite iconoCruz;
    public NpcStates npcScript;
    public float velocidadTypewriter = 0.03f;
    private Coroutine typeCoroutine;
    public int letrasPorSonido = 2; 
    private int contadorLetras = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>(); 
        if (audioSource == null)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        paginaActual = 0;
        textoPanel.gameObject.SetActive(true);
    }

    public void ShowPage()
    {
        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
        }
        typeCoroutine = StartCoroutine(TypeText(paginas[paginaActual]));
    }
    
    public void Next()
    {
        if (paginaActual < paginas.Length - 1)
        {
            paginaActual++;
            ShowPage();
        }
        else
        {
            animator.SetTrigger("Close");
        }
    }

    public void NextMision()
    {
        if (paginaActual < paginas.Length - 1)
        {
            paginaActual++;
            ShowPage();
        }
        else
        {
            animator.SetTrigger("Close");
            npcScript.dialogMisionMostrado = true;
        }
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    public void PlayBlip()
    {
        audioSource.pitch = Random.Range(0.9f, 1.2f);
        audioSource.PlayOneShot(blip);
    }

    //Efecto de typwriter para el texto del panel
    IEnumerator TypeText(string text)
    {
        textoPanel.text = "";
        contadorLetras = 0;

        foreach (char letter in text.ToCharArray())
        {
            textoPanel.text += letter;

            if (letter != ' ' && letter != '\n')
            {
                contadorLetras++;

                if (contadorLetras % letrasPorSonido == 0)
                {
                    PlayBlip();
                }
            }

            yield return new WaitForSeconds(velocidadTypewriter);
        }
    }
}
