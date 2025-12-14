using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PanelNpc : MonoBehaviour
{
    public string[] paginas;
    private int paginaActual = 0;

    public TextMeshProUGUI textoPanel;
    public Button btnNext;

    private Animator animator;
    public Sprite iconoFlecha;
    public Sprite iconoCruz;

    public NpcStates npcScript;
    public PlayerController playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
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
        textoPanel.text = paginas[paginaActual];
        if (paginaActual >= paginas.Length - 1)
        {
            //btnNext.GetComponentInChildren<TextMeshProUGUI>().text = "Cerrar";
            btnNext.image.sprite = iconoCruz;
        }
        else
        {
            //btnNext.GetComponentInChildren<TextMeshProUGUI>().text = "Siguiente";
            btnNext.image.sprite = iconoFlecha;
        }
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
}
