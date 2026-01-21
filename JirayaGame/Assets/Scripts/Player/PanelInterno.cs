using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
public class PanelInterno : MonoBehaviour
{
    public static PanelInterno Instance;
    public AudioSource audioSource;
    public AudioClip blip;
    public string[] paginas;
    private int paginaActual = 0;
    public TextMeshProUGUI textoPanel;
    public Button btnNext;

    private Animator animator;
    public float velocidadTypewriter = 0.03f;
    private Coroutine typeCoroutine;
    public int letrasPorSonido = 2; 
    private int contadorLetras = 0;
    private PlayerController player;

    void Awake()
    {
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }

        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
        GameObject playerScript = GameObject.Find("Player");
        player = playerScript.GetComponent<PlayerController>(); 

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameObject.SetActive(false);
        if (audioSource == null)
        {
            return;
        }   
    }
     void OnEnable()
    {
        paginaActual = 0;
        //textoPanel.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(btnNext.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AbrirPanelInterno(string[] textos)
    {
        paginas = textos;
        paginaActual = 0;
        textoPanel.text = "";

        gameObject.SetActive(true);
        
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
            player.maxSpeed = 5;
        }
    }

     public void DisablePanel()
    {
        gameObject.SetActive(false);
    }
    public void PlayBlip()
    {
        audioSource.pitch = Random.Range(0.8f, 1.0f);
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
