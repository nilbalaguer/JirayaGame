using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject panelOpciones;
    public GameObject panelControles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Jugar()
    {
        //GraphicsSettings.renderPipelineAsset = Resources.Load<RenderPipelineAsset>("UniversalRenderPipelineAsset");
        SceneManager.LoadScene("pruebaEstados"); 
    }

    public void AbrirOpciones()
    {
        panelOpciones.SetActive(true); 
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false); 
    }

    public void AbrirControles()
    {
        panelControles.SetActive(true); 
    }

    public void CerrarControles()
    {
        panelControles.SetActive(false); 
    }

    public void salir(){
        Application.Quit();
    }
}
