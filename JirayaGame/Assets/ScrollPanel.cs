using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollPanel : MonoBehaviour
{
    public GameObject text;
    private Animator animator;
    public npcReputacion reputacion;
    public bool hasTalked = false;
    public bool entregarObjeto = false;
    //public StatesMachine playerScript;
    public PlayerController playerScript;
    public NpcStates npcScript;
    public Misions misionsScript;
    private tsunade tsunadeScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        tsunadeScript = GameObject.FindWithTag("Tsunade").GetComponent<tsunade>();   
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        text.SetActive(false);
    }
    public void ShowText()
    {
        text.SetActive(true);
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

    //Botones panel tsunade

    public void btnAceptar()
    {
        playerScript.AceptarEntrega();
        entregarObjeto = true;
        animator.SetTrigger("Close");
        tsunadeScript.entregado = true;
    }
    
    public void btnRechazar()
    {
        entregarObjeto = false;
        animator.SetTrigger("Close");
        playerScript.ultimoDialogo = Time.time;
        playerScript.puedoMoverme = true;
        tsunadeScript.entregado = false;
    }
}
