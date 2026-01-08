using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class PanelNpc : MonoBehaviour
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
    public PlayerController playerScript;
    public float velocidadTypewriter = 0.03f;
    private Coroutine typeCoroutine;
    public int letrasPorSonido = 2; 
    private int contadorLetras = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
        if (audioSource == null)
        {
            return;
        }   
    }

    void OnEnable()
    {
        paginaActual = 0;
        //ShowPage();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(btnNext.gameObject);
    }

    public void ShowPage()
    {
        //textoPanel.text = paginas[paginaActual];
        /*if (paginaActual >= paginas.Length - 1)
        {
            //btnNext.GetComponentInChildren<TextMeshProUGUI>().text = "Cerrar";
            btnNext.image.sprite = iconoCruz;
        }
        else
        {
            //btnNext.GetComponentInChildren<TextMeshProUGUI>().text = "Siguiente";
            btnNext.image.sprite = iconoFlecha;
        }*/

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
            //npcScript.currentState = NpcStates.State.Idle;
            npcScript.NpcIntro = false;
            npcScript.necesitaAlejarse = true;
            npcScript.rb.simulated = true;
            npcScript.introTerminada = true;

            GameManager.Instance.FinalizarIntro(npcScript);
            playerScript.puedoMoverme = true;
        }
    }

    public void NextNpc()
    {
        if (paginaActual < paginas.Length - 1)
        {
            paginaActual++;
            ShowPage();
        }
        else
        {
            animator.SetTrigger("Close");
            if (npcScript != null)
            {
                npcScript.hasTalked = true;
                npcScript = null;
            }
            playerScript.puedoMoverme = true;
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
