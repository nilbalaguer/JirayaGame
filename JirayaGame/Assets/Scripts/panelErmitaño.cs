using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class panelErmitaño : MonoBehaviour
{
    public string[] paginas;
    private int paginaActual = 0;
    public TextMeshProUGUI textoPanel;
    public Button btnNext;
    public bool hasTalked = false;

    private Animator animator;
    public Sprite iconoFlecha;
    public Sprite iconoCruz;
    public ScrollPanel scrollPanel;
    public tsunade tsunadeScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>(); 
        gameObject.SetActive(false);
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
            hasTalked = true;
        }
    }

    //Boton para el panel de tsunade

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
            tsunadeScript.EntregarRecompensa();
        }
    }

    //Boton para panel2 de tsunade
    public void NextTsunade2()
    {
        if (paginaActual < paginas.Length - 1)
        {
            paginaActual++;
            ShowPage();
        }
        else
        {
            animator.SetTrigger("Close");
            tsunadeScript.ultimoDialogo = Time.time;
        }
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }
    
}
