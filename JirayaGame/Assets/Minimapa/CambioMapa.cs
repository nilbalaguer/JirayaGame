using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CambioMapa : MonoBehaviour
{
    public GameObject minimapaPequeño;
    public GameObject minimapaGrande;
    public Camera camaraBigMap;
    private bool mapaGrandeActivo = false;

    public TextMeshProUGUI textoObjetos;
    public int objetosTotales = 3;
    public int objetosRecogidos = 0;
    public static CambioMapa Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        minimapaPequeño.SetActive(true);
        minimapaGrande.SetActive(false);
        camaraBigMap.gameObject.SetActive(false);
        ActualizarContadorObjetos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapaGrandeActivo = !mapaGrandeActivo;
            minimapaPequeño.SetActive(!mapaGrandeActivo);
            minimapaGrande.SetActive(mapaGrandeActivo);
            camaraBigMap.gameObject.SetActive(mapaGrandeActivo);
            Time.timeScale = mapaGrandeActivo ? 0 : 1;
        }
    }

    public void ActualizarContadorObjetos()
    {
        textoObjetos.text = "Objetos encontrados: " + objetosRecogidos + "/" + objetosTotales;
    }
}
