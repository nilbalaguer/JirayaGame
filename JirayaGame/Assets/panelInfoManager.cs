using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class panelInfoManager : MonoBehaviour
{
    public string[] paginas;
    private int paginaActual = 0;
    public TextMeshProUGUI textoPanel;
    public Button btnNext;
     private Animator animator;
    public Sprite iconoFlecha;
    public Sprite iconoCruz;
    public NpcStates npcScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>(); 
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
}
