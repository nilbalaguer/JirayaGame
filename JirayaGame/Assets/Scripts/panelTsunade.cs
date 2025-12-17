using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class panelTsunade : MonoBehaviour
{
    private string[] paginas;
    private int paginaActual = 0;
    public string[] paginasFlor;
    public string[] paginasCollar;
    public string[] paginasPergamino;
    public string[] paginasFinal;

    public TextMeshProUGUI textoPanel;
    public Button btnNext;
    private Animator animator;
    public Sprite iconoFlecha;
    public Sprite iconoCruz;
    public ScrollPanel scrollPanel;
    public tsunade tsunadeScript;
    //public StatesMachine playerScript;
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

    // Update is called once per frame
    void Update()
    {

    }
    public void DialogoSetup(string nombreObjetoEntregado)
    {
        switch (nombreObjetoEntregado)
        {
            case "Flor":
                paginas = paginasFlor;
                break;

            case "CollarShizune":
                paginas = paginasCollar;
                break;

            case "PergaminoSagrado":
                paginas = paginasPergamino;
                break;
        }

        paginaActual = 0;
        ShowPage();
    }

    public void DialogoFinal()
    {
        paginas = paginasFinal;
        paginaActual = 0;
        ShowPage();
    }

    public void ShowPage()
    {
        textoPanel.text = paginas[paginaActual];
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
    }
    
    public void NextTsunade()
    {
        if (paginaActual < paginas.Length - 1)
        {
            paginaActual++;
            ShowPage();
        }
        else
        {
            animator.SetTrigger("Close");
            scrollPanel.entregarObjeto = false;
            tsunadeScript.entregado = false;
            tsunadeScript.EntregarRecompensa();
            playerScript.puedoMoverme = true;
        }
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}
