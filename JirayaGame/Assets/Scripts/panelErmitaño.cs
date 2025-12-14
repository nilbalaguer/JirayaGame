using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public BehaviourErmitaño ermitañoScript;
    //public StatesMachine playerScript;
    public PlayerController playerScript;
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

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(btnNext.gameObject);
    }

    public void ShowPage()
    {
        textoPanel.text = paginas[paginaActual];
        if (paginaActual >= paginas.Length - 1)
        {
            btnNext.image.sprite = iconoCruz;
        }
        else
        {
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
            playerScript.puedoMoverme = true;
            //ermitañoScript.esErmitañoTienda = true;
            //Mostrar cinematica ermitaño tienda
            //forzar mirar a la derecha
            ermitañoScript.enabled = false;
            ermitañoScript.transform.localScale = new Vector3(3, 3, 3);
            GameManager.Instance.ReproducirTimelineErmitañoTienda();
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
            playerScript.puedoMoverme = true;
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
            playerScript.puedoMoverme = true;
        }
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }
    
}
