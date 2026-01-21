using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public GameObject panelOpciones;
    public GameObject panelControles;
    public GameObject panelMenu;

    public GameObject botonInicialMenu;
    public GameObject botonInicialOpciones;
    public GameObject botonInicialControles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        EventSystem.current.SetSelectedGameObject(botonInicialMenu);
        SceneManager.LoadScene("Npc_Menu", LoadSceneMode.Additive);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (panelOpciones.activeSelf)
                CerrarOpciones();
            else if (panelControles.activeSelf)
                CerrarControles();
        }
    }

    public void Jugar()
    {
        //GraphicsSettings.renderPipelineAsset = Resources.Load<RenderPipelineAsset>("UniversalRenderPipelineAsset");
        SceneManager.LoadScene("SceneRaul"); 
        Time.timeScale = 1;
    }

    public void AbrirOpciones()
    {
        panelMenu.SetActive(false);
        panelOpciones.SetActive(true); 
        EventSystem.current.SetSelectedGameObject(botonInicialOpciones);
    }

    public void CerrarOpciones()
    {
        panelMenu.SetActive(true);
        panelOpciones.SetActive(false);
        EventSystem.current.SetSelectedGameObject(botonInicialMenu); 
    }

    public void AbrirControles()
    {
        panelMenu.SetActive(false);
        panelControles.SetActive(true);
        EventSystem.current.SetSelectedGameObject(botonInicialControles); 
    }

    public void CerrarControles()
    {
        panelMenu.SetActive(true);
        panelControles.SetActive(false); 
        EventSystem.current.SetSelectedGameObject(botonInicialMenu);
    }

    public void salir(){
        Application.Quit();
    }
}
