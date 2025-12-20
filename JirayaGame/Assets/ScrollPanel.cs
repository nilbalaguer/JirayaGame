using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ScrollPanel : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip blip;
    public GameObject text;
    public TextMeshProUGUI textPanel;
    public TextMeshProUGUI nombreNpc;
    public Animator animator;
    public npcReputacion reputacion;
    public bool hasTalked = false;
    public bool entregarObjeto = false;
    //public StatesMachine playerScript;
    public PlayerController playerScript;
    public NpcStates npcScript;
    public Misions misionsScript;
    private tsunade tsunadeScript;
    public GameObject flecha;
    public float velocidadTypewriter = 0.03f;
    private Coroutine typeCoroutine;
    public string textoMision;
    public int letrasPorSonido = 2; 
    private int contadorLetras = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        tsunadeScript = GameObject.FindWithTag("Tsunade").GetComponent<tsunade>();
        flecha.SetActive(false);
        if (audioSource == null)
        {
            return;
        }   
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        flecha.SetActive(false); 
    }
    void OnEnable()
    {
        text.SetActive(false);
    }
    public void ShowText()
    {
        text.SetActive(true);
        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
        }
        typeCoroutine = StartCoroutine(TypeText(textoMision));   
    }

    public void ClosePanel()
    {
        if (animator != null)
        {
            animator.SetTrigger("Close");
        }
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    public void HideText()
    {
        text.SetActive(false);
    }

    public void botonSi()
    {
        //reputacion.RespuestaPositiva();
        animator.SetTrigger("Close");

        if (npcScript != null)
        {
            npcScript.hasTalked = true;
            npcScript = null;
        }
        misionsScript.MostrarMision();
        playerScript.puedoMoverme = true;
        if (misionsScript.tipoMision == Misions.MisionTipo.HablarConNpc)
        {
            GameObject npcDest = GameObject.Find(misionsScript.npcDestino);
            if (npcDest != null)
            {
                NpcStates npcDestinoScript = npcDest.GetComponent<NpcStates>();

                npcDestinoScript.misionNpc = misionsScript;
                npcDestinoScript.misionNpc.misionActiva = true;       
                npcDestinoScript.npcIcono.sprite = npcDestinoScript.iconoIntro;
                npcDestinoScript.canvasImagen.SetActive(true);
            }
            else
            {
                Debug.Log("No se encontró el NPC destino");
            }
        }
    }

    public void botonNo()
    {
        //reputacion.RespuestaNegativa();
        animator.SetTrigger("Close");

        if (npcScript != null)
        {
            npcScript.hasTalked = true;
            npcScript = null;
        }
        playerScript.puedoMoverme = true;
    }

    //Asignar texto de la mision correspondiente
    public void AsignarTextoMision(NpcStates npc)
    {
        npcScript = npc;
        textoMision = npc.dialogMision;
        nombreNpc.text = npc.nameNpc;
    }

    //Botones panel tsunade

    public void btnAceptar()
    {
        playerScript.AceptarEntrega();
        if (!playerScript.inventario.modoEntrega)
        {
            entregarObjeto = true;
            tsunadeScript.entregado = true;

            animator.SetTrigger("Close");
        }
        else
        {
            animator.SetTrigger("Close");
            entregarObjeto = false;
            flecha.SetActive(true);
        }
        //tsunadeScript.entregado = true;
    }
    
    public void btnRechazar()
    {
        entregarObjeto = false;
        animator.SetTrigger("Close");
        playerScript.ultimoDialogo = Time.time;
        playerScript.puedoMoverme = true;
        tsunadeScript.entregado = false;
    }

    public void PlayBlip()
    {
        audioSource.pitch = Random.Range(0.9f, 1.2f);
        audioSource.PlayOneShot(blip);
    }

    //Efecto de typwriter para el texto del panel
    IEnumerator TypeText(string text)
    {
        textPanel.text = "";
        contadorLetras = 0;

        foreach (char letter in text.ToCharArray())
        {
            textPanel.text += letter;

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
